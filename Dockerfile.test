# Build image with SDK for tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy project files and restore as distinct layers for better caching
COPY ["Tests/IntegrationTests/IntegrationTests.csproj", "Tests/IntegrationTests/"]
COPY ["Tests/UnitTest/UnitTests.csproj", "Tests/UnitTest/"]
RUN dotnet restore "Tests/UnitTest/UnitTests.csproj"

# Copy remaining source code for tests
COPY . .

# Run unit tests and output results to console
WORKDIR /src/Tests/UnitTest
RUN dotnet test -c $configuration --no-restore --logger "console;verbosity=detailed"

# Run integration tests and output results to console
WORKDIR /src/Tests/IntegrationTests
RUN dotnet test -c $configuration --no-restore --logger "console;verbosity=detailed"