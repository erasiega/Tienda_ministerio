using Tienda.Contratos;
using Tienda.Core.Entidades;
using Tienda.Core.Servicios;

namespace Tienda.Api.Mapeadores;

/// <summary>Convierte entre los contratos de pedidos y lo que usa el núcleo, en un solo lugar.</summary>
internal static class MapeadorPedido
{
    /// <summary>Convierte la solicitud del cliente en los datos del servicio. No hay precios: los pone el servidor (R-06).</summary>
    internal static DatosPedido ADatos(CrearPedidoSolicitud solicitud) =>
        new(solicitud.NombreCliente, solicitud.DocumentoCliente, solicitud.CorreoCliente, solicitud.DireccionEntrega,
            (solicitud.Lineas ?? []).Select(linea => new DatosLineaPedido(linea.ProductoId, linea.Cantidad)).ToList());

    /// <summary>Convierte un pedido en el contrato que ve quien compró, sin el Id interno (R-09).</summary>
    internal static PedidoRespuesta ARespuesta(Pedido pedido) =>
        new(pedido.Codigo, pedido.Fecha, pedido.NombreCliente, pedido.DireccionEntrega,
            pedido.Lineas.Select(linea => new LineaPedidoRespuesta(linea.ProductoId, linea.NombreProducto,
                linea.Cantidad, linea.PrecioUnitario, linea.Subtotal)).ToList(),
            pedido.Total);
}
