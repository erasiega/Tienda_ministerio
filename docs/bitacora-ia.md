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
