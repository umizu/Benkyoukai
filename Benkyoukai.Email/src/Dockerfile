# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Benkyoukai.Email/Benkyoukai.Email.csproj" --disable-parallel
RUN dotnet publish "./Benkyoukai.Email/Benkyoukai.Email.csproj" -c release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Benkyoukai.Email.dll"]
