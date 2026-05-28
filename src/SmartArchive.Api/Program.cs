using Microsoft.EntityFrameworkCore;
using SmartArchive.Application.Interfaces;
using SmartArchive.Infrastructure.Data;
using SmartArchive.Infrastructure.Local;
using SmartArchive.Infrastructure.Mock;
using SmartArchive.Application.Interfaces;
using SmartArchive.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure EF Core Sqlite
builder.Services.AddDbContext<ArchiveDbContext>(options =>
    options.UseSqlite("Data Source=archive.db"));

// Configure LocalStorageService
var storagePath = Path.Combine(builder.Environment.ContentRootPath, "storage");
builder.Services.AddSingleton<IStorageService>(sp => new LocalStorageService(storagePath));

// Register repository
builder.Services.AddScoped<IFileRepository, FileRepository>();

// Configure Mock AI Processor
builder.Services.AddSingleton<IAiProcessor, MockAiProcessor>();

var app = builder.Build();

// Apply EF migrations automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArchiveDbContext>();
    db.Database.Migrate();
}

// Redirect root to a useful endpoint and enable HTTPS redirection
app.UseHttpsRedirection();
app.MapGet("/", (HttpContext ctx) =>
{
    if (app.Environment.IsDevelopment()) return Results.Redirect("/swagger/index.html");
    return Results.Redirect("/api/files");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// Expose Program class for integration tests (WebApplicationFactory<Program>)
public partial class Program { }
