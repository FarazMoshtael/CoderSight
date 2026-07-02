using CoderSight.Core.Enums;

namespace CoderSight.Core.Entities;

public class Page
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public PageStatus Status { get; set; } = PageStatus.Draft;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string Culture { get; set; } = "en";
    public Guid? LocalizationGroupId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PublishedAt { get; set; }

    public List<PageBlock> Blocks { get; set; } = [];
    public BlogPost? BlogPost { get; set; }
}
