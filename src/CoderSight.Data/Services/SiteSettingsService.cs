using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class SiteSettingsService : ISiteSettingsService
{
    private readonly IDbContextFactory<CmsDbContext> _factory;

    public SiteSettingsService(IDbContextFactory<CmsDbContext> factory) => _factory = factory;

    public async Task<SiteSettings> GetAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.SiteSettings.AsNoTracking().FirstOrDefaultAsync() ?? new SiteSettings();
    }

    public async Task UpdateAsync(SiteSettings settings)
    {
        await using var db = await _factory.CreateDbContextAsync();
        db.SiteSettings.Update(settings);
        await db.SaveChangesAsync();
    }
}
