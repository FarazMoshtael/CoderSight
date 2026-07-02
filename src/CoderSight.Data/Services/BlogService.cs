using CoderSight.Core.Entities;
using CoderSight.Core.Enums;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class BlogService : IBlogService
{
    private readonly CmsDbContext _db;

    public BlogService(CmsDbContext db) => _db = db;

    public async Task<BlogPost?> GetBySlugAsync(string slug, string culture) =>
        await _db.BlogPosts
            .Include(bp => bp.Page).ThenInclude(p => p.Blocks.OrderBy(b => b.SortOrder))
            .Include(bp => bp.BlogPostCategories).ThenInclude(bc => bc.Category)
            .Include(bp => bp.BlogPostTags).ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(bp => bp.Page.Slug == slug && bp.Page.Culture == culture && bp.Page.Status == PageStatus.Published);

    public async Task<BlogPost?> GetByIdAsync(Guid id) =>
        await _db.BlogPosts
            .Include(bp => bp.Page).ThenInclude(p => p.Blocks.OrderBy(b => b.SortOrder))
            .Include(bp => bp.BlogPostCategories).ThenInclude(bc => bc.Category)
            .Include(bp => bp.BlogPostTags).ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(bp => bp.Id == id);

    public async Task<BlogPost?> GetByPageIdAsync(Guid pageId) =>
        await _db.BlogPosts
            .Include(bp => bp.Page)
            .Include(bp => bp.BlogPostCategories).ThenInclude(bc => bc.Category)
            .Include(bp => bp.BlogPostTags).ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(bp => bp.PageId == pageId);

    public async Task<(List<BlogPost> Posts, int TotalCount)> GetPostsAsync(
        string culture, int page = 1, int pageSize = 9,
        string? categorySlug = null, string? tagSlug = null, string? search = null,
        bool publishedOnly = true)
    {
        var query = _db.BlogPosts
            .Include(bp => bp.Page)
            .Include(bp => bp.BlogPostCategories).ThenInclude(bc => bc.Category)
            .Include(bp => bp.BlogPostTags).ThenInclude(bt => bt.Tag)
            .Where(bp => bp.Page.Culture == culture)
            .AsQueryable();

        if (publishedOnly)
            query = query.Where(bp => bp.Page.Status == PageStatus.Published);

        if (categorySlug is not null)
            query = query.Where(bp => bp.BlogPostCategories.Any(bc => bc.Category.Slug == categorySlug));

        if (tagSlug is not null)
            query = query.Where(bp => bp.BlogPostTags.Any(bt => bt.Tag.Slug == tagSlug));

        if (search is not null)
            query = query.Where(bp => bp.Page.Title.Contains(search) || bp.Excerpt.Contains(search));

        var totalCount = await query.CountAsync();
        var posts = await query
            .OrderByDescending(bp => bp.Page.PublishedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, totalCount);
    }

    public async Task<List<BlogPost>> GetPublishedByAuthorAsync(string authorId) =>
        await _db.BlogPosts
            .Include(bp => bp.Page)
            .Include(bp => bp.BlogPostCategories).ThenInclude(bc => bc.Category)
            .Where(bp => bp.AuthorId == authorId && bp.Page.Status == PageStatus.Published)
            .OrderByDescending(bp => bp.Page.PublishedAt)
            .ToListAsync();

    public async Task<BlogPost> CreateAsync(BlogPost blogPost)
    {
        blogPost.Id = Guid.NewGuid();
        _db.BlogPosts.Add(blogPost);
        await _db.SaveChangesAsync();
        return blogPost;
    }

    public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
    {
        var page = blogPost.Page;
        if (page is not null)
        {
            page.UpdatedAt = DateTime.UtcNow;

            var existingBlockIds = await _db.PageBlocks
                .Where(b => b.PageId == page.Id)
                .Select(b => b.Id)
                .ToListAsync();

            var incomingBlockIds = page.Blocks.Select(b => b.Id).ToHashSet();

            var blocksToRemove = existingBlockIds.Where(id => !incomingBlockIds.Contains(id)).ToList();
            if (blocksToRemove.Any())
            {
                var removeEntities = await _db.PageBlocks
                    .Where(b => blocksToRemove.Contains(b.Id))
                    .ToListAsync();
                _db.PageBlocks.RemoveRange(removeEntities);
            }

            foreach (var block in page.Blocks)
            {
                block.PageId = page.Id;
                if (existingBlockIds.Contains(block.Id))
                    _db.Entry(block).State = EntityState.Modified;
                else
                    _db.Entry(block).State = EntityState.Added;
            }

            _db.Entry(page).State = EntityState.Modified;
        }

        _db.Entry(blogPost).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return blogPost;
    }

    public async Task DeleteAsync(Guid id)
    {
        var post = await _db.BlogPosts.Include(bp => bp.Page).FirstOrDefaultAsync(bp => bp.Id == id);
        if (post is not null)
        {
            _db.Pages.Remove(post.Page);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Category>> GetCategoriesAsync(string culture) =>
        await _db.Categories.Where(c => c.Culture == culture).OrderBy(c => c.Name).ToListAsync();

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        category.Id = Guid.NewGuid();
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        _db.Categories.Update(category);
        await _db.SaveChangesAsync();
        return category;
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category is not null)
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<Tag>> GetTagsAsync(string culture) =>
        await _db.Tags.Where(t => t.Culture == culture).OrderBy(t => t.Name).ToListAsync();

    public async Task<Tag> CreateTagAsync(Tag tag)
    {
        tag.Id = Guid.NewGuid();
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();
        return tag;
    }

    public async Task<Tag> UpdateTagAsync(Tag tag)
    {
        _db.Tags.Update(tag);
        await _db.SaveChangesAsync();
        return tag;
    }

    public async Task DeleteTagAsync(Guid id)
    {
        var tag = await _db.Tags.FindAsync(id);
        if (tag is not null)
        {
            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<BlogPost>> GetPendingSubmissionsAsync() =>
        await _db.BlogPosts
            .Include(bp => bp.Page)
            .Where(bp => bp.Page.Status == PageStatus.PendingReview)
            .OrderByDescending(bp => bp.Page.CreatedAt)
            .ToListAsync();

    public async Task<List<BlogPost>> GetUserSubmissionsAsync(string userId) =>
        await _db.BlogPosts
            .Include(bp => bp.Page)
            .Where(bp => bp.SubmittedByUserId == userId)
            .OrderByDescending(bp => bp.Page.CreatedAt)
            .ToListAsync();
}
