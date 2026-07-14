using System.Globalization;
using System.Threading.RateLimiting;
using CoderSight.Core.Blocks;
using CoderSight.Core.Services;
using CoderSight.Data;
using CoderSight.Data.Seed;
using CoderSight.Data.Services;
using CoderSight.Web;
using CoderSight.Web.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<CmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
})
.AddEntityFrameworkStores<CmsDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.AccessDeniedPath = "/admin/access-denied";
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // Auth endpoints: 5 requests per minute per IP
    options.AddPolicy("auth", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1)
            }));

    // Form endpoints (contact, newsletter, comments): 10 requests per minute per IP
    options.AddPolicy("form", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1)
            }));
});

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var blockRegistry = new BlockRegistry();
blockRegistry.DiscoverBlocks(typeof(IBlockData).Assembly);
builder.Services.AddSingleton(blockRegistry);

builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<ISiteSettingsService, SiteSettingsService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<INavMenuService, NavMenuService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.AddScoped<INewsletterService, NewsletterService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ITurnstileService, TurnstileService>();
builder.Services.AddSingleton<INotificationQueue, NotificationQueue>();
builder.Services.AddHostedService<CoderSight.Web.Services.NotificationBackgroundService>();
builder.Services.AddScoped<IMediaService>(sp =>
{
    var dbFactory = sp.GetRequiredService<IDbContextFactory<CmsDbContext>>();
    var env = sp.GetRequiredService<IWebHostEnvironment>();
    var uploadPath = Path.Combine(env.WebRootPath, "uploads");
    return new MediaService(dbFactory, uploadPath);
});

var app = builder.Build();

blockRegistry.RegisterComponentTypes(typeof(App).Assembly);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/uploads"),
    branch => branch.UseStaticFiles()
);
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("fa") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders =
    [
        new UrlSegmentCultureProvider(),
        new QueryStringRequestCultureProvider(),
        new CookieRequestCultureProvider()
    ]
});

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.UseAntiforgery();

app.MapPost("/api/auth/login", async (
    HttpContext httpContext,
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ITurnstileService turnstile) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    var returnUrl = form["returnUrl"].ToString();
    var safeReturn = IsLocalUrl(returnUrl) ? returnUrl : null;

    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Redirect(safeReturn is null ? "/admin/login?error=1" : $"/login?error=1&returnUrl={Uri.EscapeDataString(safeReturn)}");

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Redirect(safeReturn is null ? "/admin/login?error=captcha" : $"/login?error=captcha&returnUrl={Uri.EscapeDataString(safeReturn)}");

    var email = form["email"].ToString().Trim();
    var password = form["password"].ToString();

    if (email.Length > 256 || password.Length > 128)
        return Results.Redirect(safeReturn is null ? "/admin/login?error=1" : $"/login?error=1&returnUrl={Uri.EscapeDataString(safeReturn)}");

    var user = await userManager.FindByEmailAsync(email);
    if (user is null)
    {
        var errorRedirect = safeReturn is null ? "/admin/login?error=1" : $"/login?error=1&returnUrl={Uri.EscapeDataString(safeReturn)}";
        return Results.Redirect(errorRedirect);
    }

    var result = await signInManager.PasswordSignInAsync(user, password, isPersistent: true, lockoutOnFailure: true);
    if (result.IsLockedOut)
        return Results.Redirect(safeReturn is null ? "/login?error=locked" : $"/login?error=locked&returnUrl={Uri.EscapeDataString(safeReturn)}");
    if (!result.Succeeded)
    {
        var errorRedirect = safeReturn is null ? "/admin/login?error=1" : $"/login?error=1&returnUrl={Uri.EscapeDataString(safeReturn)}";
        return Results.Redirect(errorRedirect);
    }

    var isAdmin = await userManager.IsInRoleAsync(user, "Admin") || await userManager.IsInRoleAsync(user, "Editor");
    var redirect = safeReturn ?? (isAdmin ? "/admin" : "/");
    return Results.Redirect(redirect);
}).DisableAntiforgery().RequireRateLimiting("auth");

