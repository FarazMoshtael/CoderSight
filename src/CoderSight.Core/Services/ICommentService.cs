using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface ICommentService
{
    Task<BlogComment> AddAsync(Guid blogPostId, string userId, string displayName, string? avatarUrl, string body, Guid? parentCommentId = null);
    Task<List<BlogComment>> GetApprovedAsync(Guid blogPostId);
    Task<List<BlogComment>> GetPendingAsync(int page = 1, int pageSize = 50);
    Task<List<BlogComment>> GetAllAsync(int page = 1, int pageSize = 50);
    Task ApproveAsync(Guid commentId);
    Task RejectAsync(Guid commentId);
    Task DeleteAsync(Guid commentId);
    Task<int> GetPendingCountAsync();
}
