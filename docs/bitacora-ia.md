# Bitácora de cambios con IA

Registro de cada cambio hecho con ayuda de la IA: qué cambió, por qué, cómo se verificó y qué decidió el equipo.

## 2026-10-20 08:40 · Etapa 0 · Análisis del problema
- Cambio: se creó `docs/analisis.md` con 6 historias de usuario, 21 criterios de aceptación y 6 preguntas abiertas.
- Motivo: etapa 0 del método; el punto de partida es `docs/enunciado.md`.
- Archivos: `docs/analisis.md` (nuevo).
- Verificación: cada regla R-01 a R-10 tiene al menos un criterio; cada criterio tiene datos concretos y el código HTTP esperado.
- Decisión del equipo: ajustado; decidimos las 6 preguntas abiertas y agregamos CA-07 para que el detalle no muestre el costo de compra.

## 2026-10-20 09:05 · Etapa 1 · Estándares del proyecto
- Cambio: se adaptaron los estándares del Triqui a un servidor web: estructura de la solución, API, datos, seguridad y validación, pruebas, verificación y bitácora.
- Motivo: etapa 1 del método; consideraciones C-01 a C-08 del enunciado.
- Archivos: `.github/copilot-instructions.md` (nuevo).
- Verificación: cada regla se puede comprobar mirando el código o corriendo un comando.
- Decisión del equipo: ajustado; agregamos las reglas de CORS, de datos internos y de paquetes vulnerables.

## 2026-10-20 09:30 · Etapa 2 · Arquitectura
- Cambio: se creó `docs/arquitectura.md` con dos opciones, su cumplimiento de C-02 a C-06, una comparación y la recomendación.
- Motivo: etapa 2 del método.
- Archivos: `docs/arquitectura.md` (nuevo).
- Verificación: respondimos con cada opción «¿qué hay que tocar para pasar a SQL Server?» y «¿se pueden probar las reglas sin base de datos?».
- Decisión del equipo: aceptado; elegimos la opción A.

## 2026-10-20 09:50 · Etapa 2 · Plan por etapas
- Cambio: se creó `docs/plan.md` con doce etapas para el servidor y cinco para el cliente del proyecto final.
- Motivo: etapa 2 del método; arquitectura opción A.
- Archivos: `docs/plan.md` (nuevo).
- Verificación: cada regla R-01 a R-10 aparece en alguna etapa; cada etapa del núcleo tiene sus pruebas; ninguna etapa pasa de 15 minutos.
- Decisión del equipo: ajustado; agregamos las etapas de revisión de calidad (3.8) y de seguridad (3.9) antes de los pedidos.

## 2026-10-20 10:05 · Etapa 3.1 · Solución, proyectos y proyecto de pruebas
- Cambio: se creó `Tienda.slnx` con `Tienda.Core`, `Tienda.Contratos`, `Tienda.Datos` y `Tienda.Api` en `src/`, y `Tienda.Pruebas` (xUnit) en `tests/`, con las referencias de la arquitectura. La API escucha en `http://localhost:5080`.
- Motivo: etapa 3.1 del plan; C-02 a C-05.
- Archivos: `Tienda.slnx`, `src/Directory.Build.props`, los cuatro proyectos de `src/` y `tests/Tienda.Pruebas` (nuevos).
- Verificación: `dotnet build` sin errores ni advertencias; `dotnet test` corre; `Tienda.Core` no tiene referencias.
- Decisión del equipo: aceptado.

## 2026-10-20 10:30 · Etapa 3.2 · Entidades del núcleo
- Cambio: se crearon `Categoria`, `Producto`, `Pedido` y `LineaPedido`, con sus validaciones, y las excepciones que la API traducirá a 400, 404 y 409.
- Motivo: etapa 3.2 del plan; R-02, R-03, R-04, R-07 y R-09.
- Archivos: `src/Tienda.Core/Entidades` (5 archivos nuevos), `src/Tienda.Core/Excepciones` (3 archivos nuevos).
- Verificación: `dotnet build` sin errores ni advertencias; el precio y el stock no tienen `set` público; el núcleo no usa EF Core ni ASP.NET.
- Decisión del equipo: ajustado; pedimos que el código del pedido sea un GUID y no un número consecutivo, por R-09.

