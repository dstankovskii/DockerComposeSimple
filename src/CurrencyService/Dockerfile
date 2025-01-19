FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8102

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /workspace

COPY ./src ./src
COPY ./shared ./shared

WORKDIR /workspace/src/CurrencyService
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CurrencyService.dll"]
