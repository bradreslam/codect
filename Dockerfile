# Base stage used at runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Install netcat for the runtime environment
USER root
RUN apt-get update && apt-get install -y netcat-openbsd && apt-get clean && rm -rf /var/lib/apt/lists/*
USER app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Codect/Codect.csproj", "Codect/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["DAL/DAL.csproj", "DAL/"]
RUN dotnet restore "./Codect/Codect.csproj"
COPY . .
WORKDIR "/src/Codect"
RUN dotnet build "./Codect.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Codect.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Codect.dll"]