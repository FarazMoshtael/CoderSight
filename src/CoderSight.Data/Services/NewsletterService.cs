using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class NewsletterService : INewsletterService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;
    private readonly IEmailService _emailService;

    public NewsletterService(IDbContextFactory<CmsDbContext> dbFactory, IEmailService emailService)
    {
        _dbFactory = dbFactory;
        _emailService = emailService;
    }

    public async Task<(bool Success, string Message)> SubscribeAsync(string email, string? name = null)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.Set<Subscriber>().FirstOrDefaultAsync(s => s.Email == email);

        if (existing is not null)
        {
            if (existing.IsActive) return (false, "Already subscribed.");
            existing.IsActive = true;
            existing.UnsubscribedAt = null;
            existing.SubscribedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return (true, "Welcome back! You've been re-subscribed.");
        }

        db.Set<Subscriber>().Add(new Subscriber
        {
            Id = Guid.NewGuid(),
            Email = email.Trim().ToLowerInvariant(),
            Name = name?.Trim()
        });
        await db.SaveChangesAsync();
        return (true, "Successfully subscribed!");
    }

    public async Task<bool> UnsubscribeAsync(string token)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var subscriber = await db.Set<Subscriber>().FirstOrDefaultAsync(s => s.UnsubscribeToken == token);
        if (subscriber is null) return false;

        subscriber.IsActive = false;
        subscriber.UnsubscribedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Subscriber>> GetSubscribersAsync(bool activeOnly = true)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var query = db.Set<Subscriber>().AsQueryable();
        if (activeOnly) query = query.Where(s => s.IsActive);
        return await query.OrderByDescending(s => s.SubscribedAt).ToListAsync();
    }

    public async Task<int> GetActiveCountAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Set<Subscriber>().CountAsync(s => s.IsActive);
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var sub = await db.Set<Subscriber>().FindAsync(id);
        if (sub is not null)
        {
            db.Set<Subscriber>().Remove(sub);
            await db.SaveChangesAsync();
        }
    }

    public async Task SendNewPostNotificationAsync(BlogPost post, string siteUrl)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var settings = await db.SiteSettings.FirstOrDefaultAsync();
        var subscribers = await db.Set<Subscriber>()
            .Where(s => s.IsActive)
            .ToListAsync();

        if (subscribers.Count == 0 || settings is null) return;

        var siteName = settings.SiteName;
        var baseUrl = siteUrl.TrimEnd('/');
        var page = await db.Pages.FindAsync(post.PageId);
        var postUrl = $"{baseUrl}/en/{page?.Slug}";
        var title = post.Page?.Title ?? page?.Title ?? "New Article";

        var image = !string.IsNullOrEmpty(post.FeaturedImageUrl)
            ? $"""<img src="{EmailTemplateHelper.MakeAbsolute(post.FeaturedImageUrl, baseUrl)}" alt="{EmailTemplateHelper.Encode(title)}" style="width:100%;max-height:300px;object-fit:cover;border-radius:8px;margin-bottom:20px;" />"""
            : "";
        var body = $"""
          {image}
          <h1 style="font-size:22px;color:#020817;margin:0 0 12px;">{EmailTemplateHelper.Encode(title)}</h1>
          <p style="font-size:15px;color:#64748b;line-height:1.6;margin:0 0 24px;">{EmailTemplateHelper.Encode(post.Excerpt)}</p>
          <a href="{postUrl}" style="display:inline-block;background:#2563eb;color:#ffffff;padding:12px 28px;border-radius:8px;text-decoration:none;font-size:14px;font-weight:600;">
            Read Article
          </a>
        """;

        foreach (var sub in subscribers)
        {
            var unsubUrl = $"{baseUrl}/api/newsletter/unsubscribe?token={sub.UnsubscribeToken}";
            var footer = $"""
              <p>You received this because you subscribed to {EmailTemplateHelper.Encode(siteName)}.</p>
              <a href="{unsubUrl}" style="color:#9ca3af;text-decoration:underline;">Unsubscribe</a>
            """;
            var html = EmailTemplateHelper.Wrap(siteName, settings.LogoUrl, baseUrl, body, footer);
            try
            {
                await _emailService.SendAsync(sub.Email, $"New on {siteName}: {title}", html);
                await Task.Delay(100);
            }
            catch { }
        }
    }
}
