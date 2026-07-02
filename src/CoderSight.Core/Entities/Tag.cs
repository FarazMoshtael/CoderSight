namespace CoderSight.Core.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Culture { get; set; } = "en";
    public Guid? LocalizationGroupId { get; set; }

    public List<BlogPostTag> BlogPostTags { get; set; } = [];
}

public class BlogPostTag
{
    public Guid BlogPostId { get; set; }
    public Guid TagId { get; set; }

    public BlogPost BlogPost { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}
