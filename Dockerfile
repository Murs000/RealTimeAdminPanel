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

# Copy all files and build the main app
COPY . .
WORKDIR "/src/RealTimeAdminPanel"
RUN dotnet build "RealTimeAdminPanel.csproj" -c $configuration -o /app/build

# Separate stage to run tests
FROM build AS test
WORKDIR /src

# Run unit tests
RUN dotnet test "Tests/UnitTest/UnitTests.csproj" -c $configuration --no-build --verbosity normal

# Run integration tests
RUN dotnet test "Tests/IntegrationTests/IntegrationTests.csproj" -c $configuration --no-build --verbosity normal

# Publish stage
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RealTimeAdminPanel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeAdminPanel.dll"]