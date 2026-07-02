using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface INewsletterService
{
    Task<(bool Success, string Message)> SubscribeAsync(string email, string? name = null);
    Task<bool> UnsubscribeAsync(string token);
    Task<List<Subscriber>> GetSubscribersAsync(bool activeOnly = true);
    Task<int> GetActiveCountAsync();
    Task DeleteAsync(Guid id);
    Task SendNewPostNotificationAsync(BlogPost post, string siteUrl);
}
