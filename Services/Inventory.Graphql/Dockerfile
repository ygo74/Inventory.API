#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Inventory.Graphql/Inventory.Graphql.csproj", "Inventory.Graphql/"]
COPY ["Inventory.Infrastructure/Inventory.Infrastructure.csproj", "Inventory.Infrastructure/"]
COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
RUN dotnet restore "Inventory.Graphql/Inventory.Graphql.csproj"
COPY . .
WORKDIR "/src/Inventory.Graphql"
RUN dotnet build "Inventory.Graphql.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Graphql.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Graphql.dll"]