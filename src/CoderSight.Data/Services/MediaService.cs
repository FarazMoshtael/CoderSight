using CoderSight.Core.Entities;
using CoderSight.Core.Services;
using Microsoft.EntityFrameworkCore;

namespace CoderSight.Data.Services;

public class MediaService : IMediaService
{
    private readonly IDbContextFactory<CmsDbContext> _dbFactory;
    private readonly string _uploadPath;

    public MediaService(IDbContextFactory<CmsDbContext> dbFactory, string uploadPath)
    {
        _dbFactory = dbFactory;
        _uploadPath = uploadPath;
    }

    public async Task<Media> UploadAsync(Stream stream, string fileName, string contentType)
    {
        Directory.CreateDirectory(_uploadPath);
        var uniqueName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(_uploadPath, uniqueName);

        long sizeBytes;
        await using (var fileStream = File.Create(filePath))
        {
            await stream.CopyToAsync(fileStream);
            sizeBytes = fileStream.Length;
        }

        var media = new Media
        {
            Id = Guid.NewGuid(),
            FileName = fileName,
            Url = $"/uploads/{uniqueName}",
            ContentType = contentType,
            SizeBytes = sizeBytes,
            UploadedAt = DateTime.UtcNow
        };

        await using var db = await _dbFactory.CreateDbContextAsync();
        db.Media.Add(media);
        await db.SaveChangesAsync();
        return media;
    }

    public async Task<Media?> GetByIdAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Media.FindAsync(id);
    }

    public async Task<List<Media>> GetAllAsync(int page = 1, int pageSize = 20)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Media
            .OrderByDescending(m => m.UploadedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var media = await db.Media.FindAsync(id);
        if (media is not null)
        {
            var filePath = Path.Combine(_uploadPath, Path.GetFileName(media.Url));
            if (File.Exists(filePath)) File.Delete(filePath);
            db.Media.Remove(media);
            await db.SaveChangesAsync();
        }
    }
}
