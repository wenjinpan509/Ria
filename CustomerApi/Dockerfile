# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY CustomerApi/*.csproj ./CustomerApi/
RUN dotnet restore

# Copy everything else and build
COPY CustomerApi/. ./CustomerApi/
WORKDIR /src/CustomerApi
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CustomerApi.dll"]
