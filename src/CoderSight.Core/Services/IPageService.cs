using CoderSight.Core.Entities;
using CoderSight.Core.Enums;

namespace CoderSight.Core.Services;

public interface IPageService
{
    Task<Page?> GetBySlugAsync(string slug, string culture);
    Task<Page?> GetByIdAsync(Guid id);
    Task<List<Page>> GetAllAsync(string? culture = null, PageStatus? status = null);
    Task<Page> CreateAsync(Page page);
    Task<Page> UpdateAsync(Page page);
    Task DeleteAsync(Guid id);
    Task<Page> PublishAsync(Guid id);
    Task<Page> UnpublishAsync(Guid id);
    Task<Page> CloneForTranslationAsync(Guid sourcePageId, string targetCulture);
    Task<List<Page>> GetTranslationsAsync(Guid localizationGroupId);
}
