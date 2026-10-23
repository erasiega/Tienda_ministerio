namespace Tienda.Contratos;

/// <summary>Un producto del pedido, como lo envía el cliente: sin precio (R-06).</summary>
/// <param name="ProductoId">Producto que se pide.</param>
/// <param name="Cantidad">Unidades, mayor que cero.</param>
public record LineaPedidoSolicitud(int ProductoId, int Cantidad);
