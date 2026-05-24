# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["src/ClientManager.Api/ClientManager.Api.csproj", "src/ClientManager.Api/"]
COPY ["src/ClientManager.Infraestructure/ClientManager.Infraestructure.csproj", "src/ClientManager.Infraestructure/"]
COPY ["src/ClientManager.Domain/ClientManager.Domain.csproj", "src/ClientManager.Domain/"]

# Restore dependencies
RUN dotnet restore "src/ClientManager.Api/ClientManager.Api.csproj"

# Copy remaining source code
COPY . .

# Build the project
RUN dotnet build "src/ClientManager.Api/ClientManager.Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "src/ClientManager.Api/ClientManager.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Set entrypoint
ENTRYPOINT ["dotnet", "ClientManager.Api.dll"]