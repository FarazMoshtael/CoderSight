namespace CoderSight.Core.Services;

public record PostNotification(Guid BlogPostId, Guid PageId, string SiteUrl);

public interface INotificationQueue
{
    void EnqueuePostNotification(PostNotification notification);
    Task<PostNotification> DequeueAsync(CancellationToken cancellationToken);
}
