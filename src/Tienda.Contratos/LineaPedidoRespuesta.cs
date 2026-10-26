namespace Tienda.Contratos;

/// <summary>Un producto de un pedido registrado.</summary>
/// <param name="ProductoId">Producto.</param>
/// <param name="NombreProducto">Nombre del producto en el momento de la compra.</param>
/// <param name="Cantidad">Unidades.</param>
/// <param name="PrecioUnitario">Precio de una unidad, puesto por el servidor.</param>
/// <param name="Subtotal">Cantidad por precio unitario.</param>
public record LineaPedidoRespuesta(int ProductoId, string NombreProducto, int Cantidad, decimal PrecioUnitario, decimal Subtotal);
