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

## 2026-10-21 17:05 · Etapa 3.8 · Revisión de calidad
- Cambio: se documentaron los hallazgos de calidad de `src/`; no se modificó código.
- Motivo: primera parte de la etapa 3.8 de `docs/plan.md`; revisión contra `.github/copilot-instructions.md`.
- Archivos: `docs/revision-calidad.md` (nuevo); `docs/bitacora-ia.md` (actualizado).
- Verificación: revisión de los archivos fuente de `src/` y `dotnet build Tienda.slnx` (0 errores; 4 advertencias NU1903).
- Decisión del equipo: aceptado

## 2026-10-21 17:18 · Etapa 3.8 · Corrección de hallazgos de calidad
- Cambio: se centralizó el mapeo de productos y páginas en la API, se nombraron límites y valores predeterminados de paginación en el núcleo, se eliminó código muerto y se reemplazó SQL escrito a mano por LINQ. También se renombraron variables y se nombró la precisión y escala decimal.
- Motivo: segunda parte de la etapa 3.8; corrección de los hallazgos de `docs/revision-calidad.md`, sin modificar paquetes, configuración de seguridad ni pruebas.
- Archivos: `src/Tienda.Api/Endpoints/EndpointsAdministracion.cs`, `src/Tienda.Api/Endpoints/EndpointsCatalogo.cs`, `src/Tienda.Api/Mapeadores/MapeadorProducto.cs` (nuevo), `src/Tienda.Core/Servicios/ConstantesPaginacion.cs` (nuevo), `src/Tienda.Core/Servicios/ServicioCatalogo.cs`, `src/Tienda.Datos/Configuraciones/ConfiguracionProducto.cs`, `src/Tienda.Datos/Repositorios/RepositorioProductos.cs`, `docs/bitacora-ia.md`.
- Verificación: `dotnet build` (0 errores; 4 advertencias NU1903 conocidas de `Newtonsoft.Json` 12.0.3); `dotnet test` (36 pruebas correctas); `git diff --check`.
- Decisión del equipo: aceptado

## 2026-10-21 17:39 · Etapa 3.9 · Revisión de seguridad
- Cambio: se documentaron los resultados para V-01 a V-06; no se modificó código.
- Motivo: primera parte de la etapa 3.9 de `docs/plan.md`; revisión de `src/`, `peticiones/` y configuración contra `.github/copilot-instructions.md`.
- Archivos: `docs/revision-seguridad.md` (nuevo); `docs/bitacora-ia.md` (actualizado).
- Verificación: revisión de las superficies indicadas y `dotnet list Tienda.slnx package --vulnerable` (se reporta `Newtonsoft.Json` 12.0.3, severidad alta).
- Decisión del equipo:

## 2026-10-21 17:39 · Etapa 3.9 · Corrección de V-02
- Cambio: se retiró la clave administrativa versionada, la API ahora rechaza la autenticación si no hay secreto configurado y las peticiones usan un valor local no versionado. Se documentó la configuración de User Secrets y variables de entorno.
- Motivo: seguimiento solicitado por el equipo para corregir el hallazgo de mayor severidad V-02; no se corrigieron los demás hallazgos.
- Archivos: `src/Tienda.Api/appsettings.json`, `src/Tienda.Api/Tienda.Api.csproj`, `src/Tienda.Api/Endpoints/EndpointsAdministracion.cs`, `peticiones/catalogo.http`, `peticiones/seguridad.http`, `.gitignore`, `README.md`, `docs/revision-seguridad.md`, `docs/bitacora-ia.md`.
- Verificación: `dotnet test` (36 pruebas correctas); `dotnet build Tienda.slnx` (0 errores; 4 advertencias NU1903 para `Newtonsoft.Json` 12.0.3); solicitudes locales sin clave y con clave inválida devuelven 401; se comprobó que la clave anterior ya no aparece en los archivos del espacio de trabajo.
- Decisión del equipo: aceptado; la clave nueva debe configurarse fuera del repositorio.

## 2026-10-23 10:04 · Etapa 3.9 · Corrección de V-03 y V-05
- Cambio: un error inesperado responde 500 con «Ocurrió un error inesperado.» y el detalle va al registro con `ILogger`; se quitó `Newtonsoft.Json` y los datos iniciales se leen con `System.Text.Json`. Se completó la decisión del equipo en `docs/revision-calidad.md` y `docs/revision-seguridad.md`.
- Motivo: V-03 y V-05 de `docs/revision-seguridad.md`; C-06.
- Archivos: `src/Tienda.Api/Errores/ManejadorDeErrores.cs`, `src/Tienda.Datos/DatosIniciales/CargadorDatosIniciales.cs`, `src/Tienda.Datos/Tienda.Datos.csproj`, `docs/revision-calidad.md` y `docs/revision-seguridad.md` (modificados).
- Verificación: `dotnet build` sin advertencias (ya no aparece NU1903); `dotnet test`: 36 correctas; `dotnet list package --vulnerable` no encuentra paquetes vulnerables; al borrar `tienda.db` y arrancar, se cargan las 5 categorías y los 32 productos.
- Decisión del equipo: aceptado.

