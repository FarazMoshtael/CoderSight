using System.Net;
using System.Net.Mail;
using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoderSight.Data.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IDbContextFactory<CmsDbContext> dbFactory, ILogger<SmtpEmailService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody)
    {
        var settings = await GetSettingsAsync();
        if (string.IsNullOrEmpty(settings?.SmtpHost))
        {
            _logger.LogWarning("Email not sent to {To}: SMTP not configured", to);
            return;
        }

        try
        {
            using var client = CreateClient(settings);
            var message = CreateMessage(settings, to, subject, htmlBody);
            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent to {To}, subject: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}, subject: {Subject}", to, subject);
            throw;
        }
    }

    public async Task SendBulkAsync(List<string> recipients, string subject, string htmlBody)
    {
        var settings = await GetSettingsAsync();
        if (string.IsNullOrEmpty(settings?.SmtpHost) || recipients.Count == 0)
        {
            _logger.LogWarning("Bulk email not sent: SMTP not configured or no recipients");
            return;
        }

        var sent = 0;
        var failed = 0;
        using var client = CreateClient(settings);
        foreach (var to in recipients)
        {
            try
            {
                var message = CreateMessage(settings, to, subject, htmlBody);
                await client.SendMailAsync(message);
                sent++;
                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                failed++;
                _logger.LogError(ex, "Failed to send bulk email to {To}", to);
            }
        }
        _logger.LogInformation("Bulk email complete: {Sent} sent, {Failed} failed, subject: {Subject}", sent, failed, subject);
    }

    private async Task<SiteSettings?> GetSettingsAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.SiteSettings.FirstOrDefaultAsync();
    }

    private static SmtpClient CreateClient(SiteSettings s) => new(s.SmtpHost, s.SmtpPort)
    {
        Credentials = new NetworkCredential(s.SmtpUsername, s.SmtpPassword),
        EnableSsl = s.SmtpUseSsl
    };

    private static MailMessage CreateMessage(SiteSettings s, string to, string subject, string htmlBody) => new()
    {
        From = new MailAddress(s.SmtpFromEmail ?? s.SmtpUsername!, s.SmtpFromName ?? s.SiteName),
        Subject = subject,
        Body = htmlBody,
        IsBodyHtml = true,
        To = { to }
    };
}
