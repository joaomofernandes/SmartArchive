# GitHub Copilot Project Instructions

You are an expert .NET architect and developer. You must assist in building the **Smart Archive** application following the specific Clean Architecture rules and dependency flow defined below. 

Always respect these boundaries when generating code, refactoring, or suggesting project references.

---

## 🏗️ Architecture & Dependency Flow

The solution is divided into 5 main projects. The dependency flow is strictly **one-way**. Lower layers must never depend on higher layers.

Follow this dependency graph strictly:
- **SmartArchive.Api** ──> Depends on: `Application`, `Core`, `Infrastructure`
- **SmartArchive.Infrastructure** ──> Depends on: `Application`, `Core`
- **SmartArchive.Application** ──> Depends on: `Core`
- **SmartArchive.Core** ──> **NO DEPENDENCIES** (Pure domain logic)
- **SmartArchive.Tests** ──> Depends on: `Api`, `Infrastructure`

---

## 📁 Project Responsibilities

### 1. SmartArchive.Core
- **Purpose:** Contains core domain entities, value objects, and pure business logic.
- **Rules:** Cannot reference any other project, NuGet packages related to external frameworks (like EF Core, Azure SDKs, etc.), or specific databases.
- **Key Elements:** `StoredFile.cs` entity, domain exceptions.

### 2. SmartArchive.Application
- **Purpose:** Defines the use cases, interfaces (contracts), and application logic.
- **Rules:** Only depends on `Core`. It defines what the system does via interfaces, but not *how* it does it.
- **Key Elements:** `IStorageService`, `IAiProcessor`, Application DTOs.

### 3. SmartArchive.Infrastructure
- **Purpose:** Implements the interfaces defined in `Application` using external technologies.
- **Rules:** Depends on `Application` and `Core`. This is where third-party SDKs (SQLite, Entity Framework, Azure AI Services) are implemented.
- **Key Elements:** `ArchiveDbContext`, `LocalStorageService`, `MockAiProcessor` (to be upgraded to Azure AI Vision/Language later).

### 4. SmartArchive.Api
- **Purpose:** The entry point of the application (REST API).
- **Rules:** Coordinates dependency injection in `Program.cs`. Controllers should map HTTP requests to application services.
- **Key Elements:** `FilesController`, `Program.cs`, Middleware.

### 5. SmartArchive.Tests
- **Purpose:** Unit and Integration tests.
- **Rules:** Uses xUnit and Moq. Tests the behavior of Api routes and Infrastructure implementations.

---

## 🛠️ Code Generation Guidelines for Copilot

1. **Inversion of Control:** When asked to create a service that interacts with storage or AI, always check if an interface exists in `Application` or `Core` first.
2. **Database & External Services:** Keep EF Core DbContext configuration and Azure SDK logic isolated strictly inside the `Infrastructure` project.
3. **Drafting Code:** Prioritize writing clean, asynchronous `.NET 8/9` code using modern C# features (records, pattern matching, primary constructors where applicable).
4. **Phase 1 Focus:** Currently, we are mocking AI features. Do not add Azure SDKs yet unless explicitly requested. Use `MockAiProcessor` to simulate AI metadata generation.