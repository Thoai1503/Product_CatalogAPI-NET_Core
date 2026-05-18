# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first to leverage layer caching for restore.
COPY ["CatalogServiceAPI_Electric-Store/CatalogServiceAPI_Electric-Store.csproj", "CatalogServiceAPI_Electric-Store/"]
COPY ["Algorithm.Core/Algorithm.Core.csproj", "Algorithm.Core/"]
RUN dotnet restore "CatalogServiceAPI_Electric-Store/CatalogServiceAPI_Electric-Store.csproj"

# Copy full source and publish.
COPY . .
WORKDIR /src/CatalogServiceAPI_Electric-Store
RUN dotnet publish "CatalogServiceAPI_Electric-Store.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Copy existing uploaded images into the image.
# When using docker-compose, the bind mount will overlay this directory.
COPY ["CatalogServiceAPI_Electric-Store/Uploads", "/app/Uploads/"]

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CatalogServiceAPI_Electric-Store.dll"]
