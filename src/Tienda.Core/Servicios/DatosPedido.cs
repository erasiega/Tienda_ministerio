namespace Tienda.Core.Servicios;

/// <summary>Lo que envía quien compra para hacer un pedido. No trae precios ni totales (R-06, R-07).</summary>
/// <param name="NombreCliente">Nombre de quien compra.</param>
/// <param name="DocumentoCliente">Documento de quien compra.</param>
/// <param name="CorreoCliente">Correo de quien compra.</param>
/// <param name="DireccionEntrega">Dirección de entrega.</param>
/// <param name="Lineas">Productos y cantidades.</param>
public record DatosPedido(string NombreCliente, string DocumentoCliente, string CorreoCliente,
    string DireccionEntrega, IReadOnlyList<DatosLineaPedido> Lineas);
