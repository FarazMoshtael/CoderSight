namespace CoderSight.Core.Services;

public interface ITurnstileService
{
    bool IsEnabled { get; }
    string? SiteKey { get; }
    Task<bool> VerifyAsync(string? token, string? remoteIp);
}
