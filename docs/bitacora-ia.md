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
