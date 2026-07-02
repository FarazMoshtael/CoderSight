using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface IContactService
{
    Task<ContactMessage> SubmitAsync(ContactMessage msg);
    Task<List<ContactMessage>> GetAllAsync();
    Task<int> GetUnreadCountAsync();
    Task MarkAsReadAsync(Guid id);
    Task DeleteAsync(Guid id);
}
