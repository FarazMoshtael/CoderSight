using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoderSight.Data.Services;

public class CommentService : ICommentService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        IDbContextFactory<CmsDbContext> dbFactory,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        ILogger<CommentService> logger)
    {
        _dbFactory = dbFactory;
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<BlogComment> AddAsync(Guid blogPostId, string userId, string displayName, string? avatarUrl, string body, Guid? parentCommentId = null)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var comment = new BlogComment
        {
            Id = Guid.NewGuid(),
            BlogPostId = blogPostId,
            UserId = userId,
            UserDisplayName = displayName,
            UserAvatarUrl = avatarUrl,
            Body = body,
            ParentCommentId = parentCommentId,
            CreatedAt = DateTime.UtcNow,
            IsApproved = false
        };
        db.BlogComments.Add(comment);
        await db.SaveChangesAsync();

        return comment;
    }

    private async Task NotifyAuthorAsync(Guid commentId)
    {
        try
        {
            await using var db = await _dbFactory.CreateDbContextAsync();
            var comment = await db.BlogComments.FindAsync(commentId);
            if (comment is null) return;

            var post = await db.BlogPosts
                .Include(bp => bp.Page)
                .FirstOrDefaultAsync(bp => bp.Id == comment.BlogPostId);

            if (post is null || string.IsNullOrEmpty(post.AuthorId))
                return;

            if (post.AuthorId == comment.UserId)
                return;

            var author = await _userManager.FindByIdAsync(post.AuthorId);
            if (author?.Email is null)
                return;

            var settings = await db.SiteSettings.FirstOrDefaultAsync();
            var siteName = settings?.SiteName ?? "CoderSight";
            var siteUrl = settings?.SiteUrl?.TrimEnd('/');
            var postTitle = post.Page?.Title ?? "your article";
            var isMultilingual = settings?.EnableMultilingual ?? true;
            var postUrl = isMultilingual
                ? $"{siteUrl}/{post.Page?.Culture}/{post.Page?.Slug}"
                : $"{siteUrl}/{post.Page?.Slug}";

            var subject = $"New comment on \"{postTitle}\"";
            var body = $"""
              <h1 style="font-size:18px;color:#020817;margin:0 0 8px;">New comment on "{EmailTemplateHelper.Encode(postTitle)}"</h1>
              <p style="font-size:14px;color:#64748b;margin:0 0 20px;"><strong>{EmailTemplateHelper.Encode(comment.UserDisplayName)}</strong> commented:</p>
              <div style="background:#f8fafc;border-left:3px solid #2563eb;padding:16px;border-radius:0 8px 8px 0;margin:0 0 24px;">
                <p style="font-size:14px;color:#1a2233;line-height:1.6;margin:0;white-space:pre-wrap;">{EmailTemplateHelper.Encode(comment.Body)}</p>
              </div>
              <div style="text-align:center;margin:0 0 16px;">
                <a href="{postUrl}" style="display:inline-block;background:#2563eb;color:#ffffff;padding:10px 24px;border-radius:8px;text-decoration:none;font-size:14px;font-weight:600;">View Post</a>
              </div>
            """;
            var html = EmailTemplateHelper.Wrap(siteName, settings?.LogoUrl, siteUrl, body);

            await _emailService.SendAsync(author.Email, subject, html);
            _logger.LogInformation("Comment notification sent to {Email} for post '{Title}'", author.Email, postTitle);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to send comment notification for comment {CommentId}", commentId);
        }
    }

    public async Task<List<BlogComment>> GetApprovedAsync(Guid blogPostId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.BlogComments
            .Where(c => c.BlogPostId == blogPostId && c.IsApproved)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<BlogComment>> GetPendingAsync(int page = 1, int pageSize = 50)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.BlogComments
            .Include(c => c.BlogPost).ThenInclude(bp => bp.Page)
            .Where(c => !c.IsApproved)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<BlogComment>> GetAllAsync(int page = 1, int pageSize = 50)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.BlogComments
            .Include(c => c.BlogPost).ThenInclude(bp => bp.Page)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task ApproveAsync(Guid commentId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var comment = await db.BlogComments.FindAsync(commentId);
        if (comment is not null)
        {
            comment.IsApproved = true;
            await db.SaveChangesAsync();
            _ = Task.Run(() => NotifyAuthorAsync(commentId));
        }
    }

    public async Task RejectAsync(Guid commentId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var comment = await db.BlogComments.FindAsync(commentId);
        if (comment is not null)
        {
            comment.IsApproved = false;
            await db.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid commentId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var comment = await db.BlogComments.FindAsync(commentId);
        if (comment is not null)
        {
            db.BlogComments.Remove(comment);
            await db.SaveChangesAsync();
        }
    }

    public async Task<int> GetPendingCountAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.BlogComments.CountAsync(c => !c.IsApproved);
    }
}
