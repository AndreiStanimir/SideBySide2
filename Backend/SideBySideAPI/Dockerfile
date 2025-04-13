FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SideBySideAPI.csproj", "./"]
RUN dotnet restore "SideBySideAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "SideBySideAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SideBySideAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SideBySideAPI.dll"] 