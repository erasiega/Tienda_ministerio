# Plan por etapas

Cada etapa deja el proyecto compilando y con sus pruebas pasando, se verifica antes de seguir y termina con una entrada en la bitácora y un commit. Primero el servidor completo; el cliente es el proyecto final.

## Servidor

| Etapa | Objetivo | Proyectos y archivos | Reglas y criterios | Cómo se verifica |
|---|---|---|---|---|
| 3.1 | Solución, proyectos vacíos y proyecto de pruebas, con sus referencias | `Tienda.slnx`, `src/`, `tests/Tienda.Pruebas` | C-02 a C-05 | `dotnet build` y `dotnet test`; `Tienda.Core` sin referencias |
| 3.2 | Entidades con sus propias reglas: categoría, producto, pedido y línea | `Tienda.Core/Entidades`, `Tienda.Core/Excepciones` | R-02, R-03, R-04, R-07 | `dotnet build` sin advertencias |
| 3.3 | Pruebas unitarias de las entidades | `tests/Tienda.Pruebas/Entidades` | CA-09, CA-10, CA-11, CA-20 | `dotnet test`; rompiendo R-02 a propósito, la prueba falla |
| 3.4 | Servicio del catálogo, interfaces de los repositorios y pruebas con repositorios falsos | `Tienda.Core/Repositorios`, `Tienda.Core/Servicios`, `tests/Tienda.Pruebas/Falsos`, `tests/Tienda.Pruebas/Servicios` | R-08, R-10 · CA-01 a CA-08, CA-18 a CA-20 | `dotnet test` |
| 3.5 | Contratos del catálogo | `Tienda.Contratos` | C-05 | `dotnet build` |
| 3.6 | Datos con EF Core y SQLite, y datos iniciales; el motor se elige en un solo lugar | `Tienda.Datos`, `appsettings.json` | R-01, C-03, C-07 | Al arrancar se crea `tienda.db` con los datos iniciales y no aparece en `git status` |
| 3.7 | API del catálogo, cambio de precio y manejo de errores | `Tienda.Api`, `peticiones/catalogo.http` | CA-01 a CA-08, CA-18 a CA-20 | Las peticiones dan el código esperado |
| 3.8 | Revisión de calidad contra los estándares y refactorización | `docs/revision-calidad.md` y los archivos de los hallazgos | Estándares | `dotnet build` sin advertencias; `dotnet test` sigue en verde |
| 3.9 | Revisión de seguridad y corrección de vulnerabilidades | `docs/revision-seguridad.md` y los archivos de los hallazgos | C-06, C-07 | Las peticiones de `peticiones/seguridad.http` y `dotnet list package --vulnerable` |
| 3.10 | Servicio de pedidos y sus pruebas con repositorios falsos | `Tienda.Core/Servicios`, `tests/Tienda.Pruebas/Servicios` | R-04 a R-09 · CA-09 a CA-17 | `dotnet test`; un pedido sin stock no cambia ningún stock |
| 3.11 | Pedidos en contratos, datos y API | `Tienda.Contratos`, `Tienda.Datos`, `Tienda.Api`, `peticiones/pedidos.http` | CA-09 a CA-17 | Las peticiones dan el código esperado |
| 3.12 | Cierre del servidor: documentación de la API para el cliente y README | `docs/api.md`, `README.md` | C-08 | Cada endpoint está en `docs/api.md`; versión v1.0 |

## Cliente (proyecto final)

| Etapa | Objetivo |
|---|---|
| 4.1 | Proyecto Blazor WebAssembly en `http://localhost:5090` que referencia `Tienda.Contratos`, y la página de categorías |
| 4.2 | Productos por categoría, con búsqueda y páginas |
| 4.3 | Detalle del producto y carrito |
| 4.4 | Hacer el pedido y mostrar su código |
| 4.5 | Consultar un pedido por su código |

Aprobado por el equipo.
