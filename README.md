# Tienda de electrodomésticos · Hogar Total

Servidor de la tienda web de electrodomésticos Hogar Total: catálogo por categorías con búsqueda y páginas, detalle de productos, pedidos con varios productos que se consultan con un código, y cambio de precios para el personal de la tienda. Es el proyecto del curso «Utilización de Prompts de Copilot para Git/GitHub enfocados en .NET». El servidor está completo; el cliente web en Blazor es el proyecto final, y lo que necesita saber está en `docs/api.md`.

## Requisitos

- .NET 10 SDK.
- Visual Studio Code con C# Dev Kit, GitHub Copilot Chat y REST Client (VS Code los recomienda al abrir la carpeta).

## Configurar la clave de administración

El cambio de precios requiere una clave configurada fuera del repositorio. En desarrollo, una sola vez por computador:

```
dotnet user-secrets init --project src/Tienda.Api
dotnet user-secrets set "Administracion:Clave" "<clave-aleatoria-nueva>" --project src/Tienda.Api
```

Para las peticiones de `peticiones/catalogo.http`, creen el archivo `peticiones/.env` (en la misma carpeta de los `.http`, que es donde REST Client lo busca) con `ADMINISTRACION_CLAVE=<la-misma-clave-aleatoria>`. `.env` está excluido de Git. No reutilicen la clave anterior ni la escriban en archivos versionados. Sin clave configurada, el cambio de precio responde 401 siempre. En producción configuren `Administracion__Clave` mediante el gestor de secretos o el entorno de despliegue.

## Cómo correr el servidor

```
dotnet run --project src/Tienda.Api
```

La API queda en `http://localhost:5080`. La primera vez crea `tienda.db` con 5 categorías y 32 productos de ejemplo. Para empezar con datos limpios, detengan la API y borren `src/Tienda.Api/tienda.db`. CORS solo permite el origen del cliente configurado en `Cors:Origenes` de `src/Tienda.Api/appsettings.json` (hoy `http://localhost:5090`).

## Cómo probar

```
dotnet test
dotnet list package --vulnerable
```

Con la API corriendo, abran los archivos de la carpeta `peticiones` (`catalogo.http`, `pedidos.http` y `seguridad.http`) y envíen cada petición con «Send Request». El título de cada una dice el resultado esperado.

## Cómo está organizado

| Carpeta | Qué hay |
|---|---|
| `src/Tienda.Core` | Entidades, reglas, interfaces de los repositorios y servicios del catálogo y de pedidos. |
| `src/Tienda.Contratos` | Lo que viaja entre el cliente y el servidor. El cliente lo referencia. |
| `src/Tienda.Datos` | EF Core con SQLite, repositorios y datos iniciales. El motor se elige en `ExtensionesDatos`. |
| `src/Tienda.Api` | Endpoints, mapeadores a contratos, manejo de errores, CORS y configuración. |
| `tests/Tienda.Pruebas` | Pruebas unitarias con xUnit y repositorios falsos en memoria. |
| `peticiones` | Peticiones de prueba para REST Client. |
| `docs` | Enunciado, análisis, arquitectura, plan, revisiones de calidad y de seguridad, API para el cliente y bitácora. |
