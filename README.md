# GestionPuntosGApi
API ASP.NET Core para la gestión de puntos georreferenciados (accidentes, congestión, obstrucciones, etc.) con persistencia en MongoDB y autenticación JWT.

## Estructura principal
- `GestionPuntosGApi/` — proyecto Web API (entrada: `Program.cs`)
- `Controllers/` — `PuntosGeoController.cs`, `UsuarioController.cs`
- `BLL/` — lógica de negocio (`BllPuntoGeo.cs`, `BllUsuario.cs`)
- `DAL/` — acceso a datos (`DalPuntoGeo.cs`, `DalUsuario.cs`)
- `Model/` — modelos (`PuntoGeo.cs`, `MongoDbConfig.cs`, `JwtSettings.cs`)
- `Config/` — utilidades (`TokenJwt.cs`)

## Características
- CRUD completo para `PuntoGeo` (latitud, longitud, tipo, descripción, fecha, usuario).
- Validaciones por atributos en `Model/PuntoGeo.cs`.
- Autenticación con JWT (`JwtBearer`) y verificación de issuer/audience/clave.
- Persistencia en MongoDB usando `MongoDB.Driver`.
- Swagger UI configurado con esquema de seguridad Bearer.
- Política CORS `PermitirFrontend` (permite cualquier origen/header/método).

## Requisitos
- .NET 8 SDK
- MongoDB (instancia local o remota)
- Visual Studio 2022 o `dotnet` CLI

## Configuración
Agregar en `appsettings.json` (o `appsettings.Development.json`) las secciones `ConnectionStrings` y `JwtSettings`:

{ 
    "ConnectionStrings": 
    { 
        "ConnectionString": "mongodb://localhost:27017", 
        "DatabaseName": "GestionPuntosDb" 
    }, 
    "JwtSettings": { 
        "SecretKey": "tu-clave-secreta-muy-larga",
        "Issuer": "tu-issuer", 
        "Audience": "tu-audience", 
        "ExpiresMinutes": 60 
    } 
}


- `MongoDbConfig` lee `ConnectionStrings`.
- `JwtSettings` se usa en `Program.cs` para configurar el middleware JWT.

## Ejecutar localmente
- Visual Studio: abrir la solución y presionar __F5__ (depurar) o __Ctrl+F5__ (sin depurar).
- CLI: dotnet run --project GestionPuntosGApi

Abrir Swagger: `https://localhost:<puerto>/swagger/index.html`

## Autenticación
- Las rutas de `PuntosGeoController` están protegidas con `[Authorize]`.
- Obtener un JWT (por ejemplo, mediante `UsuarioController` o `TokenJwt`) y añadirlo en Swagger con el botón "Authorize" o en el header:
  `Authorization: Bearer <token>`

## Endpoints principales (`/api/PuntosGeo`)
- GET `/api/PuntosGeo`  
  Query opcionales: `tipo`, `latitud`, `longitud`, `radioKm`  
  -> Lista de `PuntoGeo`.
- GET `/api/PuntosGeo/{id}`  
  -> `PuntoGeo` por id.
- POST `/api/PuntosGeo`  
  Body: `PuntoGeo` JSON. -> `201 Created`.
- PUT `/api/PuntosGeo/{id}`  
  Body: `PuntoGeo` JSON. Header requerido: `usuarioId`. -> `204 No Content`.
- DELETE `/api/PuntosGeo?id={id}&usuarioId={usuarioId}`  
  -> `204 No Content` si se elimina.

Modelo ejemplo:
{ 
    "latitud": -34.6037, 
    "longitud": -58.3816, 
    "tipo": "accidente", 
    "descripcion": "Choque en intersección", 
    "usuarioIngreso": "64f3a2abc..." 
}

Validaciones importantes (en `Model/PuntoGeo.cs`):
- `Latitud`: requerido, rango -90 a 90.
- `Longitud`: requerido, rango -180 a 180.
- `Tipo`: requerido, patrón `accidente|congestión|obstrucción|otro`.
- `FechaCreacion` se fija con `DateTime.UtcNow`.

## Inyección de dependencias (resumen en `Program.cs`)
- Registra `IMongoClient` y `IMongoDatabase` a partir de `MongoDbConfig`.
- Configura autenticación JWT con `JwtSettings` y `SymmetricSecurityKey`.
- Servicios registrados (scoped):
  - `IDalPuntoGeo` -> `DalPuntoGeo`
  - `IDalUsuario` -> `DalUsuario`
  - `BllPuntoGeo`, `BllUsuario`, `TokenJwt`

Swagger incluye definición de seguridad "Bearer" para probar endpoints protegidos.

## Ejemplo con curl
Crear un punto (reemplazar `<token>` y `<puerto>`):

curl -X POST "https://localhost:<puerto>/api/PuntosGeo" 
-H "Authorization: Bearer <token>" 
-H "Content-Type: application/json" 
-d '{"latitud":-34.6,"longitud":-58.38,"tipo":"accidente","descripcion":"Ejemplo","usuarioIngreso":"64f3a2abc..."}'

## Problemas comunes
- 401 Unauthorized: verificar `JwtSettings` (Issuer, Audience, SecretKey) y formato del token.
- Conexión con MongoDB: comprobar `ConnectionString` y que el servicio esté corriendo.
- Errores de validación: revisar respuesta con detalles de `ModelState`.

## Notas de desarrollo
- Patrón: `Controllers` → `BLL` → `DAL`.
- Para pruebas unitarias: mockear interfaces `IDal...`.
- `TokenJwt` y `UsuarioController` gestionan emisión/validación de tokens.