app.MapPost("/api/auth/register", async (
    HttpContext httpContext,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ISiteSettingsService settingsService,
    ITurnstileService turnstile) =>
{
    var siteSettings = await settingsService.GetAsync();
    if (siteSettings is not null && !siteSettings.EnableUserRegistration)
        return Results.Redirect("/register");

    var form = await httpContext.Request.ReadFormAsync();
    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Redirect("/register");

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Redirect("/register?error=captcha");

    var displayName = form["displayName"].ToString().Trim();
    var email = form["email"].ToString().Trim();
    var password = form["password"].ToString();
    var confirmPassword = form["confirmPassword"].ToString();
    var returnUrl = form["returnUrl"].ToString();
    var safeReturn = IsLocalUrl(returnUrl) ? returnUrl : null;
    var returnSuffix = safeReturn is not null ? $"&returnUrl={Uri.EscapeDataString(safeReturn)}" : "";

    if (displayName.Length > 100 || email.Length > 256 || password.Length > 128)
        return Results.Redirect($"/register?error=fields{returnSuffix}");

    if (string.IsNullOrEmpty(displayName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        return Results.Redirect($"/register?error=fields{returnSuffix}");

    if (password != confirmPassword)
        return Results.Redirect($"/register?error=password{returnSuffix}");

    if (!email.Contains('@') || email.Length < 5)
        return Results.Redirect($"/register?error=fields{returnSuffix}");

    if (await userManager.FindByEmailAsync(email) is not null)
        return Results.Redirect($"/register?error=exists{returnSuffix}");

    var user = new ApplicationUser
    {
        UserName = email,
        Email = email,
        DisplayName = displayName,
        EmailConfirmed = true
    };

    var result = await userManager.CreateAsync(user, password);
    if (!result.Succeeded)
        return Results.Redirect($"/register?error=password{returnSuffix}");

    await signInManager.SignInAsync(user, isPersistent: true);
    return Results.Redirect(safeReturn ?? "/");
}).DisableAntiforgery().RequireRateLimiting("auth");

app.MapGet("/api/auth/logout", async (HttpContext httpContext, SignInManager<ApplicationUser> signInManager) =>
{
    var returnUrl = httpContext.Request.Query["returnUrl"].ToString();
    await signInManager.SignOutAsync();
    return Results.Redirect(IsLocalUrl(returnUrl) ? returnUrl : "/");
});

app.MapPost("/api/auth/forgot-password", async (
    HttpContext httpContext,
    UserManager<ApplicationUser> userManager,
    IEmailService emailService,
    ISiteSettingsService settingsService,
    ITurnstileService turnstile) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Redirect("/forgot-password?status=sent");

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Redirect("/forgot-password?status=captcha");

    var email = form["email"].ToString().Trim();

    if (string.IsNullOrEmpty(email) || !email.Contains('@') || email.Length > 256)
        return Results.Redirect("/forgot-password?status=sent");

    var user = await userManager.FindByEmailAsync(email);
    if (user is not null)
    {
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var settings = await settingsService.GetAsync();
        var siteUrl = settings?.SiteUrl?.TrimEnd('/') ?? $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        var siteName = settings?.SiteName ?? "CoderSight";
        var resetUrl = $"{siteUrl}/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}";

        var body = $"""
          <h1 style="font-size:18px;color:#020817;margin:0 0 12px;">Password Reset</h1>
          <p style="font-size:14px;color:#64748b;line-height:1.6;margin:0 0 24px;">
            We received a request to reset your password. Click the button below to choose a new one. If you didn't request this, you can safely ignore this email.
          </p>
          <div style="text-align:center;margin:0 0 24px;">
            <a href="{resetUrl}" style="display:inline-block;background:#2563eb;color:#ffffff;padding:12px 28px;border-radius:8px;text-decoration:none;font-size:14px;font-weight:600;">
              Reset Password
            </a>
          </div>
          <p style="font-size:12px;color:#9ca3af;line-height:1.5;margin:0;">
            This link will expire after use. If the button doesn't work, copy and paste this URL into your browser:<br/>
            <span style="word-break:break-all;">{resetUrl}</span>
          </p>
        """;
        var html = CoderSight.Data.Services.EmailTemplateHelper.Wrap(siteName, settings?.LogoUrl, siteUrl, body);

        try
        {
            await emailService.SendAsync(email, $"Reset your {siteName} password", html);
        }
        catch
        {
            // Don't reveal whether email exists
        }
    }

    return Results.Redirect("/forgot-password?status=sent");
}).DisableAntiforgery().RequireRateLimiting("auth");

app.MapPost("/api/auth/reset-password", async (
    HttpContext httpContext,
    UserManager<ApplicationUser> userManager,
    ITurnstileService turnstile) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Redirect("/reset-password?status=invalid");

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Redirect("/reset-password?status=captcha");

    var email = form["email"].ToString().Trim();
    var token = form["token"].ToString();
    var password = form["password"].ToString();
    var confirmPassword = form["confirmPassword"].ToString();

    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        return Results.Redirect("/reset-password?status=invalid");

    if (string.IsNullOrEmpty(password) || password.Length < 8)
        return Results.Redirect($"/reset-password?status=error&detail={Uri.EscapeDataString("Password must be at least 8 characters.")}&email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");

    if (password != confirmPassword)
        return Results.Redirect($"/reset-password?status=error&detail={Uri.EscapeDataString("Passwords do not match.")}&email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");

    var user = await userManager.FindByEmailAsync(email);
    if (user is null)
        return Results.Redirect("/reset-password?status=invalid");

    var result = await userManager.ResetPasswordAsync(user, token, password);
    if (!result.Succeeded)
    {
        var error = result.Errors.FirstOrDefault()?.Description ?? "Failed to reset password.";
        return Results.Redirect($"/reset-password?status=error&detail={Uri.EscapeDataString(error)}&email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(token)}");
    }

    return Results.Redirect("/reset-password?status=success");
}).DisableAntiforgery().RequireRateLimiting("auth");

