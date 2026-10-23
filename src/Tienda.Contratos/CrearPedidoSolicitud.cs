namespace Tienda.Contratos;

/// <summary>Lo que envía el cliente para hacer un pedido. No lleva precios ni totales: los pone el servidor (R-06, R-07).</summary>
/// <param name="NombreCliente">Nombre de quien compra.</param>
/// <param name="DocumentoCliente">Documento de quien compra.</param>
/// <param name="CorreoCliente">Correo de quien compra.</param>
/// <param name="DireccionEntrega">Dirección de entrega.</param>
/// <param name="Lineas">Productos y cantidades.</param>
public record CrearPedidoSolicitud(string NombreCliente, string DocumentoCliente, string CorreoCliente,
    string DireccionEntrega, IReadOnlyList<LineaPedidoSolicitud> Lineas);
