namespace CoderSight.Core.Blocks.Data;

[CmsBlock("RelatedPosts", Icon = "article", Description = "Grid of related blog posts", Category = "Blog")]
public class RelatedPostsBlockData : IBlockData
{
    public string? Title { get; set; }
    public int MaxPosts { get; set; } = 3;
    public string? FilterByCategory { get; set; }
    public string? FilterByTag { get; set; }
    public string GetDisplayName() => Title ?? "Related posts";
}

[CmsBlock("AuthorBio", Icon = "user", Description = "Author card with avatar and bio", Category = "Blog")]
public class AuthorBioBlockData : IBlockData
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string GetDisplayName() => Name;
}

[CmsBlock("ShareButtons", Icon = "share", Description = "Social media share links", Category = "Blog")]
public class ShareButtonsBlockData : IBlockData
{
    public bool ShowTwitter { get; set; } = true;
    public bool ShowLinkedIn { get; set; } = true;
    public bool ShowFacebook { get; set; } = true;
    public bool ShowCopyLink { get; set; } = true;
    public string GetDisplayName() => "Share buttons";
}

[CmsBlock("PostNavigation", Icon = "arrow-right", Description = "Previous/next post links", Category = "Blog")]
public class PostNavigationBlockData : IBlockData
{
    public string GetDisplayName() => "Post navigation";
}

[CmsBlock("Comments", Icon = "message-circle", Description = "Comment section", Category = "Blog")]
public class CommentsBlockData : IBlockData
{
    public string Provider { get; set; } = "internal";
    public string? DisqusShortname { get; set; }
    public string GetDisplayName() => "Comments";
}

[CmsBlock("TableOfContents", Icon = "list", Description = "Auto-generated from headings", Category = "Blog")]
public class TableOfContentsBlockData : IBlockData
{
    public string Title { get; set; } = "Table of contents";
    public int MinHeadingLevel { get; set; } = 2;
    public int MaxHeadingLevel { get; set; } = 3;
    public string GetDisplayName() => Title;
}

[CmsBlock("PostList", Icon = "news", Description = "Embeddable post listing with filters", Category = "Blog")]
public class PostListBlockData : IBlockData
{
    public string? Title { get; set; }
    public int PostsPerPage { get; set; } = 9;
    public string Layout { get; set; } = "grid";
    public bool ShowCategoryFilter { get; set; } = true;
    public bool ShowSearch { get; set; } = true;
    public bool ShowLoadMore { get; set; } = true;
    public string? ViewAllUrl { get; set; }
    public string? ViewAllText { get; set; }
    public string GetDisplayName() => Title ?? "Post list";
}
