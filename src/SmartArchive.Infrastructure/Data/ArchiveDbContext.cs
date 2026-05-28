using Microsoft.EntityFrameworkCore;
using SmartArchive.Core.Domain;

namespace SmartArchive.Infrastructure.Data;

public class ArchiveDbContext : DbContext
{
    public ArchiveDbContext(DbContextOptions<ArchiveDbContext> options) : base(options) { }

    public DbSet<StoredFile> StoredFiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredFile>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.FileName).IsRequired();
            b.Property(x => x.ContentType).IsRequired();
            b.Property(x => x.BlobPath).IsRequired();
            b.Property(x => x.Size).IsRequired();
            b.Property(x => x.UploadedAt).IsRequired();
        });
    }
}
