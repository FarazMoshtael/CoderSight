using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class NavMenuService : INavMenuService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;

    public NavMenuService(IDbContextFactory<CmsDbContext> dbFactory) => _dbFactory = dbFactory;

    public async Task<List<NavMenuItem>> GetMenuTreeAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var all = await db.NavMenuItems
            .Where(m => m.IsVisible)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();

        var lookup = all.ToLookup(m => m.ParentId);
        var roots = lookup[null].ToList();
        foreach (var root in roots)
            root.Children = lookup[root.Id].ToList();

        return roots;
    }

    public async Task<List<NavMenuItem>> GetAllFlatAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.NavMenuItems
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    public async Task<NavMenuItem> CreateAsync(NavMenuItem item)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        item.Id = Guid.NewGuid();
        var maxOrder = await db.NavMenuItems
            .Where(m => m.ParentId == item.ParentId)
            .MaxAsync(m => (int?)m.SortOrder) ?? -1;
        item.SortOrder = maxOrder + 1;
        db.NavMenuItems.Add(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task<NavMenuItem> UpdateAsync(NavMenuItem item)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.NavMenuItems.Update(item);
        await db.SaveChangesAsync();
        return item;
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var item = await db.NavMenuItems.FindAsync(id);
        if (item is not null)
        {
            var children = await db.NavMenuItems.Where(m => m.ParentId == id).ToListAsync();
            db.NavMenuItems.RemoveRange(children);
            db.NavMenuItems.Remove(item);
            await db.SaveChangesAsync();
        }
    }

    public async Task ReorderAsync(List<Guid> orderedIds)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var items = await db.NavMenuItems.Where(m => orderedIds.Contains(m.Id)).ToListAsync();
        for (int i = 0; i < orderedIds.Count; i++)
        {
            var item = items.FirstOrDefault(m => m.Id == orderedIds[i]);
            if (item is not null) item.SortOrder = i;
        }
        await db.SaveChangesAsync();
    }
}