app.MapPost("/api/newsletter/subscribe", async (HttpContext httpContext, INewsletterService newsletter, ITurnstileService turnstile) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Json(new { success = false, message = "Invalid request." });

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Json(new { success = false, message = "Verification failed. Please try again." });

    var email = form["email"].ToString().Trim();
    if (string.IsNullOrEmpty(email) || !email.Contains('@') || email.Length > 256 || email.Length < 5)
        return Results.Json(new { success = false, message = "Please enter a valid email." });

    var (success, message) = await newsletter.SubscribeAsync(email);
    return Results.Json(new { success, message });
}).DisableAntiforgery().RequireRateLimiting("form");

app.MapGet("/api/newsletter/unsubscribe", async (string token, INewsletterService newsletter) =>
{
    var result = await newsletter.UnsubscribeAsync(token);
    return result
        ? Results.Redirect("/unsubscribe?status=success")
        : Results.Redirect("/unsubscribe?status=invalid");
});

app.MapPost("/api/contact", async (HttpContext httpContext, IContactService contactService, ITurnstileService turnstile) =>
{
    var form = await httpContext.Request.ReadFormAsync();
    if (!string.IsNullOrEmpty(form["cs_hp"].ToString()))
        return Results.Json(new { success = false, message = "Invalid request." });

    if (!await turnstile.VerifyAsync(form["cf-turnstile-response"], httpContext.Connection.RemoteIpAddress?.ToString()))
        return Results.Json(new { success = false, message = "Verification failed. Please try again." });

    var name = form["name"].ToString().Trim();
    var email = form["email"].ToString().Trim();
    var message = form["message"].ToString().Trim();
    var subject = form["subject"].ToString().Trim();
    var pageSlug = form["pageSlug"].ToString().Trim();

    if (name.Length > 200 || email.Length > 256 || message.Length > 5000 || subject.Length > 300)
        return Results.Json(new { success = false, message = "Input exceeds maximum length." });
    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(message))
        return Results.Json(new { success = false, message = "Please fill in all required fields." });
    if (!email.Contains('@') || email.Length < 5)
        return Results.Json(new { success = false, message = "Please enter a valid email." });

    await contactService.SubmitAsync(new CoderSight.Core.Entities.ContactMessage
    {
        Name = name,
        Email = email,
        Message = message,
        Subject = string.IsNullOrEmpty(subject) ? null : subject,
        PageSlug = string.IsNullOrEmpty(pageSlug) ? null : pageSlug
    });
    return Results.Json(new { success = true, message = "Thank you! Your message has been sent." });
}).DisableAntiforgery().RequireRateLimiting("form");

