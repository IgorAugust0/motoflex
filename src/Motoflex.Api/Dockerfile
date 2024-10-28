# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Motoflex.Api/Motoflex.Api.csproj", "src/Motoflex.Api/"]
COPY ["src/Motoflex.Application/Motoflex.Application.csproj", "src/Motoflex.Application/"]
COPY ["src/Motoflex.Domain/Motoflex.Domain.csproj", "src/Motoflex.Domain/"]
COPY ["src/Motoflex.Infrastructure/Motoflex.Infrastructure.csproj", "src/Motoflex.Infrastructure/"]
RUN dotnet restore "./src/Motoflex.Api/Motoflex.Api.csproj"
COPY . .
WORKDIR "/src/src/Motoflex.Api"
RUN dotnet build "./Motoflex.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Motoflex.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Motoflex.Api.dll"]