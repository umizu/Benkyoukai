# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Benkyoukai.Api.csproj" --disable-parallel
RUN dotnet publish "./Benkyoukai.Api.csproj" -c release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Benkyoukai.Api.dll"]
