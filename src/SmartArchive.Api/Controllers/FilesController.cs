using Microsoft.AspNetCore.Mvc;
using SmartArchive.Application.Interfaces;
using SmartArchive.Core.Domain;

namespace SmartArchive.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FilesController : ControllerBase
{
    private readonly IStorageService _storage;
    private readonly IAiProcessor _ai;
    private readonly IFileRepository _repo;

    public FilesController(IStorageService storage, IAiProcessor ai, IFileRepository repo)
    {
        _storage = storage;
        _ai = ai;
        _repo = repo;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file uploaded");

        await using var stream = file.OpenReadStream();
        var stored = await _storage.SaveFileAsync(stream, file.FileName, file.ContentType ?? "application/octet-stream");
        var enriched = await _ai.EnrichMetadataAsync(stored);
        await _repo.AddAsync(enriched);
        return CreatedAtAction(nameof(Download), new { id = enriched.Id }, enriched);
    }

    [HttpGet]
    public async Task<IActionResult> List()
    {
        var files = await _repo.ListAsync();
        return Ok(files);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var stream = await _storage.OpenReadAsync(id);
        if (stream == null) return NotFound();
        return File(stream, "application/octet-stream");
    }
}
