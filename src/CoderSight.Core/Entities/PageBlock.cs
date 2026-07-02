namespace CoderSight.Core.Entities;

public class PageBlock
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string BlockType { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string Data { get; set; } = "{}";
    public string Styles { get; set; } = "{}";

    public Page Page { get; set; } = null!;
}
