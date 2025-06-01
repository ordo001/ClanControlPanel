FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
#EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ClanControlPanel.Api/ClanControlPanel.Api.csproj", "ClanControlPanel.Api/"]
COPY ["ClanControlPanel.Application/ClanControlPanel.Application.csproj", "ClanControlPanel.Application/"]
COPY ["ClanControlPanel.Core/ClanControlPanel.Core.csproj", "ClanControlPanel.Core/"]
COPY ["ClanControlPanel.Infrastructure/ClanControlPanel.Infrastructure.csproj", "ClanControlPanel.Infrastructure/"]
RUN dotnet restore "ClanControlPanel.Api/ClanControlPanel.Api.csproj"
COPY . .
WORKDIR "/src/ClanControlPanel.Api"
RUN dotnet build "ClanControlPanel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ClanControlPanel.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClanControlPanel.Api.dll"]
