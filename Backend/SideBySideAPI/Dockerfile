FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SideBySideAPI.csproj", "."]
RUN dotnet restore "./SideBySideAPI.csproj"
COPY . .
RUN dotnet build "SideBySideAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "SideBySideAPI.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create logs directory
RUN mkdir -p /app/logs && chmod 777 /app/logs

# Install wget for healthcheck
RUN apt-get update && apt-get install -y wget && apt-get clean

# Debug: List all files in the published directory
RUN ls -la /app

# Add healthcheck
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "SideBySideAPI.dll"] 