## 2026-10-23 10:05 · Etapa 3.9 · Corrección de V-02 y V-04
- Cambio: CORS solo permite los orígenes de `Cors:Origenes` en `appsettings.json`, por ahora `http://localhost:5090`, el del cliente del proyecto final. La clave de administración se compara en tiempo constante y queda vacía en `appsettings.json`, así que sin secreto configurado responde 401. El README explica que el `.env` va en la carpeta `peticiones`, que es donde REST Client lo busca.
- Motivo: V-04 de `docs/revision-seguridad.md` y ajuste de V-02; C-07 y R-10.
- Archivos: `src/Tienda.Api/Program.cs`, `src/Tienda.Api/appsettings.json`, `src/Tienda.Api/Endpoints/EndpointsAdministracion.cs`, `peticiones/catalogo.http` y `README.md` (modificados).
- Verificación: `dotnet build` sin advertencias; `dotnet test`: 36 correctas; con otro origen la API no responde `Access-Control-Allow-Origin`, con `http://localhost:5090` sí; sin clave y con la clave vieja, 401; con la clave nueva en `peticiones/.env`, CA-18 da 200.
- Decisión del equipo: aceptado.

## 2026-10-23 10:05 · Etapa 3.10 · Servicio de pedidos y sus pruebas
- Cambio: se crearon `IRepositorioPedidos`, `DatosPedido`, `DatosLineaPedido` y `ServicioPedidos` (registrar todo o nada con el precio del servidor, y consultar por código), con un repositorio de pedidos falso, un reloj fijo y 13 pruebas.
- Motivo: etapa 3.10 del plan; R-04 a R-09 · CA-09 a CA-17.
- Archivos: `src/Tienda.Core/Repositorios/IRepositorioPedidos.cs`, `src/Tienda.Core/Servicios` (3 nuevos), `tests/Tienda.Pruebas/Falsos` (2 nuevos), `tests/Tienda.Pruebas/Servicios/ServicioPedidosPruebas.cs` (nuevo).
- Verificación: `dotnet build` sin advertencias; `dotnet test`: 49 correctas. En la prueba de CA-12 la línea sin stock va de última; si el servicio bajara el stock mientras valida, esa prueba falla.
- Decisión del equipo: aceptado.

## 2026-10-23 10:07 · Etapa 3.11 · Pedidos en contratos, datos y API
- Cambio: se crearon los contratos `CrearPedidoSolicitud`, `LineaPedidoSolicitud`, `PedidoRespuesta` y `LineaPedidoRespuesta`; las tablas de pedidos y líneas con índice único en el código y el `RepositorioPedidos`; el `MapeadorPedido`; y los endpoints `POST /api/pedidos` (201 con la ruta del pedido) y `GET /api/pedidos/{codigo}`, con sus peticiones de prueba.
- Motivo: etapa 3.11 del plan; CA-09 a CA-17.
- Archivos: `src/Tienda.Contratos` (4 nuevos); `src/Tienda.Datos/Configuraciones` (2 nuevos), `src/Tienda.Datos/Repositorios/RepositorioPedidos.cs` (nuevo), `TiendaDbContext.cs` y `ExtensionesDatos.cs` (modificados); `src/Tienda.Api/Mapeadores/MapeadorPedido.cs` y `src/Tienda.Api/Endpoints/EndpointsPedidos.cs` (nuevos), `Program.cs` (modificado); `peticiones/pedidos.http` (nuevo).
- Verificación: se borró `tienda.db` para que se cree la tabla de pedidos; las peticiones de `peticiones/pedidos.http` dan el código del título (201 con total 3677000, 200 al consultar por código, 400, 409 y 404 en los errores); `dotnet build` sin advertencias y `dotnet test`: 49 correctas.
- Decisión del equipo: aceptado.

## 2026-10-23 10:07 · Etapa 3.12 · Cierre del servidor
- Cambio: se creó `docs/api.md` con cada endpoint, sus parámetros, ejemplos y códigos para quien construya el cliente, y se actualizó el README con la clave de administración, CORS, cómo correr, cómo probar y la organización. En el plan, la etapa 4.1 dice el puerto del cliente.
- Motivo: etapa 3.12 del plan; C-08.
- Archivos: `docs/api.md` (nuevo), `README.md` y `docs/plan.md` (modificados).
- Verificación: cada endpoint de `src/Tienda.Api/Endpoints` está en `docs/api.md`; los ejemplos coinciden con las respuestas de las peticiones; `dotnet build` sin advertencias, `dotnet test`: 49 correctas y `dotnet list package --vulnerable` sin hallazgos.
- Decisión del equipo: aceptado.

