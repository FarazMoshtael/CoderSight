using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface ISiteSettingsService
{
    Task<SiteSettings> GetAsync();
    Task UpdateAsync(SiteSettings settings);
}
