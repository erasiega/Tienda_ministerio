# Enunciado: tienda web de electrodomésticos

## El cliente

**Hogar Total** vende electrodomésticos en Bogotá: neveras, lavadoras, televisores, cocina y pequeños electrodomésticos. Hoy recibe los pedidos por WhatsApp y los anota en una hoja de cálculo. Le pasa lo de siempre: vende productos que ya no tiene, cobra precios viejos y a veces no encuentra un pedido cuando el cliente pregunta por él.

Quiere una **tienda web**: que cualquier persona pueda entrar, ver los productos por categoría, buscarlos, armar un carrito y hacer el pedido sin crear una cuenta.

## Alcance

El sistema es **cliente-servidor y todo en C#**:

- **Servidor:** una API web en ASP.NET Core con la lógica del negocio y la base de datos.
- **Cliente:** una aplicación web en Blazor que solo muestra y envía datos. Se construye en el proyecto final.

Este proyecto entrega primero el **servidor completo**, listo para que el cliente lo use.

## Lo que el sistema debe hacer

1. Mostrar las categorías, con cuántos productos tiene cada una.
2. Mostrar los productos de una categoría, con búsqueda por nombre y en páginas.
3. Mostrar el detalle de un producto: nombre, descripción, precio, si hay disponibilidad e imagen.
4. Registrar un pedido con varios productos y los datos de quien compra y de la entrega.
5. Consultar un pedido con el código que se le entrega a quien compra.
6. Permitir que el personal de la tienda cambie el precio de un producto.

## Entidades

| Entidad | Datos |
|---|---|
| Categoría | Nombre, identificador para la dirección web (slug). |
| Producto | SKU, nombre, descripción, categoría, precio de venta, costo de compra (interno), stock, imagen, activo. |
| Pedido | Código, fecha, nombre, documento, correo y dirección de quien compra, líneas y total. |
| Línea de pedido | Producto, cantidad, precio unitario y subtotal. |

## Reglas del negocio

| ID | Regla |
|---|---|
| R-01 | El SKU de un producto es único. |
| R-02 | El precio de venta de un producto es mayor que cero. |
| R-03 | El stock nunca es negativo. Solo baja con los pedidos. |
| R-04 | Un pedido tiene al menos una línea. Cada línea tiene cantidad mayor que cero y un producto no se repite en el mismo pedido. |
| R-05 | Un pedido se registra completo o no se registra: si alguna línea no tiene stock suficiente, el pedido no se guarda y el stock de ningún producto cambia. |
| R-06 | El precio de cada línea lo pone el servidor, con el precio de venta del producto en ese momento. Quien compra solo envía el producto y la cantidad. |
| R-07 | El total del pedido lo calcula el servidor. |
| R-08 | Un producto inactivo no se muestra en el catálogo ni se puede comprar. |
| R-09 | Un pedido solo se consulta con su código, y el código no se puede adivinar. |
| R-10 | Solo el personal de la tienda puede cambiar el precio de un producto. |

## Consideraciones técnicas

| ID | Consideración |
|---|---|
| C-01 | Cliente-servidor, todo en C# con .NET 10. El cliente nunca habla con la base de datos. |
| C-02 | El núcleo (entidades, reglas y servicios) no depende de la API ni de la base de datos. |
| C-03 | La base de datos es SQLite, pero cambiar de motor solo debe tocar el proyecto de datos y la configuración. |
| C-04 | Las reglas del núcleo se prueban con pruebas unitarias, sin base de datos. |
| C-05 | Lo que viaja entre cliente y servidor está en un proyecto de contratos que ambos comparten. |
| C-06 | El servidor valida todo lo que recibe y nunca confía en el cliente. Los errores responden con el código HTTP correcto y sin detalles internos. |
| C-07 | Ni secretos ni la base de datos se suben al repositorio. |
| C-08 | Cada clase pública tiene documentación XML y el README explica cómo correr y probar el servidor. |

## Lo que queda por fuera

Cuentas de usuario e inicio de sesión, pagos en línea, envíos e inventario de proveedores.
