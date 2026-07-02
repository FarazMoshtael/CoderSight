using CoderSight.Core.Entities;
using CoderSight.Core.Enums;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class PageService : IPageService
{
    private readonly CmsDbContext _db;

    public PageService(CmsDbContext db) => _db = db;

    public async Task<Page?> GetBySlugAsync(string slug, string culture) =>
        await _db.Pages
            .Include(p => p.Blocks.OrderBy(b => b.SortOrder))
            .FirstOrDefaultAsync(p => p.Slug == slug && p.Culture == culture && p.Status == PageStatus.Published);

    public async Task<Page?> GetByIdAsync(Guid id) =>
        await _db.Pages
            .Include(p => p.Blocks.OrderBy(b => b.SortOrder))
            .Include(p => p.BlogPost)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Page>> GetAllAsync(string? culture = null, PageStatus? status = null)
    {
        var query = _db.Pages.AsQueryable();
        if (culture is not null) query = query.Where(p => p.Culture == culture);
        if (status is not null) query = query.Where(p => p.Status == status);
        return await query.OrderByDescending(p => p.UpdatedAt).ToListAsync();
    }

    public async Task<Page> CreateAsync(Page page)
    {
        page.Id = Guid.NewGuid();
        page.CreatedAt = DateTime.UtcNow;
        page.UpdatedAt = DateTime.UtcNow;
        _db.Pages.Add(page);
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task<Page> UpdateAsync(Page page)
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
            {
                _db.Entry(block).State = EntityState.Modified;
            }
            else
            {
                _db.Entry(block).State = EntityState.Added;
            }
        }

        _db.Entry(page).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task DeleteAsync(Guid id)
    {
        var page = await _db.Pages.FindAsync(id);
        if (page is not null)
        {
            _db.Pages.Remove(page);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Page> PublishAsync(Guid id)
    {
        var page = await _db.Pages.FindAsync(id) ?? throw new InvalidOperationException("Page not found");
        page.Status = PageStatus.Published;
        page.PublishedAt = DateTime.UtcNow;
        page.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task<Page> UnpublishAsync(Guid id)
    {
        var page = await _db.Pages.FindAsync(id) ?? throw new InvalidOperationException("Page not found");
        page.Status = PageStatus.Draft;
        page.PublishedAt = null;
        page.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return page;
    }

    public async Task<Page> CloneForTranslationAsync(Guid sourcePageId, string targetCulture)
    {
        var source = await GetByIdAsync(sourcePageId) ?? throw new InvalidOperationException("Source page not found");

        var locGroupId = source.LocalizationGroupId ?? Guid.NewGuid();
        if (source.LocalizationGroupId is null)
        {
            source.LocalizationGroupId = locGroupId;
            await _db.SaveChangesAsync();
        }

        var clone = new Page
        {
            Id = Guid.NewGuid(),
            Title = source.Title,
            Slug = source.Slug,
            Status = PageStatus.Draft,
            Culture = targetCulture,
            LocalizationGroupId = locGroupId,
            MetaTitle = source.MetaTitle,
            MetaDescription = source.MetaDescription,
            Blocks = source.Blocks.Select(b => new PageBlock
            {
                Id = Guid.NewGuid(),
                BlockType = b.BlockType,
                SortOrder = b.SortOrder,
                Data = b.Data,
                Styles = b.Styles
            }).ToList()
        };

        _db.Pages.Add(clone);
        await _db.SaveChangesAsync();
        return clone;
    }

    public async Task<List<Page>> GetTranslationsAsync(Guid localizationGroupId) =>
        await _db.Pages.Where(p => p.LocalizationGroupId == localizationGroupId).ToListAsync();
}
