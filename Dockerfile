# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5228

ENV ASPNETCORE_URLS=http://+:5228
USER app

# Build image with SDK
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy and restore project files for main app and tests
COPY ["RealTimeAdminPanel/RealTimeAdminPanel.csproj", "RealTimeAdminPanel/"]
COPY ["Tests/IntegrationTests/IntegrationTests.csproj", "Tests/IntegrationTests/"]
COPY ["Tests/UnitTest/UnitTests.csproj", "Tests/UnitTest/"]

# Restore all projects
RUN dotnet restore "RealTimeAdminPanel/RealTimeAdminPanel.csproj"
RUN dotnet restore "Tests/IntegrationTests/IntegrationTests.csproj"
RUN dotnet restore "Tests/UnitTest/UnitTests.csproj"

# Copy all files and build the main app and test projects
COPY . .
WORKDIR "/src/RealTimeAdminPanel"
RUN dotnet build "RealTimeAdminPanel.csproj" -c $configuration -o /app/build

# Build and publish test DLLs
WORKDIR "/src/Tests/UnitTest"
RUN dotnet build "UnitTests.csproj" -c $configuration -o /app/tests/unit

WORKDIR "/src/Tests/IntegrationTests"
RUN dotnet build "IntegrationTests.csproj" -c $configuration -o /app/tests/integration

# Separate stage to publish main app
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RealTimeAdminPanel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeAdminPanel.dll"]