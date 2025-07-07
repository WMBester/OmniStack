# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the main API project files
COPY OmniStack/WMB.Api.csproj ./OmniStack/
COPY OmniStack/ ./OmniStack/

# Copy the Integration Tests project files only
COPY WMB.Api.IntegrationTests/WMB.Api.IntegrationTests.csproj ./WMB.Api.IntegrationTests/
COPY WMB.Api.IntegrationTests/ ./WMB.Api.IntegrationTests/

# Restore dependencies for the Integration Tests project only
RUN dotnet restore WMB.Api.IntegrationTests/WMB.Api.IntegrationTests.csproj

# Build and publish the API project
WORKDIR /app/OmniStack
RUN dotnet publish -c Release -o /app/publish

# Verify the contents of the publish directory
RUN ls /app/publish

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WMB.Api.dll"]
