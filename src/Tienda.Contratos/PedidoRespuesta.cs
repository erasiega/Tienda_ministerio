namespace Tienda.Contratos;

/// <summary>Un pedido registrado, como lo ve quien compró. No incluye el Id interno (R-09).</summary>
/// <param name="Codigo">Código para consultar el pedido.</param>
/// <param name="Fecha">Fecha y hora del pedido.</param>
/// <param name="NombreCliente">Nombre de quien compró.</param>
/// <param name="DireccionEntrega">Dirección de entrega.</param>
/// <param name="Lineas">Productos del pedido.</param>
/// <param name="Total">Total calculado por el servidor (R-07).</param>
public record PedidoRespuesta(string Codigo, DateTime Fecha, string NombreCliente, string DireccionEntrega,
    IReadOnlyList<LineaPedidoRespuesta> Lineas, decimal Total);
