namespace CoderSight.Core.Services;

public interface IEmailService
{
    Task SendAsync(string to, string subject, string htmlBody);
    Task SendBulkAsync(List<string> recipients, string subject, string htmlBody);
}
