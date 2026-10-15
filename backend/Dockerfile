FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/InvoicePro.Api/InvoicePro.Api.csproj src/InvoicePro.Api/
COPY src/InvoicePro.Application/InvoicePro.Application.csproj src/InvoicePro.Application/
COPY src/InvoicePro.Domain/InvoicePro.Domain.csproj src/InvoicePro.Domain/
COPY src/InvoicePro.Infrastructure/InvoicePro.Infrastructure.csproj src/InvoicePro.Infrastructure/
RUN dotnet restore src/InvoicePro.Api/InvoicePro.Api.csproj

COPY . .
WORKDIR /src/src/InvoicePro.Api
RUN dotnet publish InvoicePro.Api.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "InvoicePro.Api.dll"]