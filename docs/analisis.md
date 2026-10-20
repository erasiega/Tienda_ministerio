# Análisis del problema

Etapa 0 del método. Sale de `docs/enunciado.md`. Cada criterio de aceptación es un caso concreto que se puede probar.

## HU-01 · Ver las categorías

Como visitante, quiero ver las categorías de la tienda para saber qué venden.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-01 | Hay 5 categorías y «Neveras» tiene 3 productos activos y 1 inactivo | Consulto las categorías | Veo las 5, y «Neveras» dice 3 productos | R-08 |

## HU-02 · Ver y buscar productos

Como visitante, quiero ver los productos de una categoría y buscarlos por nombre para encontrar lo que necesito.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-02 | La categoría `neveras` tiene 3 productos activos | Consulto sus productos | Veo los 3, con nombre, precio, imagen y si hay disponibilidad | R-08 |
| CA-03 | Hay 30 productos activos | Consulto la página 2 de tamaño 12 | Veo los productos 13 a 24 y el total es 30 | — |
| CA-04 | Existe «Nevera No Frost 300 L» | Busco «no frost» | Aparece, sin importar mayúsculas | — |
| CA-05 | Un producto está inactivo | Consulto la lista o lo busco | No aparece | R-08 |
| CA-06 | Pido una página 0 o un tamaño mayor que 50 | Consulto los productos | Se rechaza con 400 | C-06 |

## HU-03 · Ver el detalle de un producto

Como visitante, quiero ver el detalle de un producto para decidir si lo compro.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-07 | Existe el producto 1, activo | Lo consulto | Veo SKU, nombre, descripción, categoría, precio, disponibilidad e imagen; no veo el costo de compra | C-06 |
| CA-08 | No existe el producto 9999, o está inactivo | Lo consulto | Respuesta 404 | R-08 |

## HU-04 · Hacer un pedido

Como visitante, quiero hacer un pedido con varios productos para comprarlos sin crear una cuenta.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-09 | `NEV-300` cuesta 2.899.000 con stock 5 y `LIC-001` cuesta 389.000 con stock 10 | Pido 1 `NEV-300` y 2 `LIC-001` | Se crea con 201, el total es 3.677.000, los stocks quedan en 4 y 8, y recibo un código | R-05, R-07 |
| CA-10 | Un pedido sin líneas, o con una línea de cantidad 0 | Lo envío | Se rechaza con 400 | R-04 |
| CA-11 | Un pedido con el mismo producto en dos líneas | Lo envío | Se rechaza con 400 | R-04 |
| CA-12 | `NEV-300` con stock 1 y `LIC-001` con stock 10 | Pido 1 `LIC-001` y 2 `NEV-300` | Se rechaza con 409, no se guarda y los stocks siguen en 1 y 10 | R-05 |
| CA-13 | La solicitud trae un precio para la línea | Envío el pedido | Se ignora: el precio es el del producto | R-06 |
| CA-14 | Un producto inactivo o que no existe | Lo pido | Se rechaza con 409 o 404 y no se guarda nada | R-08 |
| CA-15 | Falta el nombre, el documento, la dirección o el correo es inválido | Envío el pedido | Se rechaza con 400 | C-06 |

## HU-05 · Consultar mi pedido

Como comprador, quiero consultar mi pedido con su código para saber qué pedí.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-16 | Hice un pedido y tengo su código | Lo consulto con el código | Veo la fecha, las líneas, los subtotales y el total | R-07 |
| CA-17 | Tengo un código que no existe, o el número de un pedido | Consulto | Respuesta 404 | R-09 |

## HU-06 · Cambiar un precio (personal de la tienda)

Como personal de la tienda, quiero cambiar el precio de un producto para mantenerlo al día.

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-18 | Tengo la clave de administración | Cambio el precio de `NEV-300` a 2.799.000 | Queda en 2.799.000 | R-10 |
| CA-19 | No tengo la clave, o es incorrecta | Intento cambiar el precio | Se rechaza con 401 y el precio no cambia | R-10 |
| CA-20 | Tengo la clave | Cambio el precio a 0 | Se rechaza con 400 | R-02 |

## Reglas que cuida la base de datos

| ID | Dado | Cuando | Entonces | Regla |
|---|---|---|---|---|
| CA-21 | Ya existe el producto `NEV-300` | Se intenta guardar otro producto con el SKU `nev-300` | La base de datos lo rechaza: el SKU se guarda en mayúsculas y tiene índice único | R-01 |

## Preguntas abiertas

| ID | Pregunta | Decisión del equipo |
|---|---|---|
| P-01 | ¿Qué pasa si un pedido trae el mismo producto en dos líneas? | Se rechaza con 400 (CA-11). |
| P-02 | ¿El precio del pedido cambia si después cambia el precio del producto? | No: la línea guarda el precio del momento. |
| P-03 | ¿Los precios incluyen IVA? | Sí; el sistema no lo calcula aparte. |
| P-04 | ¿Se puede cancelar o editar un pedido? | No en esta versión. |
| P-05 | ¿Cómo se identifica el personal de la tienda sin cuentas de usuario? | Con una clave de administración en un encabezado; la clave no va en el repositorio. |
| P-06 | ¿Qué pasa con la disponibilidad que ve el visitante? | Se muestra «disponible» o «agotado», no el número exacto de unidades. |
