# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5228

ENV ASPNETCORE_URLS=http://+:5228
USER app

# Build image with SDK for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy project files and restore as distinct layers for better caching
COPY ["RealTimeAdminPanel/RealTimeAdminPanel.csproj", "RealTimeAdminPanel/"]
RUN dotnet restore "RealTimeAdminPanel/RealTimeAdminPanel.csproj"

# Copy remaining source code and build the project
COPY . .
RUN dotnet build "RealTimeAdminPanel/RealTimeAdminPanel.csproj" -c $configuration -o /app/build

# Separate stage to publish the main app
FROM build AS publish
ARG configuration=Release
WORKDIR /src/RealTimeAdminPanel
RUN dotnet publish "RealTimeAdminPanel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final stage for runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RealTimeAdminPanel.dll"]