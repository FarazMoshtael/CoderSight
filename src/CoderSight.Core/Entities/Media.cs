namespace CoderSight.Core.Entities;

public class Media
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
    public string? AltText { get; set; }
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
