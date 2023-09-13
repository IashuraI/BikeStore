#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Presentation/BikeStore.WebApi/BikeStore.WebApi.csproj", "Presentation/BikeStore.WebApi/"]
COPY ["Application/BikeStore.Application/BikeStore.Application.csproj", "Application/BikeStore.Application/"]
COPY ["Domain/BikeStore.Domain/BikeStore.Domain.csproj", "Domain/BikeStore.Domain/"]
COPY ["Infrastructure/BikeStore.Persistence/BikeStore.Persistence.csproj", "Infrastructure/BikeStore.Persistence/"]
RUN dotnet restore "Presentation/BikeStore.WebApi/BikeStore.WebApi.csproj"
COPY . .
WORKDIR "/src/Presentation/BikeStore.WebApi"
RUN dotnet build "BikeStore.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BikeStore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BikeStore.WebApi.dll"]