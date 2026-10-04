FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY NovaTech.TerraTech.Platform/*.csproj NovaTech.TerraTech.Platform/
RUN dotnet restore ./NovaTech.TerraTech.Platform
COPY . .
RUN dotnet publish ./NovaTech.TerraTech.Platform -c Debug --no-restore -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
USER $APP_UID
EXPOSE 8080
ENTRYPOINT ["dotnet", "NovaTech.TerraTech.Platform.dll"]
