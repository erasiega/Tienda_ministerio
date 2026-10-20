# Tienda de electrodomésticos · Hogar Total

Servidor de una tienda web de electrodomésticos: catálogo por categorías, búsqueda, detalle de productos y cambio de precios por el personal de la tienda. Es el proyecto del curso «Utilización de Prompts de Copilot para Git/GitHub enfocados en .NET». El cliente web en Blazor es el proyecto final.

## Requisitos

- .NET 10 SDK.
- Visual Studio Code con C# Dev Kit, GitHub Copilot Chat y REST Client (VS Code los recomienda al abrir la carpeta).

## Cómo correr el servidor

```
dotnet run --project src/Tienda.Api
```

La API queda en `http://localhost:5080`. La primera vez crea `tienda.db` con 5 categorías y 32 productos de ejemplo. Para empezar con datos limpios, detengan la API y borren `src/Tienda.Api/tienda.db`.

## Cómo probar

```
dotnet test
```

Con la API corriendo, abran los archivos de la carpeta `peticiones` y envíen cada petición con «Send Request».

## Cómo está organizado

| Carpeta | Qué hay |
|---|---|
| `src/Tienda.Core` | Entidades, reglas, interfaces de los repositorios y servicios. |
| `src/Tienda.Contratos` | Lo que viaja entre el cliente y el servidor. |
| `src/Tienda.Datos` | EF Core con SQLite, repositorios y datos iniciales. |
| `src/Tienda.Api` | Endpoints, manejo de errores y configuración. |
| `tests/Tienda.Pruebas` | Pruebas unitarias con xUnit. |
| `peticiones` | Peticiones de prueba para REST Client. |
| `docs` | Enunciado, análisis, arquitectura, plan y bitácora. |
