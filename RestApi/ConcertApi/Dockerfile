# ConcertAPI Dockerfile
# Multi-stage build for .NET 8.0 REST API with MySQL

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore
RUN dotnet publish "ConcertApi.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install netcat for health checks and entrypoint script
RUN apt-get update && apt-get install -y netcat-traditional && rm -rf /var/lib/apt/lists/*

# Copy published app from build stage
COPY --from=build /app/publish .

# Copy entrypoint script
COPY entrypoint.sh /app/entrypoint.sh
RUN chmod +x /app/entrypoint.sh

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5085
ENV ASPNETCORE_ENVIRONMENT=Development
ENV CONCERT_DB_HOST=mysql
ENV CONCERT_DB_PORT=3306
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Expose port
EXPOSE 5085 

# Health check (optional - adjust path to your actual endpoint)
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:5085/swagger/index.html || exit 1

# Start the application with entrypoint script
ENTRYPOINT ["dotnet", "ConcertApi.dll"]
