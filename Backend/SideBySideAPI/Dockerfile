FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
# Install Tesseract OCR and required language packs
RUN apk add --no-cache tesseract-ocr \
    tesseract-ocr-eng \
    tesseract-ocr-deu \
    tesseract-ocr-fra \
    tesseract-ocr-ita \
    tesseract-ocr-spa \
    tesseract-ocr-por

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["SideBySideAPI.csproj", "./"]
RUN dotnet restore "SideBySideAPI.csproj"
COPY . .
WORKDIR "/src"
# Use Debug configuration for development
RUN dotnet build "SideBySideAPI.csproj" -c Debug -o /app/build

FROM build AS publish
# Use Debug configuration for development with detailed error info
RUN dotnet publish "SideBySideAPI.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Add healthcheck to ensure the container is working properly
HEALTHCHECK --interval=30s --timeout=30s --start-period=5s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:80/health || exit 1
ENTRYPOINT ["dotnet", "SideBySideAPI.dll"] 