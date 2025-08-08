# What to Eat - Backend API

## Database Migrations

### Creating a New Migration

```bash
dotnet ef migrations add YourMigrationName --project src/Infrastructure --startup-project src/API
```

### Update database after migration

```bash
dotnet ef database update --project src/Infrastructure --startup-project src/API
```
