using SmartArchive.Core.Domain;

namespace SmartArchive.Application.Interfaces;

public interface IStorageService
{
    Task<StoredFile> SaveFileAsync(Stream content, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> OpenReadAsync(Guid id, CancellationToken cancellationToken = default);
}
