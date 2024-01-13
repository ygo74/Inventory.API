#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["provisioning/Inventory.Provisioning.WorkerService/Inventory.Provisioning.WorkerService.csproj", "provisioning/Inventory.Provisioning.WorkerService/"]
RUN dotnet restore "provisioning/Inventory.Provisioning.WorkerService/Inventory.Provisioning.WorkerService.csproj"
COPY . .
WORKDIR "/src/provisioning/Inventory.Provisioning.WorkerService"
RUN dotnet build "Inventory.Provisioning.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Provisioning.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Provisioning.WorkerService.dll"]