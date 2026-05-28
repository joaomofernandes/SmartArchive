using SmartArchive.Core.Domain;

namespace SmartArchive.Application.Interfaces;

public interface IAiProcessor
{
    Task<StoredFile> EnrichMetadataAsync(StoredFile file, CancellationToken cancellationToken = default);
}
