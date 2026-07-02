namespace CoderSight.Core.Blocks.Data;

[CmsBlock("Testimonial", Icon = "message-circle", Description = "Customer quote with avatar", Category = "Social Proof")]
public class TestimonialBlockData : IBlockData
{
    public string Quote { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorTitle { get; set; }
    public string? AuthorImageUrl { get; set; }
    public int Rating { get; set; }
    public string GetDisplayName() => AuthorName;
}

[CmsBlock("Team", Icon = "users", Description = "Team member cards with role and photo", Category = "Social Proof")]
public class TeamBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<TeamMember> Members { get; set; } = [];
    public int Columns { get; set; } = 4;
    public string GetDisplayName() => SectionTitle ?? "Team";
}

public class TeamMember
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Bio { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? TwitterUrl { get; set; }
}

[CmsBlock("Reviews", Icon = "star", Description = "Star ratings with review text", Category = "Social Proof")]
public class ReviewsBlockData : IBlockData
{
    public string? SectionTitle { get; set; }
    public List<ReviewItem> Reviews { get; set; } = [];
    public string GetDisplayName() => SectionTitle ?? "Reviews";
}

public class ReviewItem
{
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public string Text { get; set; } = string.Empty;
    public string? Date { get; set; }
}
