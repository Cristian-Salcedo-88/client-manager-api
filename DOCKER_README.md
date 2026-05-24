# Docker Setup para Client Manager API

Este proyecto está configurado para ejecutarse en contenedores Docker.

## Requisitos previos

- Docker Desktop instalado
- Docker Compose (generalmente viene incluido con Docker Desktop)

## Construcción de la imagen

```bash
# Opción 1: Usar docker-compose (recomendado)
docker-compose build

# Opción 2: Usar docker directamente
docker build -t clientmanager-api:latest .
```

## Ejecución del contenedor

```bash
# Opción 1: Usar docker-compose
docker-compose up -d

# Opción 2: Usar docker directamente
docker run -d -p 80:80 -p 443:443 --name clientmanager-api clientmanager-api:latest
```

## Acceder a la API

- **HTTP**: http://localhost:80
- **Swagger UI**: http://localhost:80

## Ver logs

```bash
# Con docker-compose
docker-compose logs -f clientmanager-api

# Con docker
docker logs -f clientmanager-api
```

## Detener el contenedor

```bash
# Con docker-compose
docker-compose down

# Con docker
docker stop clientmanager-api
docker rm clientmanager-api
```

## Variables de entorno

Puedes modificar las variables de entorno en `docker-compose.yml`:

- `ASPNETCORE_ENVIRONMENT`: Ambiente de ejecución (Production, Development)
- `ASPNETCORE_URLS`: URLs donde escucha la aplicación

## Notas

- El Dockerfile utiliza un build multi-stage para optimizar el tamaño de la imagen final
- La imagen base es `mcr.microsoft.com/dotnet/aspnet:8.0`
- El contenedor escucha en los puertos 80 (HTTP) y 443 (HTTPS)
