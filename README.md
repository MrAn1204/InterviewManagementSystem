# Interview Management System

An interview management system with an Angular frontend and an ASP.NET Core Web API backend. The backend uses SQL Server and Entity Framework Core for persistence, JWT for authentication, Hangfire for background jobs, and Swagger for API documentation.

## Project Structure

| Path | Description |
| --- | --- |
| `src/client` | Angular 19 frontend |
| `src/server/IMS/IMS.API` | ASP.NET Core Web API |
| `src/server/IMS/IMS.Business` | Application and business logic |
| `src/server/IMS/IMS.Core` | Shared core functionality |
| `src/server/IMS/IMS.Data` | Entity Framework Core data access and migrations |
| `src/server/IMS/IMS.Domain` | Domain entities and models |
| `src/server/IMS/IMS.Tests` | NUnit backend tests |
| `design` | UI design mockups and team design notes |
| `docs` | Meeting, planning, QA, and requirements documentation |

## Prerequisites

- Node.js and npm compatible with Angular 19
- .NET 9 SDK
- SQL Server
- Optional: the Entity Framework Core CLI for database migrations

The repository does not pin a Node.js version. Use a current LTS version that is compatible with Angular 19.

## Configuration

The API requires SQL Server, JWT, email, and Google Cloud Storage settings. The tracked `src/server/IMS/IMS.API/appsettings.json` contains logging configuration only, so provide local settings through the ignored `appsettings.Development.json` file, .NET user secrets, or environment variables.

Required settings:

```json
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=.;Database=IMS;User Id=sa;Password=replace-with-local-password;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;Integrated Security=False"
    },
    "Jwt": {
        "ValidAudience": "https://localhost:5131",
        "ValidIssuer": "https://localhost:5131",
        "Secret": "replace-with-a-long-development-secret",
        "ExpirationInMinutes": 3600,
        "RefreshDuration": 7
    },
    "GoogleCloudStorage": {
        "BucketName": "your-gcs-bucket",
        "ProjectId": "your-gcp-project"
    },
    "EmailSettings": {
        "SmtpServer": "smtp.gmail.com",
        "Port": 587,
        "SenderEmail": "your-sender@example.com",
        "SenderName": "IMS",
        "Password": "use-a-secret-or-provider-app-password"
    }
}
```

`DefaultConnection` is the only connection string consumed by the application. SQL Server must be available before starting the API.

Use development-only values for local work and do not commit secrets, service-account JSON files, or production connection strings. The development settings file is ignored by Git.

### Google Cloud Storage

Candidate CVs are stored in a Google Cloud Storage bucket under the `cv/` prefix. Create a bucket in the configured Google Cloud project and grant the runtime service account the minimum required permissions.

For hosted environments, use the platform service account. Do not add a service-account JSON key to the repository. Replace the placeholder bucket and project values for each environment. `GoogleCloudStorage:BucketName` selects the bucket used by the application, while `ProjectId` identifies the project.

### Email

Password-reset and notification email use the `EmailSettings` values shown above. For Gmail, use an SMTP app password rather than an account password. Store the password outside source control.

## Run the Frontend

Run these commands from `src/client`:

```bash
npm ci
npx ng serve
```

Open [http://localhost:4200](http://localhost:4200). The development build calls the API at `http://localhost:5113/api/`.

### Frontend Commands

From `src/client`:

```bash
# Build the production bundle
npx ng build

# Run unit tests with Karma
npx ng test
```

The production build uses the relative API URL `api/`, which is configured in `src/client/src/environments/environment.ts`. No end-to-end test target is currently configured.

## Run the Backend

Run these commands from `src/server/IMS`:

```bash
dotnet restore
dotnet build
dotnet run --project IMS.API
```

The API launch profiles provide:

- HTTP: [http://localhost:5113](http://localhost:5113)
- HTTPS: [https://localhost:7287](https://localhost:7287)

Both profiles use the `Development` environment. When running in development, Swagger is available at [http://localhost:5113/swagger](http://localhost:5113/swagger), and the Hangfire dashboard is available at [http://localhost:5113/hangfire](http://localhost:5113/hangfire).

### Backend Commands

From `src/server/IMS`:

```bash
# Restart automatically when source files change
dotnet watch --project IMS.API

# Run backend tests
dotnet test
```

If you use the HTTPS profile, trust the local ASP.NET Core development certificate with:

```bash
dotnet dev-certs https --trust
```

## Database Migrations

Install the EF Core CLI if it is not already available:

```bash
dotnet tool install --global dotnet-ef
```

Run migration commands from `src/server/IMS`. The solution contains both `ApplicationDbContext` and `StorageDbContext`; update both databases when setting up a local environment:

```bash
dotnet ef database update --project IMS.Data --startup-project IMS.API --context ApplicationDbContext
dotnet ef database update --project IMS.Data --startup-project IMS.API --context StorageDbContext
```

The API seeds roles, users, and application data during startup after the database is available. For adding, rolling back, removing, or dropping migrations, see [Appendix/MigrationCommand.md](src/server/IMS/Appendix/MigrationCommand.md).

## Testing and Building

Build and test the backend from `src/server/IMS`:

```bash
dotnet build
dotnet test
```

Build and test the frontend from `src/client`:

```bash
npx ng build
npx ng test
```

## Additional Documentation

- [Angular client documentation](src/client/README.md)
- [Backend run commands](src/server/IMS/Appendix/RunCommand.md)
- [Backend migration commands](src/server/IMS/Appendix/MigrationCommand.md)
- [Backend package commands](src/server/IMS/Appendix/PackageCommand.md)
