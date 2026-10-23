# Estándares del proyecto Tienda

Somos un equipo que desarrolla en C# con .NET 10 un sistema cliente-servidor: la tienda web de electrodomésticos Hogar Total. Estas reglas aplican a todo el código y a toda la documentación del repositorio. El problema está en `docs/enunciado.md`, los criterios de aceptación en `docs/analisis.md`, la arquitectura en `docs/arquitectura.md` y las etapas en `docs/plan.md`.

## Estructura y modularidad

- La solución `Tienda.slnx` tiene sus proyectos en `src/`: `Tienda.Core` (entidades, reglas, interfaces y servicios), `Tienda.Contratos` (lo que viaja por HTTP), `Tienda.Datos` (EF Core), `Tienda.Api` (endpoints) y, en el proyecto final, `Tienda.Web` (cliente Blazor). Las pruebas van en `tests/Tienda.Pruebas`.
- `Tienda.Core` no depende de ningún otro proyecto ni de paquetes de bases de datos o de ASP.NET.
- Las dependencias apuntan hacia el núcleo: `Tienda.Datos` usa `Tienda.Core`; `Tienda.Api` usa `Tienda.Core`, `Tienda.Datos` y `Tienda.Contratos`; `Tienda.Web` solo usará `Tienda.Contratos`.
- Una clase, interfaz, record o enumeración por archivo, con el mismo nombre del archivo.

## Principios de diseño

- Responsabilidad única: los endpoints reciben y responden; las reglas del negocio están en el núcleo; el acceso a datos está en los repositorios.
- Inversión de dependencias: el núcleo define las interfaces de los repositorios y `Tienda.Datos` las implementa.
- Abierto/cerrado: cambiar de motor de base de datos no obliga a tocar el núcleo ni la API.
- Las entidades protegen sus reglas: el stock y el precio solo cambian con métodos de la entidad, nunca con un `set` público.
- Sin código duplicado: si la misma lógica aparece dos veces, va en un solo método.

## Nombres e idioma

- Nombres, mensajes y documentación en español, sin abreviaturas (`precioVenta`, no `pv`; `productos`, no `lst`).
- Clases y métodos en PascalCase; variables y parámetros en camelCase; campos privados con guion bajo (`_productos`).
- Los métodos asíncronos terminan en `Async`.
- Nada de números mágicos: constantes con nombre.
- Nada de código muerto: ni métodos sin uso ni bloques comentados.

## API

- Rutas en plural y en minúscula: `/api/categorias`, `/api/productos`, `/api/pedidos`. Lo del personal de la tienda va en `/api/admin/...`.
- Los endpoints reciben y devuelven contratos de `Tienda.Contratos`, nunca entidades.
- Códigos: 200 consulta, 201 creación (con la ruta del recurso nuevo), 204 sin contenido, 400 datos inválidos, 401 sin la clave de administración, 404 no existe, 409 regla del negocio violada.
- Los errores se responden con `ProblemDetails` y un mensaje en español que dice qué falló y, si aplica, qué regla (por ejemplo «R-05»).

## Datos

- Entity Framework Core con SQLite. El motor se elige en un solo lugar de `Tienda.Datos`; la cadena de conexión va en `appsettings.json`.
- Nada de SQL escrito a mano: las consultas se hacen con LINQ.
- Un pedido se guarda en una sola operación: todo o nada.

## Seguridad y validación

- El servidor valida todo lo que recibe y nunca confía en el cliente: los precios, los totales y el stock los calcula el servidor.
- Una entrada inválida se responde con 400; el servidor nunca se cae por una entrada inválida.
- Los errores inesperados responden 500 con un mensaje general; el detalle técnico solo va al registro (log), nunca a la respuesta.
- No hay contraseñas, claves ni bases de datos en el repositorio. Las claves se leen de la configuración: secretos de usuario en desarrollo (`dotnet user-secrets`) y variables de entorno en producción.
- CORS solo permite el origen del cliente, configurado en `appsettings.json`.
- Las respuestas no incluyen datos internos, como el costo de compra.
- Los paquetes NuGet no tienen vulnerabilidades conocidas: se revisan con `dotnet list package --vulnerable`.
- El proyecto compila sin errores y sin advertencias.

## Documentación

- Todo lo público lleva comentario XML (`///`) con `summary`, y `param` y `returns` cuando aplica.
- Los comentarios explican el porqué, no repiten el código.
- El `README.md` explica qué hace el proyecto, cómo se ejecuta, cómo se prueba y cómo está organizado.

## Forma de trabajar

- Cada solicitud resuelve una sola etapa de `docs/plan.md`.
- Antes de cambiar código, lee `docs/arquitectura.md` y `docs/plan.md`.
- Puedes ejecutar `dotnet build`, `dotnet test` y `dotnet add package`. No ejecutes `dotnet run`: el servidor se queda corriendo y el chat no termina.

## Pruebas

- Las pruebas usan xUnit y viven en `tests/Tienda.Pruebas`, que solo referencia `Tienda.Core`.
- Cada regla del negocio del núcleo tiene al menos una prueba unitaria, basada en los criterios de aceptación de `docs/analisis.md`, con el ID del criterio en un comentario.
- Las pruebas se escriben a partir de los criterios, no copiando lo que hace el código.
- Patrón Arrange-Act-Assert y nombres `Metodo_Escenario_ResultadoEsperado`.
- Se prueban el camino feliz, los errores y los bordes (por ejemplo, pedir exactamente el stock que hay).
- Las pruebas unitarias no usan base de datos, red, archivos ni la fecha real; los repositorios se reemplazan por repositorios falsos en memoria y la fecha llega por `TimeProvider`.
- Cada prueba es independiente: no depende del orden ni del resultado de otra.

## Verificación

- Cada etapa se verifica con `dotnet build` y `dotnet test`, sin errores ni advertencias.
- Las reglas del núcleo se verifican con pruebas; los endpoints, con las peticiones de la carpeta `peticiones` (archivos `.http`), con el resultado esperado de cada una.
- No cambies los criterios de aceptación, las pruebas ni las peticiones de prueba para que el código pase. Si algo falla, se corrige el código; si crees que la prueba está mal, dilo en el chat y no la cambies.

## Bitácora de cambios con IA

Después de cada cambio que hagas en el proyecto, agrega una entrada al final de `docs/bitacora-ia.md` con este formato:

```
## AAAA-MM-DD HH:mm · Etapa X.Y · Título corto
- Autor: nombre de quien hizo el cambio.
- Cambio: qué se hizo, en una o dos líneas.
- Motivo: el hallazgo, la regla o la etapa del plan que lo pide.
- Archivos: los archivos creados o modificados.
- Verificación: cómo se comprobó.
- Decisión del equipo: pendiente
```

Para la fecha y la hora, ejecuta en la terminal `Get-Date -Format "yyyy-MM-dd HH:mm"`.
Para el autor, ejecuta en la terminal `git config user.name`. No borres ni cambies entradas anteriores. Si no hiciste cambios, no agregues entrada.
