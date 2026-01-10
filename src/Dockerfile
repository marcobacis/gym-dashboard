
# Stage 2: Build .NET
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution file and all project directories
COPY ["GymDashboard.sln", "."]
COPY ["GymClient/", "GymClient/"]
COPY ["Domain/", "Domain/"]
COPY ["Persistence/", "Persistence/"]
COPY ["WebApp/", "WebApp/"]

# Restore .NET dependencies
RUN dotnet restore "GymDashboard.sln"

# Build the application (disable Tailwind build target in Docker)
RUN dotnet build "GymDashboard.sln" -c Release --no-restore -p:DisableBuildTailwind=true

# Publish the WebApp project
RUN dotnet publish "WebApp/WebApp.csproj" -c Release -o /app/publish --no-build

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published application from build stage
COPY --from=build /app/publish .

# Expose port (adjust as needed)
EXPOSE 8080

# Run the application
ENTRYPOINT ["dotnet", "WebApp.dll"]

