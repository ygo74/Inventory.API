#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["devices/Inventory.Devices.Api/Inventory.Devices.Api.csproj", "devices/Inventory.Devices.Api/"]
COPY ["common/Inventory.Common.Application/Inventory.Common.Application.csproj", "common/Inventory.Common.Application/"]
COPY ["common/Inventory.Common.Infrastructure/Inventory.Common.Infrastructure.csproj", "common/Inventory.Common.Infrastructure/"]
COPY ["common/Inventory.Common.Domain/Inventory.Common.Domain.csproj", "common/Inventory.Common.Domain/"]
COPY ["common/Inventory.Common.Plugins/Inventory.Common.Plugins.csproj", "common/Inventory.Common.Plugins/"]
COPY ["devices/Inventory.Devices.Domain/Inventory.Devices.Domain.csproj", "devices/Inventory.Devices.Domain/"]
COPY ["devices/Inventory.Devices.Infrastructure/Inventory.Devices.Infrastructure.csproj", "devices/Inventory.Devices.Infrastructure/"]
RUN dotnet restore "devices/Inventory.Devices.Api/Inventory.Devices.Api.csproj"
COPY . .
WORKDIR "/src/devices/Inventory.Devices.Api"
RUN dotnet build "Inventory.Devices.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Devices.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Devices.Api.dll"]