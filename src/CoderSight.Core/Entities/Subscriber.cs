namespace CoderSight.Core.Entities;

public class Subscriber
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnsubscribedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string UnsubscribeToken { get; set; } = Guid.NewGuid().ToString("N");
}