app.MapGet("/uploads/{fileName}", (string fileName, IWebHostEnvironment env) =>
{
    var safeName = Path.GetFileName(fileName);
    var filePath = Path.Combine(env.WebRootPath, "uploads", safeName);
    if (!File.Exists(filePath))
        return Results.NotFound();

    var contentType = safeName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ? "image/png"
        : safeName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || safeName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ? "image/jpeg"
        : safeName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ? "image/gif"
        : safeName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) ? "image/webp"
        : safeName.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ? "image/svg+xml"
        : safeName.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ? "image/x-icon"
        : safeName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ? "application/pdf"
        : safeName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) ? "video/mp4"
        : "application/octet-stream";

    return Results.File(filePath, contentType);
});

app.MapGet("/api/debug/test-notification", (INotificationQueue queue, ILogger<Program> logger) =>
{
    var testId = Guid.NewGuid();
    logger.LogWarning("TEST: Enqueuing test notification with ID {Id}", testId);
    queue.EnqueuePostNotification(new PostNotification(testId, testId, "https://localhost"));
    return Results.Json(new { message = "Test notification enqueued. Check console for background service output.", testId });
});

app.MapGet("/api/posts", async (
    string? culture, int? page, int? pageSize, string? search, string? category,
    IBlogService blogService, ISiteSettingsService settingsService) =>
{
    var lang = culture ?? "en";
    var p = Math.Max(1, page ?? 1);
    var ps = Math.Clamp(pageSize ?? 9, 1, 50);
    var s = string.IsNullOrWhiteSpace(search) ? null : search.Trim();
    if (s is not null && s.Length > 200) s = s[..200];
    var cat = string.IsNullOrWhiteSpace(category) ? null : category.Trim();

    var settings = await settingsService.GetAsync();
    var multilingual = settings?.EnableMultilingual ?? true;

    var (posts, totalCount) = await blogService.GetPostsAsync(lang, p, ps, categorySlug: cat, search: s);

    var items = posts.Select(post => new
    {
        url = multilingual ? $"/{post.Page.Culture}/{post.Page.Slug}" : $"/{post.Page.Slug}",
        title = post.Page.Title,
        excerpt = post.Excerpt,
        featuredImageUrl = post.FeaturedImageUrl,
        categories = string.Join(", ", post.BlogPostCategories.Select(bc => bc.Category.Name)),
        readTimeMinutes = post.ReadTimeMinutes,
        publishedAt = post.Page.PublishedAt?.ToString("MMM dd, yyyy")
    });

    return Results.Json(new { items, totalCount, page = p, pageSize = ps });
});

app.MapGet("/api/categories", async (string? culture, IBlogService blogService) =>
{
    var lang = culture ?? "en";
    var categories = await blogService.GetCategoriesAsync(lang);
    var items = categories.Select(c => new { slug = c.Slug, name = c.Name });
    return Results.Json(new { items });
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await SeedData.InitializeAsync(app.Services);

app.Run();

static bool IsLocalUrl(string? url) =>
    !string.IsNullOrEmpty(url)
    && url.StartsWith('/')
    && !url.StartsWith("//")
    && !url.StartsWith("/\\");
