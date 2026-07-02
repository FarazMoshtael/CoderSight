namespace CoderSight.Core.Entities;

public class BlogComment
{
    public Guid Id { get; set; }
    public Guid BlogPostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserDisplayName { get; set; } = string.Empty;
    public string? UserAvatarUrl { get; set; }
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsApproved { get; set; }
    public Guid? ParentCommentId { get; set; }

    public BlogPost BlogPost { get; set; } = null!;
    public BlogComment? ParentComment { get; set; }
    public List<BlogComment> Replies { get; set; } = [];
}
