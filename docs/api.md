# API de la tienda · Guía para el cliente

Lo que necesita saber quien construya el cliente web (proyecto final). Todos los contratos están en `src/Tienda.Contratos`: el cliente Blazor los referencia, no los copia.

- **Dirección base:** `http://localhost:5080`
- **Formato:** JSON, con nombres de propiedades en camelCase.
- **Errores:** `ProblemDetails` con `title`, `status` y `detail` en español. Los de reglas del negocio traen además `regla` (por ejemplo `"R-05"`).
- **CORS:** solo responde a los orígenes de `Cors:Origenes` en `src/Tienda.Api/appsettings.json`. Hoy es `http://localhost:5090`: el cliente debe correr en ese puerto, o se agrega su dirección a la lista.

| Código | Cuándo |
|---|---|
| 200 | Consulta correcta. |
| 201 | Pedido creado; el encabezado `Location` trae su ruta. |
| 400 | Datos inválidos: página fuera de rango, pedido sin líneas, correo inválido… |
| 401 | Falta la clave de administración o no es correcta. |
| 404 | No existe: producto, categoría o pedido. |
| 409 | Regla del negocio: sin stock, producto inactivo. |
| 500 | Error inesperado; el detalle queda en el registro del servidor. |

## Catálogo

### `GET /api/categorias`

Las categorías, ordenadas por nombre, con cuántos productos activos tiene cada una.

```json
[ { "id": 1, "nombre": "Neveras", "slug": "neveras", "cantidadProductos": 3 } ]
```

### `GET /api/productos`

Productos activos, ordenados por nombre, de a una página.

| Parámetro | Obligatorio | Valor |
|---|---|---|
| `categoria` | No | Slug de la categoría. Si no existe: 404. |
| `buscar` | No | Texto que debe estar en el nombre, sin importar mayúsculas. |
| `pagina` | No | Desde 1. Por defecto 1. |
| `tamano` | No | De 1 a 50. Por defecto 12. |

```json
{
  "elementos": [
    { "id": 1, "sku": "NEV-300", "nombre": "Nevera No Frost 300 L", "categoria": "neveras",
      "precio": 2899000, "disponible": true, "imagenUrl": "/imagenes/productos/nev-300.jpg" }
  ],
  "total": 3, "pagina": 1, "tamano": 12, "totalPaginas": 1
}
```

### `GET /api/categorias/{slug}/productos`

Lo mismo que `GET /api/productos?categoria={slug}`, con `pagina` y `tamano`.

### `GET /api/productos/{id}`

El detalle de un producto activo. Si no existe o está inactivo: 404. No incluye el costo de compra ni el stock exacto: solo si está disponible.

```json
{ "id": 1, "sku": "NEV-300", "nombre": "Nevera No Frost 300 L",
  "descripcion": "Nevera de dos puertas con congelador superior, sin escarcha y con luz LED.",
  "categoria": "neveras", "precio": 2899000, "disponible": true, "imagenUrl": "/imagenes/productos/nev-300.jpg" }
```

## Pedidos

### `POST /api/pedidos`

Hace un pedido. El cliente solo envía los datos de quien compra y, por cada producto, su Id y la cantidad: el precio y el total los pone el servidor, y si el cliente los envía, se ignoran.

```json
{
  "nombreCliente": "Laura Gómez",
  "documentoCliente": "52123456",
  "correoCliente": "laura@correo.com",
  "direccionEntrega": "Calle 45 # 12-30, Bogotá",
  "lineas": [ { "productoId": 1, "cantidad": 1 }, { "productoId": 24, "cantidad": 2 } ]
}
```

Respuesta 201, con `Location: /api/pedidos/{codigo}`:

```json
{
  "codigo": "4b400ba8c2c1451a8a774d55c6347bfd",
  "fecha": "2026-10-21T20:30:00",
  "nombreCliente": "Laura Gómez",
  "direccionEntrega": "Calle 45 # 12-30, Bogotá",
  "lineas": [
    { "productoId": 1, "nombreProducto": "Nevera No Frost 300 L", "cantidad": 1, "precioUnitario": 2899000, "subtotal": 2899000 },
    { "productoId": 24, "nombreProducto": "Licuadora 10 velocidades", "cantidad": 2, "precioUnitario": 389000, "subtotal": 778000 }
  ],
  "total": 3677000
}
```

| Error | Cuándo |
|---|---|
| 400 | Sin líneas, cantidad 0 o menos, un producto repetido, o faltan el nombre, el documento, la dirección o un correo válido. |
| 404 | Un producto no existe. |
| 409 | Un producto está inactivo (`regla: R-08`) o no hay stock (`regla: R-05`). En los dos casos no se guarda nada. |

El cliente debe mostrar el `codigo` a quien compró: es la única forma de consultar el pedido.

### `GET /api/pedidos/{codigo}`

Un pedido por su código, con la misma forma de la respuesta anterior. Si el código no existe: 404.

## Administración (personal de la tienda)

### `PUT /api/admin/productos/{id}/precio`

Cambia el precio de venta. Pide el encabezado `X-Clave-Admin` con la clave de administración; sin ella o con una incorrecta: 401. Precio 0 o menos: 400.

```json
{ "precio": 2799000 }
```

Este endpoint no es para el cliente web público: la clave nunca va en el código del cliente.
