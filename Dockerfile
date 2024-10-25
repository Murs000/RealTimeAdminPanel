FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5228

ENV ASPNETCORE_URLS=http://+:5228

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["RealTimeAdminPanel/RealTimeAdminPanel.csproj", "RealTimeAdminPanel/"]
RUN dotnet restore "RealTimeAdminPanel/RealTimeAdminPanel.csproj"
COPY ["Tests/IntegrationTests/IntegrationTests.csproj", "Tests/IntegrationTests/"]
RUN dotnet restore "Tests/IntegrationTests/IntegrationTests.csproj"
COPY ["Tests/UnitTest/UnitTests.csproj", "Tests/UnitTest/"]
RUN dotnet restore "Tests/UnitTest/UnitTests.csproj"
COPY . .
WORKDIR "/src/RealTimeAdminPanel"
RUN dotnet build "RealTimeAdminPanel.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RealTimeAdminPanel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealTimeAdminPanel.dll"]
