# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and projects
COPY ["Codect.sln", "."]
COPY ["Codect/Codect.csproj", "Codect/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
COPY ["DAL/DAL.csproj", "DAL/"]

# Restore dependencies
RUN dotnet restore "./Codect.sln"

# Copy the rest of the code
COPY . .

# Build the solution
WORKDIR "/src/Codect"
RUN dotnet build "./Codect.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Codect.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Codect.dll"]