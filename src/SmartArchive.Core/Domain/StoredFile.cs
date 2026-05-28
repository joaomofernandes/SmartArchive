namespace SmartArchive.Core.Domain;

public record StoredFile
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long Size { get; init; }
    public string BlobPath { get; init; } = string.Empty; // path in local storage
    public DateTimeOffset UploadedAt { get; init; } = DateTimeOffset.UtcNow;
}
