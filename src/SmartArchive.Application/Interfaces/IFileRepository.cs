using SmartArchive.Core.Domain;

namespace SmartArchive.Application.Interfaces;

public interface IFileRepository
{
    Task AddAsync(StoredFile file, CancellationToken cancellationToken = default);
    Task<StoredFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<StoredFile>> ListAsync(CancellationToken cancellationToken = default);
}
