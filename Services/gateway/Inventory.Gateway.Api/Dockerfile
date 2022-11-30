#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["gateway/Inventory.Gateway.Api/Inventory.Gateway.Api.csproj", "gateway/Inventory.Gateway.Api/"]
RUN dotnet restore "gateway/Inventory.Gateway.Api/Inventory.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/gateway/Inventory.Gateway.Api"
RUN dotnet build "Inventory.Gateway.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Gateway.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Gateway.Api.dll"]