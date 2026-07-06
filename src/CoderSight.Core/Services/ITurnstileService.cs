namespace CoderSight.Core.Services;

public interface ITurnstileService
{
    Task<bool> VerifyAsync(string? token, string? remoteIp);
}