## 2026-10-23 16:55 · Etapa 1 · Autor en la bitácora
- Autor: JOSE AHUMADA
- Cambio: se añadió el campo de autor al formato de la bitácora y la instrucción para obtener el nombre desde la configuración de Git.
- Motivo: solicitud para identificar a quien realizó cada cambio registrado.
- Archivos: `.github/copilot-instructions.md` y `docs/bitacora-ia.md`.
- Verificación: se comprobó la hora con `Get-Date -Format "yyyy-MM-dd HH:mm"` y el autor con `git config user.name`; las entradas anteriores se conservaron.
- Decisión del equipo: pendiente

## 2026-10-23 17:00 · Etapa 1 · Reglas del cliente web
- Autor: JOSE AHUMADA
- Cambio: se añadieron las reglas del cliente web a las instrucciones de Copilot.
- Motivo: solicitud para documentar los criterios de implementación y verificación del cliente.
- Archivos: `.github/copilot-instructions.md` y `docs/bitacora-ia.md`.
- Verificación: se comprobó la inserción en el punto solicitado; al ser un cambio documental, no requiere compilación ni pruebas.
- Decisión del equipo: aceptado

## 2026-10-23 17:21 · Etapa 4.1 · Cliente web y categorías
- Cambio: se configuró Blazor en el puerto 5090 y el cliente HTTP hacia la API; la página Inicio muestra categorías como tarjetas, y la plantilla quedó en español sin páginas de ejemplo.
- Motivo: etapa 4.1 de `docs/plan.md` y sección «Cliente web» de las instrucciones del proyecto.
- Archivos: `src/Tienda.Web/Program.cs`, `Properties/launchSettings.json`, `wwwroot/appsettings.json`, `Servicios/ClienteTienda.cs`, `Servicios/ErrorApiException.cs`, `Pages/Inicio.razor` (renombrado desde `Pages/Home.razor`), `Pages/Counter.razor` y `Pages/Weather.razor` (eliminados), `wwwroot/sample-data/weather.json` (eliminado), `_Imports.razor`, `Layout/MainLayout.razor`, `Layout/NavMenu.razor`, `Layout/NavMenu.razor.css`, `Pages/NotFound.razor`, `wwwroot/index.html`, `wwwroot/css/app.css`, `README.md` y `docs/bitacora-ia.md`.
- Verificación: `dotnet build src/Tienda.Web`, compilación correcta con 0 errores y 0 advertencias.
- Decisión del equipo: aceptado

## 2026-10-23 17:49 · Etapa 4.2 · Productos por categoría
- Autor: Jaime Peña
- Cambio: se agregó la consulta paginada de productos, el formato fijo de pesos, el recuadro SKU y la página por categoría con búsqueda, tarjetas y navegación entre páginas.
- Motivo: etapa 4.2 de `docs/plan.md` y sección «Cliente web» de las instrucciones del proyecto.
- Archivos: `src/Tienda.Web/Servicios/ClienteTienda.cs`, `src/Tienda.Web/Servicios/Formato.cs`, `src/Tienda.Web/Componentes/ImagenProducto.razor`, `src/Tienda.Web/Componentes/ImagenProducto.razor.css`, `src/Tienda.Web/Pages/ProductosCategoria.razor`, `src/Tienda.Web/_Imports.razor` y `docs/bitacora-ia.md`.
- Verificación: `dotnet build src/Tienda.Web`, compilación correcta con 0 errores y 0 advertencias; `git diff --check` sin errores.
- Decisión del equipo: aceptado

## 2026-10-26 17:20 · Etapa 4.4 · Hacer y consultar pedidos
- Autor: Jaime Peña
- Cambio: se completó el flujo de carrito necesario para la etapa, con formulario validado de entrega, envío de líneas sin precios, consulta del pedido por código y detalle con el total del servidor.
- Motivo: etapa 4.4 de `docs/plan.md` y sección «Cliente web» de las instrucciones del proyecto; la página de carrito de la etapa 4.3 no existía en el workspace.
- Archivos: `src/Tienda.Web/Servicios/ClienteTienda.cs`, `src/Tienda.Web/Servicios/CarritoTienda.cs`, `src/Tienda.Web/Modelos/DatosEntrega.cs`, `src/Tienda.Web/Modelos/LineaCarrito.cs`, `src/Tienda.Web/Pages/DetalleProducto.razor`, `src/Tienda.Web/Pages/Carrito.razor`, `src/Tienda.Web/Pages/DetallePedido.razor`, `src/Tienda.Web/Program.cs`, `src/Tienda.Web/_Imports.razor`, `src/Tienda.Web/Layout/NavMenu.razor` y `docs/bitacora-ia.md`.
- Verificación: `dotnet build src/Tienda.Web`, compilación correcta con 0 errores y 0 advertencias.
- Decisión del equipo: aceptado
