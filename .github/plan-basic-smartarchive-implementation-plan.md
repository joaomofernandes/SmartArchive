# 🎯 Basic SmartArchive Implementation Plan

This plan implements a minimal, working end-to-end SmartArchive flow following the Clean Architecture rules in .github/copilot-instructions.md. I reviewed the existing code and will reuse these elements:
- Core: src/SmartArchive.Core/Domain/StoredFile.cs (existing domain record)
- Application interfaces: src/SmartArchive.Application/Interfaces/IStorageService.cs and IAiProcessor.cs
- Infrastructure: LocalStorageService (src/SmartArchive.Infrastructure/Local/LocalStorageService.cs), ArchiveDbContext (src/SmartArchive.Infrastructure/Data/ArchiveDbContext.cs), MockAiProcessor (src/SmartArchive.Infrastructure/Mock/MockAiProcessor.cs)
- Api: FilesController (src/SmartArchive.Api/Controllers/FilesController.cs) and Program.cs

High-level goal: save uploaded files to local disk, enrich metadata via the mock AI processor, persist metadata to SQLite via EF Core, and expose list/download endpoints. Migrations will be applied automatically at startup.

**Progress**: 100% [██████████]

**Last Updated**: 2026-05-28 21:44:08

## 📝 Plan Steps
- ✅ **Add repository interface in Application — src/SmartArchive.Application/Interfaces/IFileRepository.cs:**
- ✅ **Implement FileRepository in Infrastructure — src/SmartArchive.Infrastructure/Data/FileRepository.cs:**
- ✅ **Update DI registration in Api Program.cs — src/SmartArchive.Api/Program.cs:**
- ✅ **Update FilesController to persist metadata and add listing endpoint — src/SmartArchive.Api/Controllers/FilesController.cs:**
- ✅ **Ensure LocalStorageService behavior is compatible:** Local storage service uses a GUID-based path and returns StoredFile metadata; compatible with current flow.
- ✅ **Implement migrations baseline (optional developer step):** Applied automatic migrations at startup (Database.Migrate()). If you prefer explicit migration files, run `dotnet ef migrations add InitialCreate -p src/SmartArchive.Infrastructure -s src/SmartArchive.Api` locally (dotnet-ef tool required).
- ✅ **Register services and run locally:** DI registrations added (IFileRepository). To run locally: build the solution, start SmartArchive.Api; the app will apply migrations and create `archive.db` in the Api content root and a `storage` folder for blobs.
- ✅ **Developer notes, risks, and follow-ups:**
  - EF Core mapping: StoredFile is a record with init-only properties. EF Core can map records, but if you encounter runtime mapping issues, convert StoredFile to a class with public setters or add a parameterless constructor.
  - Transactionality: current flow writes file to disk, then persists metadata. If DB persist fails, disk file remains; consider compensating deletes or switching to a transactional approach.
  - dotnet-ef: The development environment must have the `dotnet-ef` tool installed to create migrations locally. The project applies migrations at startup automatically.
  - Future work: replace MockAiProcessor with a real AI service implementation in Infrastructure, add integration and unit tests, add authorization, and implement cleanup/retention policies for stored files.