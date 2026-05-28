using SmartArchive.Application.Interfaces;
using SmartArchive.Core.Domain;

namespace SmartArchive.Infrastructure.Mock;

public class MockAiProcessor : IAiProcessor
{
    public Task<StoredFile> EnrichMetadataAsync(StoredFile file, CancellationToken cancellationToken = default)
    {
        // Simple mock: append "-enriched" to filename and pretend we added tags
        var enriched = file with { FileName = file.FileName + "-enriched" };
        return Task.FromResult(enriched);
    }
}
