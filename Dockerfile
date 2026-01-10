FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["WebApp/WebApp.csproj", "WebApp/"]
RUN dotnet restore "WebApp/WebApp.csproj"

COPY . .
WORKDIR "/src/WebApp"
RUN dotnet publish "WebApp.csproj" --no-restore -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApp.dll"]
