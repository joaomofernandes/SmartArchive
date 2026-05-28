using Microsoft.EntityFrameworkCore;
using SmartArchive.Application.Interfaces;
using SmartArchive.Infrastructure.Data;
using SmartArchive.Infrastructure.Local;
using SmartArchive.Infrastructure.Mock;

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

// Configure Mock AI Processor
builder.Services.AddSingleton<IAiProcessor, MockAiProcessor>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

// Expose Program class for integration tests (WebApplicationFactory<Program>)
public partial class Program { }
