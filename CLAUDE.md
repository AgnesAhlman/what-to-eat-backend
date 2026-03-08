# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

**Run the API** (requires Postgres running):
```bash
dotnet run --project src/API
```

**Start local Postgres via Docker:**
```bash
docker-compose up db -d
```

**Run everything via Docker:**
```bash
docker-compose up
```

**Build:**
```bash
dotnet build WhatToEat.sln
```

**EF Core migrations:**
```bash
# Create a new migration
dotnet ef migrations add <MigrationName> --project src/Infrastructure --startup-project src/API

# Apply migrations manually
dotnet ef database update --project src/Infrastructure --startup-project src/API
```

## Architecture

Clean architecture with four projects in `src/`:

- **Domain** — Entities only (`Recipe`, `Category`). No dependencies.
- **Application** — DTOs and mappers. References Domain.
- **Infrastructure** — EF Core `ApplicationDbContext` and migrations. References Domain.
- **API** — ASP.NET Core controllers, Swagger, DI wiring. References Application and Infrastructure.

Dependency direction: `API → Application → Domain`, `API → Infrastructure → Domain`.

## Local Configuration

The app loads config in this order: `appsettings.json` → `appsettings.{env}.json` → `appsettings.local.json` → environment variables.

For local development, create `src/API/appsettings.local.json` with the connection string (not committed):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5433;Database=devdatabase;Username=devuser;Password=devpassword"
  }
}
```

The docker-compose Postgres uses: host `localhost:5432`, db `devdatabase`, user `devuser`, password `devpassword`.

## Key Behaviors

- EF Core migrations run automatically on startup via `db.Database.MigrateAsync()`.
- Swagger UI is always enabled (not gated by environment).
- CORS allows any origin/method/header (`AllowFrontend` policy).
- `appsettings.json` currently has an `AzureSqlDb` key (legacy) — the active key used by the code is `DefaultConnection`.
