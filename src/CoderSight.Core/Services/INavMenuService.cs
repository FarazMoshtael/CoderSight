using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface INavMenuService
{
    Task<List<NavMenuItem>> GetMenuTreeAsync();
    Task<List<NavMenuItem>> GetAllFlatAsync();
    Task<NavMenuItem> CreateAsync(NavMenuItem item);
    Task<NavMenuItem> UpdateAsync(NavMenuItem item);
    Task DeleteAsync(Guid id);
    Task ReorderAsync(List<Guid> orderedIds);
}
