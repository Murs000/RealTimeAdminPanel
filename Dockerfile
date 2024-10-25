# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5228

ENV ASPNETCORE_URLS=http://+:5228
USER app

# Build image with SDK for build and tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy project files and restore as distinct layers for better caching
COPY ["RealTimeAdminPanel/RealTimeAdminPanel.csproj", "RealTimeAdminPanel/"]
COPY ["Tests/IntegrationTests/IntegrationTests.csproj", "Tests/IntegrationTests/"]
COPY ["Tests/UnitTest/UnitTests.csproj", "Tests/UnitTest/"]

# Restore for each project explicitly
RUN dotnet restore "RealTimeAdminPanel/RealTimeAdminPanel.csproj"
RUN dotnet restore "Tests/IntegrationTests/IntegrationTests.csproj"
RUN dotnet restore "Tests/UnitTest/UnitTests.csproj"

# Copy remaining source code and build the projects
COPY . .
RUN dotnet build "RealTimeAdminPanel/RealTimeAdminPanel.csproj" -c $configuration -o /app/build

# Run unit tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS unit-test
WORKDIR /src/Tests/UnitTest
COPY --from=build /app/publish /app/publish 
# Copy published app
RUN dotnet test -c $configuration --results-directory /app/tests-results --logger trx

# Run integration tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS integration-test
WORKDIR /src/Tests/IntegrationTests
COPY --from=build /app/publish /app/publish  
# Copy published app
RUN dotnet test -c $configuration --results-directory /app/tests-results --logger trx

# Separate stage to publish the main app
FROM build AS publish
WORKDIR /src/RealTimeAdminPanel
RUN dotnet publish "RealTimeAdminPanel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeAdminPanel.dll"]