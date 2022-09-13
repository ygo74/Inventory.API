#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["configuration/Inventory.Configuration.Api/Inventory.Configuration.Api.csproj", "configuration/Inventory.Configuration.Api/"]
RUN dotnet restore "configuration/Inventory.Configuration.Api/Inventory.Configuration.Api.csproj"
COPY . .
WORKDIR "/src/configuration/Inventory.Configuration.Api"
RUN dotnet build "Inventory.Configuration.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Configuration.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Configuration.Api.dll"]