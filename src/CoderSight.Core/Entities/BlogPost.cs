namespace CoderSight.Core.Entities;

public class BlogPost
{
    public Guid Id { get; set; }
    public Guid PageId { get; set; }
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatarUrl { get; set; }
    public string? AuthorBio { get; set; }
    public string? AuthorLinkedIn { get; set; }
    public string? AuthorTwitter { get; set; }
    public string? AuthorWebsite { get; set; }
    public string? FeaturedImageUrl { get; set; }
    public string Excerpt { get; set; } = string.Empty;
    public int ReadTimeMinutes { get; set; }
    public bool IsFeatured { get; set; }
    public string RelatedPostIds { get; set; } = "[]";
    public string? SubmittedByUserId { get; set; }
    public string? AdminNotes { get; set; }

    public Page Page { get; set; } = null!;
    public List<BlogPostCategory> BlogPostCategories { get; set; } = [];
    public List<BlogPostTag> BlogPostTags { get; set; } = [];
}
