using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface IBlogService
{
    Task<BlogPost?> GetBySlugAsync(string slug, string culture);
    Task<BlogPost?> GetByIdAsync(Guid id);
    Task<BlogPost?> GetByPageIdAsync(Guid pageId);
    Task<(List<BlogPost> Posts, int TotalCount)> GetPostsAsync(
        string culture, int page = 1, int pageSize = 9,
        string? categorySlug = null, string? tagSlug = null, string? search = null,
        bool publishedOnly = true);
    Task<BlogPost> CreateAsync(BlogPost blogPost);
    Task<BlogPost> UpdateAsync(BlogPost blogPost);
    Task DeleteAsync(Guid id);
    Task<List<Category>> GetCategoriesAsync(string culture);
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task DeleteCategoryAsync(Guid id);
    Task<List<Tag>> GetTagsAsync(string culture);
    Task<Tag> CreateTagAsync(Tag tag);
    Task<Tag> UpdateTagAsync(Tag tag);
    Task DeleteTagAsync(Guid id);
    Task<List<BlogPost>> GetPublishedByAuthorAsync(string authorId);
    Task<List<BlogPost>> GetPendingSubmissionsAsync();
    Task<List<BlogPost>> GetUserSubmissionsAsync(string userId);
}
