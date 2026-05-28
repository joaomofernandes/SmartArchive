using Microsoft.EntityFrameworkCore;
using SmartArchive.Application.Interfaces;
using SmartArchive.Core.Domain;

namespace SmartArchive.Infrastructure.Data;

public class FileRepository : IFileRepository
{
    private readonly ArchiveDbContext _db;

    public FileRepository(ArchiveDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(StoredFile file, CancellationToken cancellationToken = default)
    {
        await _db.StoredFiles.AddAsync(file, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<StoredFile?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.StoredFiles.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<StoredFile>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.StoredFiles.OrderByDescending(x => x.UploadedAt).ToListAsync(cancellationToken);
    }
}
