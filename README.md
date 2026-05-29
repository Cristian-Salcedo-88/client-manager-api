# ClientManager API

API REST desarrollada en **.NET 8** con arquitectura **DDD (Domain-Driven Design)** para gestión de clientes. Expone 3 endpoints para consultar, crear y actualizar clientes, conectada a una base de datos **SQL Server 2022** y contenerizada con **Docker**.

---

## Tecnologías

- .NET 8 / ASP.NET Core
- SQL Server 2022 (imagen Docker `mcr.microsoft.com/mssql/server:2022-latest`)
- Docker & Docker Compose
- Dapper (acceso a datos)
- MediatR (patrón CQRS)
- FluentValidation
- Swagger / OpenAPI
- ngrok (exposición pública gratuita)

---

## Estructura del proyecto

```
client-manager-api/
│   ClientManagerApi.sln
│   Dockerfile
│   docker-compose.yml
│
└───src/
    ├───ClientManager.Api            # Capa de presentación (controladores, middlewares, DTOs)
    ├───ClientManager.Domain         # Capa de dominio (entidades, interfaces, excepciones)
    └───ClientManager.Infraestructure # Capa de infraestructura (repositorios, contexto, settings)
```

---

## Requisitos previos

Tener instalado en la máquina:

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) — debe estar corriendo antes de ejecutar cualquier comando
- [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) — para gestionar la base de datos
- [ngrok](https://ngrok.com/download) — para exponer la API públicamente (gratuito, sin tarjeta)

---

## Configuración

### Variables de entorno (docker-compose.yml)

| Variable | Descripción | Valor por defecto |
|---|---|---|
| `SA_PASSWORD` | Contraseña del usuario `sa` de SQL Server | `Programador.88` |
| `ACCEPT_EULA` | Aceptación de licencia de SQL Server | `Y` |
| `MSSQL_PID` | Edición de SQL Server | `Express` |
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución de la API | `Production` |
| `SqlServerSettings__ConnectionStrings` | Cadena de conexión a SQL Server | Ver docker-compose.yml |

### Puertos

| Servicio | Puerto interno | Puerto externo (Windows) |
|---|---|---|
| API .NET 8 | 8080 | 8080 |
| SQL Server | 1433 | 1434 |

> El puerto externo de SQL Server es `1434` porque el `1433` lo ocupa el SQL Server local de Windows.

---

## Levantar el proyecto

### Paso 1 — Construir y levantar los contenedores

Abrir PowerShell en la raíz del proyecto y ejecutar:

```powershell
cd C:\Proyectos\client-manager-api
docker-compose up --build
```

> Usar `--build` solo cuando se modifique el código fuente. Para inicios normales usar solo `docker-compose up`.

Esperar hasta ver en los logs:
```
clientmanager-api | Now listening on: http://[::]:8080
```

### Paso 2 — Restaurar la base de datos (solo la primera vez)

La base de datos debe ser generada como script SQL desde SSMS:

1. En SSMS clic derecho sobre `BdClientes` → **Tasks → Generate Scripts**
2. Seleccionar **Script entire database and all database objects**
3. En **Advanced** → **Types of data to script** → `Schema and data`
4. Guardar en `C:\Backup\BdClientes.sql`

Luego copiar y ejecutar el script en el contenedor:

```powershell
# Copiar el script al contenedor
docker cp "C:\Backup\BdClientes.sql" clientmanager-sqlserver:/var/opt/mssql/BdClientes.sql

# Ejecutar el script dentro del contenedor
docker exec -it clientmanager-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Programa2#8" -No -i /var/opt/mssql/BdClientes.sql
```

> Este paso solo es necesario la primera vez. Los datos quedan persistidos en el volumen `sqldata` de Docker y sobreviven reinicios.

### Paso 3 — Verificar que la API está funcionando

Abrir en el navegador:
```
http://localhost:8080
```

Debe aparecer la interfaz de Swagger con los 3 endpoints disponibles.

---

## Endpoints

Base URL local: `http://localhost:8080`

| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/v1/Client` | Obtiene todos los clientes |
| POST | `/api/v1/Client` | Crea un nuevo cliente |
| PUT | `/api/v1/Client/{identificationNumber}` | Actualiza un cliente por número de identificación |

### Ejemplo de body para POST y PUT

```json
{
  "name": "Ivan",
  "identificationNumber": "1010114049",
  "phone": "3134911858",
  "address": "calle 57"
}
```

---

## Conectarse a la base de datos desde SSMS

Para gestionar los datos directamente desde SSMS mientras los contenedores están corriendo:

| Campo | Valor |
|---|---|
| Server | `localhost,1434` |
| Authentication | SQL Server Authentication |
| Login | `sa` |
| Password | `Programa2#8` |

---

## Exponer la API públicamente con ngrok

Para que la API sea accesible desde otra ciudad de forma gratuita se usa **ngrok**.

### Requisitos
1. Crear cuenta gratuita en [ngrok.com](https://ngrok.com) (sin tarjeta)
2. Obtener el **Authtoken** desde el dashboard de ngrok
3. Obtener un **dominio estático gratuito** desde el dashboard → Cloud Edge → Domains → New Domain

### Configurar ngrok (solo la primera vez)

Abrir la app de ngrok y ejecutar:
```
ngrok config add-authtoken TU_TOKEN_AQUI
```

### Abrir el túnel

Con los contenedores Docker ya corriendo, ejecutar en la app de ngrok:
```
ngrok http --url=TU-DOMINIO.ngrok-free.app 8080
```

La API quedará accesible desde cualquier lugar en:
```
https://TU-DOMINIO.ngrok-free.app
```

Los endpoints públicos quedan así:
- GET → `https://TU-DOMINIO.ngrok-free.app/api/v1/Client`
- POST → `https://TU-DOMINIO.ngrok-free.app/api/v1/Client`
- PUT → `https://TU-DOMINIO.ngrok-free.app/api/v1/Client/{identificationNumber}`

---

## Flujo de uso diario

### Encender

```powershell
# Terminal 1 — Levantar contenedores
cd C:\Proyectos\client-manager-api
docker-compose up
```

```
# App de ngrok — Abrir túnel
ngrok http --url=TU-DOMINIO.ngrok-free.app 8080
```

### Apagar

```powershell
# Ctrl+C en la terminal de docker-compose, luego:
docker-compose down
```

```
# Ctrl+C en la ventana de ngrok
```

---

## Comandos útiles

```powershell
# Ver contenedores corriendo
docker ps

# Ver logs de la API
docker-compose logs api

# Ver logs de SQL Server
docker-compose logs sqlserver

# Reiniciar desde cero (elimina datos)
docker-compose down -v
docker-compose up --build

# Entrar al contenedor de SQL Server
docker exec -it clientmanager-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Programa2#8" -No
```

---

## Notas importantes

- La base de datos persiste en el volumen `sqldata` de Docker. No se pierde al apagar los contenedores.
- Si se ejecuta `docker-compose down -v` (con `-v`) se eliminan los volúmenes y se pierden los datos. Se deberá restaurar la base de datos nuevamente.
- ngrok en plan gratuito permite 1 túnel activo y 40 conexiones por minuto — suficiente para uso universitario.
- La URL de ngrok solo cambia si no se usa dominio estático. Con dominio estático siempre es la misma URL.
- El puerto `1434` (SQL Server externo) y `8080` (API) deben estar libres en Windows para que Docker pueda usarlos.
