FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

LABEL org.opencontainers.image.source=https://github.com/bradreslam/codect

FROM base AS final
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "Codect.dll"]
