using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class ContactService : IContactService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;
    private readonly IEmailService _emailService;

    public ContactService(IDbContextFactory<CmsDbContext> dbFactory, IEmailService emailService)
    {
        _dbFactory = dbFactory;
        _emailService = emailService;
    }

    public async Task<ContactMessage> SubmitAsync(ContactMessage msg)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        msg.Id = Guid.NewGuid();
        msg.SubmittedAt = DateTime.UtcNow;
        db.Set<ContactMessage>().Add(msg);
        await db.SaveChangesAsync();

        try
        {
            var settings = await db.SiteSettings.FirstOrDefaultAsync();
            if (settings is not null && !string.IsNullOrEmpty(settings.SmtpHost) && !string.IsNullOrEmpty(settings.SmtpFromEmail))
            {
                var subject = $"New contact from {msg.Name}: {msg.Subject ?? "No subject"}";
                var siteUrl = settings.SiteUrl?.TrimEnd('/');
                var body = $"""
                  <h2 style="font-size:18px;color:#020817;margin:0 0 16px;">New Contact Message</h2>
                  <table style="width:100%;font-size:14px;border-collapse:collapse;">
                    <tr><td style="padding:8px 0;color:#64748b;width:80px;vertical-align:top;">From</td><td style="padding:8px 0;color:#020817;font-weight:600;">{EmailTemplateHelper.Encode(msg.Name)}</td></tr>
                    <tr><td style="padding:8px 0;color:#64748b;vertical-align:top;">Email</td><td style="padding:8px 0;"><a href="mailto:{EmailTemplateHelper.Encode(msg.Email)}" style="color:#2563eb;">{EmailTemplateHelper.Encode(msg.Email)}</a></td></tr>
                    {(msg.Subject is not null ? $"<tr><td style=\"padding:8px 0;color:#64748b;vertical-align:top;\">Subject</td><td style=\"padding:8px 0;color:#020817;\">{EmailTemplateHelper.Encode(msg.Subject)}</td></tr>" : "")}
                    <tr><td style="padding:8px 0;color:#64748b;vertical-align:top;">Message</td><td style="padding:8px 0;color:#020817;line-height:1.6;white-space:pre-wrap;">{EmailTemplateHelper.Encode(msg.Message)}</td></tr>
                  </table>
                """;
                var html = EmailTemplateHelper.Wrap(settings.SiteName, settings.LogoUrl, siteUrl, body);
                await _emailService.SendAsync(settings.SmtpFromEmail, subject, html);
            }
        }
        catch { }

        return msg;
    }

    public async Task<List<ContactMessage>> GetAllAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Set<ContactMessage>()
            .OrderByDescending(m => m.SubmittedAt)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Set<ContactMessage>().CountAsync(m => !m.IsRead);
    }

    public async Task MarkAsReadAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var msg = await db.Set<ContactMessage>().FindAsync(id);
        if (msg is not null)
        {
            msg.IsRead = true;
            await db.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var msg = await db.Set<ContactMessage>().FindAsync(id);
        if (msg is not null)
        {
            db.Set<ContactMessage>().Remove(msg);
            await db.SaveChangesAsync();
        }
    }
}
