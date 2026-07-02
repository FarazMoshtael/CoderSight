using CoderSight.Core.Services;
using CoderSight.Data;
using CoderSight.Data.Services;
using Microsoft.EntityFrameworkCore;


namespace CoderSight.Web.Services;

public class NotificationBackgroundService : BackgroundService
{
    private readonly INotificationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NotificationBackgroundService> _logger;

    public NotificationBackgroundService(
        INotificationQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<NotificationBackgroundService> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationBackgroundService started, waiting for notifications...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var notification = await _queue.DequeueAsync(stoppingToken);
                _logger.LogInformation("Received notification for post {PostId}, processing...", notification.BlogPostId);
                await ProcessNotificationAsync(notification, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing post notification");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private async Task ProcessNotificationAsync(PostNotification notification, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<CmsDbContext>>();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        await using var db = await dbFactory.CreateDbContextAsync(ct);

        var post = await db.BlogPosts
            .Include(bp => bp.Page)
            .FirstOrDefaultAsync(bp => bp.Id == notification.BlogPostId, ct);
        if (post is null) return;

        var settings = await db.SiteSettings.FirstOrDefaultAsync(ct);
        if (settings is null)
        {
            _logger.LogWarning("No site settings found, skipping notification");
            return;
        }

        if (string.IsNullOrEmpty(settings.SmtpHost))
        {
            _logger.LogWarning("SMTP not configured in site settings, skipping email notifications. Configure SMTP in Admin → Settings.");
            return;
        }

        var subscribers = await db.Set<CoderSight.Core.Entities.Subscriber>()
            .Where(s => s.IsActive)
            .ToListAsync(ct);
        if (subscribers.Count == 0)
        {
            _logger.LogInformation("No active subscribers, skipping notification");
            return;
        }

        var siteUrl = notification.SiteUrl.TrimEnd('/');
        var page = post.Page ?? await db.Pages.FindAsync(new object[] { notification.PageId }, ct);
        var postUrl = $"{siteUrl}/{page?.Culture ?? "en"}/{page?.Slug}";
        var siteName = settings.SiteName;
        var title = post.Page?.Title ?? page?.Title ?? "New Article";
        var subject = $"New on {siteName}: {title}";

        var sent = 0;
        var failed = 0;

        foreach (var sub in subscribers)
        {
            if (ct.IsCancellationRequested) break;

            var unsubUrl = $"{siteUrl}/api/newsletter/unsubscribe?token={sub.UnsubscribeToken}";
            var image = !string.IsNullOrEmpty(post.FeaturedImageUrl)
                ? $"""<img src="{EmailTemplateHelper.MakeAbsolute(post.FeaturedImageUrl, siteUrl)}" alt="{EmailTemplateHelper.Encode(title)}" style="width:100%;max-height:300px;object-fit:cover;border-radius:8px;margin-bottom:20px;" />"""
                : "";
            var body = $"""
              {image}
              <h1 style="font-size:22px;color:#020817;margin:0 0 12px;">{EmailTemplateHelper.Encode(title)}</h1>
              <p style="font-size:15px;color:#64748b;line-height:1.6;margin:0 0 24px;">{EmailTemplateHelper.Encode(post.Excerpt)}</p>
              <a href="{postUrl}" style="display:inline-block;background:#2563eb;color:#ffffff;padding:12px 28px;border-radius:8px;text-decoration:none;font-size:14px;font-weight:600;">
                Read Article
              </a>
            """;
            var footer = $"""
              <p>You received this because you subscribed to {EmailTemplateHelper.Encode(siteName)}.</p>
              <a href="{unsubUrl}" style="color:#9ca3af;text-decoration:underline;">Unsubscribe</a>
            """;
            var html = EmailTemplateHelper.Wrap(siteName, settings.LogoUrl, siteUrl, body, footer);

            try
            {
                await emailService.SendAsync(sub.Email, subject, html);
                sent++;
                await Task.Delay(100, ct);
            }
            catch (Exception ex)
            {
                failed++;
                _logger.LogWarning(ex, "Failed to send notification to {Email}", sub.Email);
            }
        }

        _logger.LogInformation("Post notification complete: {Sent} sent, {Failed} failed, for post '{Title}'", sent, failed, title);
    }
}
