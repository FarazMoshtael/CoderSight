namespace CoderSight.Core.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Culture { get; set; } = "en";
    public Guid? LocalizationGroupId { get; set; }

    public List<BlogPostCategory> BlogPostCategories { get; set; } = [];
}

public class BlogPostCategory
{
    public Guid BlogPostId { get; set; }
    public Guid CategoryId { get; set; }

    public BlogPost BlogPost { get; set; } = null!;
    public Category Category { get; set; } = null!;
}
