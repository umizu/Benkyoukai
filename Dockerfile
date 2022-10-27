# FROM mcr.microsoft.com/dotnet/sdk:6.0
# WORKDIR /home/app
# COPY . .
# RUN dotnet restore
# RUN dotnet publish ./Benkyoukai.Api/Benkyoukai.Api.csproj -o /publish/
# WORKDIR /publish
# ENTRYPOINT ["dotnet", "Benkyoukai.Api.dll"]

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY . .
RUN dotnet restore "./Benkyoukai.Api/Benkyoukai.Api.csproj" --disable-parallel
RUN dotnet publish "./Benkyoukai.Api/Benkyoukai.Api.csproj" -c release -o /app --no-restore

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app .

RUN apt-get update
# RUN apt-get install -y iputils-ping
RUN apt-get install -y iproute2
RUN ip route add 10.0.1.0/24 via 10.0.0.10


ENTRYPOINT ["dotnet", "Benkyoukai.Api.dll"]
