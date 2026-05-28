using SmartArchive.Application.Interfaces;
using SmartArchive.Core.Domain;

namespace SmartArchive.Infrastructure.Local;

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;

    public LocalStorageService(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<StoredFile> SaveFileAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var id = Guid.NewGuid();
        var subPath = Path.Combine(id.ToString()[..2], id.ToString());
        var fullDir = Path.Combine(_basePath, id.ToString()[..2]);
        Directory.CreateDirectory(fullDir);
        var fullPath = Path.Combine(_basePath, subPath);

        using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs, cancellationToken);

        var fi = new FileInfo(fullPath);

        var stored = new StoredFile
        {
            Id = id,
            FileName = fileName,
            ContentType = contentType,
            Size = fi.Length,
            BlobPath = fullPath,
            UploadedAt = DateTimeOffset.UtcNow
        };

        return stored;
    }

    public Task<Stream?> OpenReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, id.ToString()[..2], id.ToString());
        if (!File.Exists(fullPath)) return Task.FromResult<Stream?>(null);
        Stream fs = File.OpenRead(fullPath);
        return Task.FromResult<Stream?>(fs);
    }
}
