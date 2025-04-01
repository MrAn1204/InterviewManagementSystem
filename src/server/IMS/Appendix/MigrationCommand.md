# Migrations

## 1. Using the CLI

### Add a migration
```bash
dotnet ef migrations add [MigrationName] --project IMS.Data --startup-project IMS.MVC --context ApplicationDbContext --output-dir Migrations
```

### Update the database
```bash
dotnet ef database update --project IMS.Data --startup-project IMS.MVC --context ApplicationDbContext
dotnet ef database update --project IMS.Data --startup-project IMS.API --context StorageDbContext
```

### Roll back a migration 
```bash
dotnet ef database update [MigrationName] --project IMS.Data --startup-project IMS.API --context ApplicationDbContext
dotnet ef database update [MigrationName] --project IMS.Data --startup-project IMS.API --context StorageDbContext
```

### Drop the database 
```bash
dotnet ef database drop --project IMS.Data --startup-project IMS.API --context ApplicationDbContext
dotnet ef database drop --project IMS.Data --startup-project IMS.API --context StorageDbContext
```

### Remove a migration
```bash
dotnet ef migrations remove --project IMS.Data --startup-project IMS.API --context ApplicationDbContext
dotnet ef migrations remove --project IMS.Data --startup-project IMS.API --context StorageDbContext
```

## 2. Using the Package Manager Console
### Add a migration
```bash
Add-Migration [MigrationName] -Project IMS.Data -StartupProject IMS.API -Context ApplicationDbContext -OutputDir IMS.Data/Migrations
```

### Update the database
```bash
Update-Database -Project IMS.Data -StartupProject IMS.API -Context ApplicationDbContext
```

### Roll back a migration
```bash
Update-Database [MigrationName] -Project IMS.Data -StartupProject IMS.API -Context ApplicationDbContext
```

### Remove a migration
```bash
Remove-Migration -Project IMS.Data -StartupProject IMS.API -Context ApplicationDbContext
```

[]: # Path: README.md
