# InvoicePro

InvoicePro is a modular SaaS invoice management application scaffold designed for a secure business workflow, invoice management, and tenant-aware operations.

## Included in this stage

- React + TypeScript frontend shell
- ASP.NET Core backend API scaffold
- Domain, application, and infrastructure layers
- Invoice calculation service with decimal-safe totals
- Unit tests for financial calculations
- Docker Compose setup for local SQL Server and API containerization

## Quick start

1. Copy `.env.example` to `.env` and update values as needed.
2. Start SQL Server through Docker Compose: `docker compose up -d db`
3. Restore and run backend: `cd backend && dotnet restore && dotnet run --project src/InvoicePro.Api/InvoicePro.Api.csproj`
4. Install frontend dependencies and run the web app: `cd frontend && npm install && npm run dev`

## Current status

This repository is intentionally in a secure, working skeleton stage. The calculation engine and API endpoints are implemented, while some SaaS features remain to be completed in later stages.
