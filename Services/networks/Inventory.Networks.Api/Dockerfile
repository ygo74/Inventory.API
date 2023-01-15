#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["networks/Inventory.Networks.Api/Inventory.Networks.Api.csproj", "networks/Inventory.Networks.Api/"]
COPY ["common/Inventory.Common.Application/Inventory.Common.Application.csproj", "common/Inventory.Common.Application/"]
COPY ["common/Inventory.Common.Infrastructure/Inventory.Common.Infrastructure.csproj", "common/Inventory.Common.Infrastructure/"]
COPY ["common/Inventory.Common.Domain/Inventory.Common.Domain.csproj", "common/Inventory.Common.Domain/"]
COPY ["common/Inventory.Common.Plugins/Inventory.Common.Plugins.csproj", "common/Inventory.Common.Plugins/"]
COPY ["plugins/Interfaces/Inventory.Plugins.Interfaces/Inventory.Plugins.Interfaces.csproj", "plugins/Interfaces/Inventory.Plugins.Interfaces/"]
COPY ["networks/Inventory.Networks.Domain/Inventory.Networks.Domain.csproj", "networks/Inventory.Networks.Domain/"]
COPY ["networks/Inventory.Networks.Infrastructure/Inventory.Networks.Infrastructure.csproj", "networks/Inventory.Networks.Infrastructure/"]
RUN dotnet restore "networks/Inventory.Networks.Api/Inventory.Networks.Api.csproj"
COPY . .
WORKDIR "/src/networks/Inventory.Networks.Api"
RUN dotnet build "Inventory.Networks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Networks.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Networks.Api.dll"]