## 2026-10-20 10:55 · Etapa 3.3 · Pruebas unitarias de las entidades
- Cambio: se crearon 23 pruebas con xUnit para `Producto`, `Pedido` y `LineaPedido`, cada una con el ID del criterio o de la regla, con casos de error y bordes.
- Motivo: etapa 3.3 del plan; CA-05, CA-09 a CA-12, CA-15, CA-18, CA-20 y R-09.
- Archivos: `tests/Tienda.Pruebas/Entidades` (3 archivos nuevos).
- Verificación: `dotnet test`: 23 correctas. Rompimos a propósito R-02 (`<=` por `<`): fallaron las pruebas del precio en cero; restauramos y volvieron a pasar.
- Decisión del equipo: aceptado.

## 2026-10-20 11:30 · Etapa 3.4 · Servicio del catálogo y pruebas con repositorios falsos
- Cambio: se crearon las interfaces `IRepositorioCategorias`, `IRepositorioProductos` e `IUnidadDeTrabajo`, el `ServicioCatalogo` (categorías con su cantidad, búsqueda paginada, detalle y cambio de precio), repositorios falsos en memoria y 13 pruebas del servicio.
- Motivo: etapa 3.4 del plan; R-08 y R-10 · CA-01 a CA-08, CA-18 y CA-20.
- Archivos: `src/Tienda.Core/Repositorios` (3 nuevos), `src/Tienda.Core/Servicios` (3 nuevos), `tests/Tienda.Pruebas/Falsos` (4 nuevos), `tests/Tienda.Pruebas/Servicios/ServicioCatalogoPruebas.cs` (nuevo).
- Verificación: `dotnet test`: 36 correctas.
- Decisión del equipo: aceptado.

## 2026-10-20 11:45 · Etapa 3.5 · Contratos del catálogo
- Cambio: se crearon los contratos `CategoriaRespuesta`, `ProductoResumenRespuesta`, `ProductoDetalleRespuesta`, `PaginaRespuesta<T>` y `CambiarPrecioSolicitud`.
- Motivo: etapa 3.5 del plan; C-05 y CA-07 (el detalle no muestra el costo de compra).
- Archivos: `src/Tienda.Contratos` (5 archivos nuevos).
- Verificación: `dotnet build`; ningún contrato trae costo de compra ni stock exacto.
- Decisión del equipo: aceptado.

## 2026-10-20 12:10 · Etapa 3.6 · Datos con EF Core y SQLite
- Cambio: se creó `TiendaDbContext` (también es la unidad de trabajo), la configuración de las tablas con el SKU y el slug únicos, los repositorios de categorías y productos, la elección del motor en `ExtensionesDatos` y la carga de 5 categorías y 32 productos desde `datos-iniciales.json`.
- Motivo: etapa 3.6 del plan; R-01, C-03 y C-07.
- Archivos: `src/Tienda.Datos` (13 archivos nuevos).
- Verificación: `dotnet build` sin errores; la búsqueda usa `LIKE` para no distinguir mayúsculas (CA-04).
- Decisión del equipo: aceptado.

## 2026-10-20 12:50 · Etapa 3.7 · API del catálogo, cambio de precio y manejo de errores
- Cambio: se crearon los endpoints de categorías y productos, el de cambio de precio para el personal con la clave en el encabezado `X-Clave-Admin`, el manejador de errores con `ProblemDetails`, CORS para el cliente y las peticiones de prueba. Se actualizó el README.
- Motivo: etapa 3.7 del plan; CA-01 a CA-08 y CA-18 a CA-20.
- Archivos: `src/Tienda.Api/Program.cs`, `src/Tienda.Api/appsettings.json` (modificados); `src/Tienda.Api/Endpoints` (2 nuevos), `src/Tienda.Api/Errores/ManejadorDeErrores.cs`, `peticiones/catalogo.http` (nuevos); `README.md` (modificado).
- Verificación: las 17 peticiones de `peticiones/catalogo.http` dan el código del título; `dotnet test` en verde.
- Decisión del equipo: aceptado.
