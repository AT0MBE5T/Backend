FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RealEstateAgency.API/RealEstateAgency.API.csproj", "RealEstateAgency.API/"]
COPY ["RealEstateAgency.Application/RealEstateAgency.Application.csproj", "RealEstateAgency.Application/"]
COPY ["RealEstateAgency.Core/RealEstateAgency.Core.csproj", "RealEstateAgency.Core/"]
COPY ["RealEstateAgency.Infrastructure/RealEstateAgency.Infrastructure.csproj", "RealEstateAgency.Infrastructure/"]
RUN dotnet restore "RealEstateAgency.API/RealEstateAgency.API.csproj"
COPY . .
WORKDIR "/src/RealEstateAgency.API"
RUN dotnet build "./RealEstateAgency.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RealEstateAgency.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RealEstateAgency.API.dll"]
