# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Benkyoukai.Api/Benkyoukai.Api.csproj" --disable-parallel
RUN dotnet publish "./Benkyoukai.Api/Benkyoukai.Api.csproj" -c release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Benkyoukai.Api.dll"]
