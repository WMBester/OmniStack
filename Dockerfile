# Use the official .NET 8 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.sln .
COPY OmniStack/WMB.Api.csproj ./OmniStack/
RUN dotnet restore

# Copy the remaining files and build the application
COPY . .
WORKDIR /app/OmniStack
RUN dotnet publish -c Release -o /app/publish

# Verify the contents of the publish directory
RUN ls /app/publish

# Use the official .NET 8 runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WMB.Api.dll"]
