using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace SmartArchive.Tests;

public class FilesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FilesControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task PostUpload_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms, leaveOpen: true);
        writer.Write("hello world");
        writer.Flush();
        ms.Position = 0;

        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(ms);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("text/plain");
        content.Add(streamContent, "file", "test.txt");

        var response = await client.PostAsync("/api/files/upload", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
