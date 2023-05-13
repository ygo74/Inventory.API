#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["configuration/Inventory.Configuration.Api/Inventory.Configuration.Api.csproj", "configuration/Inventory.Configuration.Api/"]
COPY ["common/Inventory.Common.Application/Inventory.Common.Application.csproj", "common/Inventory.Common.Application/"]
COPY ["common/Inventory.Common.Infrastructure/Inventory.Common.Infrastructure.csproj", "common/Inventory.Common.Infrastructure/"]
COPY ["common/Inventory.Common.Domain/Inventory.Common.Domain.csproj", "common/Inventory.Common.Domain/"]
COPY ["common/Inventory.Common.Plugins/Inventory.Common.Plugins.csproj", "common/Inventory.Common.Plugins/"]
COPY ["configuration/Inventory.Configuration.Domain/Inventory.Configuration.Domain.csproj", "configuration/Inventory.Configuration.Domain/"]
COPY ["configuration/Inventory.Configuration.Infrastructure/Inventory.Configuration.Infrastructure.csproj", "configuration/Inventory.Configuration.Infrastructure/"]
COPY ["plugins/Interfaces/Inventory.Plugins.Interfaces/Inventory.Plugins.Interfaces.csproj", "plugins/Interfaces/Inventory.Plugins.Interfaces/"]
COPY ["networks/Inventory.Networks.Domain/Inventory.Networks.Domain.csproj", "networks/Inventory.Networks.Domain/"]
RUN dotnet restore "configuration/Inventory.Configuration.Api/Inventory.Configuration.Api.csproj"
COPY . .
WORKDIR "/src/configuration/Inventory.Configuration.Api"
RUN dotnet build "Inventory.Configuration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Configuration.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Configuration.Api.dll"]