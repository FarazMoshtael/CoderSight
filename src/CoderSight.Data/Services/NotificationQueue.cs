using System.Threading.Channels;
using CoderSight.Core.Services;

namespace CoderSight.Data.Services;

public class NotificationQueue : INotificationQueue
{
    private readonly Channel<PostNotification> _channel =
        Channel.CreateUnbounded<PostNotification>(new UnboundedChannelOptions { SingleReader = true });

    public void EnqueuePostNotification(PostNotification notification) =>
        _channel.Writer.TryWrite(notification);

    public async Task<PostNotification> DequeueAsync(CancellationToken cancellationToken) =>
        await _channel.Reader.ReadAsync(cancellationToken);
}
