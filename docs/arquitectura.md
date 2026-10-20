# Arquitectura

Etapa 2 del método. Dos opciones para una solución .NET modular, juzgadas con las consideraciones C-02 a C-06 del enunciado.

## Opción A · Núcleo al centro, en capas

| Proyecto | Responsabilidad |
|---|---|
| `src/Tienda.Core` | Entidades, reglas del negocio, excepciones, interfaces de los repositorios y servicios. Sin dependencias. |
| `src/Tienda.Contratos` | Solicitudes y respuestas que viajan por HTTP. Lo comparten el servidor y el cliente. |
| `src/Tienda.Datos` | EF Core con SQLite: contexto, configuración de tablas, repositorios y datos iniciales. |
| `src/Tienda.Api` | Endpoints, manejo de errores, CORS y configuración. |
| `src/Tienda.Web` | Cliente Blazor WebAssembly (proyecto final). |
| `tests/Tienda.Pruebas` | Pruebas unitarias con xUnit. Solo referencia `Tienda.Core`. |

```
Tienda.Web ──► Tienda.Contratos ◄── Tienda.Api ──► Tienda.Core ◄── Tienda.Datos
                                        │                ▲
                                        └──► Tienda.Datos │
                              tests/Tienda.Pruebas ───────┘
```

| Consideración | Cómo la cumple |
|---|---|
| C-02 | `Tienda.Core` no referencia ningún proyecto ni paquete. |
| C-03 | El motor se elige en `Tienda.Datos` con `MotorBaseDeDatos` y la configuración; nadie más sabe qué motor hay. |
| C-04 | Los servicios reciben interfaces: en las pruebas se usan repositorios falsos en memoria. |
| C-05 | `Tienda.Contratos` lo usan la API y el cliente. |
| C-06 | Las entidades y los servicios validan; la API traduce las excepciones a 400, 404 y 409. |

## Opción B · Un solo proyecto de servidor

`Tienda.Api` con carpetas `Modelos`, `Datos` y `Endpoints`, más `Tienda.Web` y las pruebas.

| Consideración | Cómo la cumple |
|---|---|
| C-02 | No la cumple: las reglas viven junto a EF Core y ASP.NET. |
| C-03 | A medias: el motor se elige en un lugar, pero las consultas están en los endpoints. |
| C-04 | No la cumple: para probar una regla hay que levantar la base de datos. |
| C-05 | El cliente tendría que copiar las clases o referenciar todo el servidor. |
| C-06 | Se puede, pero la validación queda repartida en los endpoints. |

## Comparación

| Criterio | Opción A | Opción B |
|---|---|---|
| Probar reglas sin base de datos | Sí | No |
| Cambiar de motor | Se toca `Tienda.Datos` y la configuración | Se tocan los endpoints |
| Agregar el cliente | Referencia `Tienda.Contratos` | Copiar clases |
| Cantidad de proyectos | 5 y las pruebas | 2 y las pruebas |
| Esfuerzo inicial | Mayor | Menor |

**Recomendación:** la opción A. Cuesta un poco más al inicio, pero es la única que cumple C-02, C-04 y C-05.

## Decisión del equipo

Elegimos la opción A. Para pasar a SQL Server se toca solo `Tienda.Datos` (un caso en el `switch` y el paquete) y la cadena de conexión: el núcleo y la API no se enteran.
