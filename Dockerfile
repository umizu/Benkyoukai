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
RUN dotnet restore "Benkyoukai.Api/Benkyoukai.Api.csproj" --disable-parallel
RUN dotnet publish "Benkyoukai.Api/Benkyoukai.Api.csproj" -c release -o /app
