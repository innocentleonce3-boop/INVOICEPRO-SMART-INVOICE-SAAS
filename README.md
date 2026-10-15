# InvoicePro

InvoicePro is an early-stage smart invoicing and management SaaS project. The repository currently contains a React/TypeScript frontend, a layered ASP.NET Core API, an invoice calculation service, and a SQL Server persistence scaffold. It is not yet a complete production SaaS product.

## Features in this stage

- React and TypeScript frontend application built with Vite.
- ASP.NET Core API with health, dashboard-summary, invoice-list, registration, and invoice-draft endpoints.
- Layered domain, application, and infrastructure projects.
- Decimal-based invoice line, tax, and discount calculations.
- Unit tests for invoice calculations.
- SQL Server 2022 development service and container builds for the API and frontend.
- Development-only Swagger UI.

Authentication, durable invoice workflows, tenant isolation, and production deployment hardening are not implemented yet. The current API includes sample dashboard and invoice-list responses; do not treat these as persisted customer data.

## Technology

- Frontend: React, TypeScript, Vite, ESLint.
- Backend: ASP.NET Core on .NET 8, C#.
- Data: Microsoft SQL Server and Entity Framework Core.
- Tests: xUnit.
- Containers: Docker and Docker Compose.

## Architecture

The frontend is a Vite application. The API project hosts HTTP endpoints and registers the invoice calculator and EF Core DbContext. Application contains invoice calculation logic, Domain contains invoice models, and Infrastructure contains the SQL Server DbContext. Unit tests exercise the calculation service.

## Project layout

```text
.
|-- backend/
|   |-- src/
|   |   |-- InvoicePro.Api/             # HTTP API and composition root
|   |   |-- InvoicePro.Application/     # Invoice calculation service
|   |   |-- InvoicePro.Domain/          # Invoice models
|   |   `-- InvoicePro.Infrastructure/  # EF Core SQL Server DbContext
|   `-- tests/InvoicePro.UnitTests/     # xUnit calculation tests
|-- frontend/
|   |-- public/                         # Static public assets
|   `-- src/                            # React app, styles, and assets
|-- .env.example                        # Safe local environment template
`-- docker-compose.yml                 # Local SQL Server and app services
```

There is no solution file or EF Core migration in the repository yet; use the project files directly. No separate SQL schema scripts are currently provided.

## Prerequisites

- .NET SDK 8.0.426 or a compatible .NET 8 feature-band SDK (see `backend/global.json`).
- Node.js 22.12 or later and npm.
- Docker Desktop with Docker Compose v2 for the container workflow, or a local SQL Server instance.

## Local setup

1. Clone the repository and copy `.env.example` to `.env`.
2. Replace `MSSQL_SA_PASSWORD` in `.env` with a unique, strong local password. `.env` is ignored by Git.
3. Start SQL Server with `docker compose up -d db`.
4. To run the API outside Docker, set the local password and connection string in the current shell. In PowerShell:

	```powershell
	$env:MSSQL_SA_PASSWORD = Read-Host 'Local SQL Server password'
	$env:ConnectionStrings__Default = "Server=localhost,1433;Database=InvoicePro;User Id=sa;Password=$env:MSSQL_SA_PASSWORD;TrustServerCertificate=True"
	```

	In a POSIX shell, export `MSSQL_SA_PASSWORD` and `ConnectionStrings__Default` with the equivalent local values. Do not commit real connection strings.

### Frontend

```sh
cd frontend
npm ci
npm run dev
```

Vite prints the local URL, normally `http://localhost:5173`.

### Backend

From the repository root, with `ConnectionStrings__Default` configured in the current shell:

```sh
cd backend
dotnet restore src/InvoicePro.Api/InvoicePro.Api.csproj
dotnet run --project src/InvoicePro.Api/InvoicePro.Api.csproj
```

## SQL Server and EF Core migrations

The DbContext is configured for SQL Server, but this repository does not yet contain migrations or schema scripts. No database schema is automatically applied at startup. After entity configuration is ready, install the EF tool if needed and create a migration from the `backend` directory:

```sh
cd backend
dotnet tool install --global dotnet-ef --version 8.0.8
dotnet ef migrations add InitialCreate --project src/InvoicePro.Infrastructure/InvoicePro.Infrastructure.csproj --startup-project src/InvoicePro.Api/InvoicePro.Api.csproj --output-dir Data/Migrations
dotnet ef database update --project src/InvoicePro.Infrastructure/InvoicePro.Infrastructure.csproj --startup-project src/InvoicePro.Api/InvoicePro.Api.csproj
```

## Tests and quality checks

```sh
cd frontend
npm run lint
npm run build
```

```sh
cd backend
dotnet restore tests/InvoicePro.UnitTests/InvoicePro.UnitTests.csproj
dotnet test tests/InvoicePro.UnitTests/InvoicePro.UnitTests.csproj
dotnet build src/InvoicePro.Api/InvoicePro.Api.csproj --no-restore
```

## Docker

Copy `.env.example` to `.env`, set a unique `MSSQL_SA_PASSWORD`, then build and start the stack:

```sh
docker compose up --build
```

The frontend is published on `http://localhost:5173`, the API on `http://localhost:8080`, and SQL Server on port `1433`. Stop the services with `docker compose down`. The named SQL Server volume persists local database data; `docker compose down -v` deletes that data.

## Deployment guidance

The Dockerfiles provide buildable application images, not a production deployment configuration. Before deploying, provision managed SQL Server, configure secrets through the hosting platform, terminate TLS at a trusted ingress, restrict network access, configure health checks and backups, and implement authentication, authorization, tenant isolation, and migration review. Do not use development credentials or sample API responses in production.

## Security

- Keep `.env`, local settings, and private keys out of Git. Only placeholder values belong in `.env.example`.
- Compose requires `MSSQL_SA_PASSWORD` from the local environment; no default database password is embedded in the application.
- Use a unique secret locally and a managed secret store in deployed environments. Rotate any real credential that has ever been committed or shared.

## License

See [LICENSE](LICENSE).

## Development status

This is a functional project scaffold, not a finished SaaS product. The invoice calculator and its unit tests are implemented; persistence workflows, authentication, customer management, and production operations remain future work.
