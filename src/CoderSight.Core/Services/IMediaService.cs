using CoderSight.Core.Entities;

namespace CoderSight.Core.Services;

public interface IMediaService
{
    Task<Media> UploadAsync(Stream stream, string fileName, string contentType);
    Task<Media?> GetByIdAsync(Guid id);
    Task<List<Media>> GetAllAsync(int page = 1, int pageSize = 20);
    Task DeleteAsync(Guid id);
}
