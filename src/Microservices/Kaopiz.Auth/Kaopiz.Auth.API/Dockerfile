# =========================
# 1. Base runtime image
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# =========================
# 2. Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files for restore
COPY ["src/Microservices/Kaopiz.Auth/Kaopiz.Auth.API/Kaopiz.Auth.API.csproj", "src/Microservices/Kaopiz.Auth/Kaopiz.Auth.API/"]
COPY ["src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Application/Kaopiz.Auth.Application.csproj", "src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Application/"]
COPY ["src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Domain/Kaopiz.Auth.Domain.csproj", "src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Domain/"]
COPY ["src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Infrastructure/Kaopiz.Auth.Infrastructure.csproj", "src/Microservices/Kaopiz.Auth/Kaopiz.Auth.Infrastructure/"]
COPY ["src/shared/Kaopiz.Shared.Contracts/Kaopiz.Shared.Contracts.csproj", "src/shared/Kaopiz.Shared.Contracts/"]

# Restore NuGet packages
RUN dotnet restore "src/Microservices/Kaopiz.Auth/Kaopiz.Auth.API/Kaopiz.Auth.API.csproj"

# Copy toàn bộ source code
COPY . .

# Build project
WORKDIR "/src/src/Microservices/Kaopiz.Auth/Kaopiz.Auth.API"
RUN dotnet build "Kaopiz.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# =========================
# 3. Publish stage
# =========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Kaopiz.Auth.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# =========================
# 4. Final runtime image
# =========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Env vars
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "Kaopiz.Auth.API.dll"]