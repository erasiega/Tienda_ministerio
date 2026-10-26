namespace Tienda.Core.Servicios;

/// <summary>Lo que envía quien compra por cada producto: no trae precio (R-06).</summary>
/// <param name="ProductoId">Producto que se pide.</param>
/// <param name="Cantidad">Unidades, mayor que cero (R-04).</param>
public record DatosLineaPedido(int ProductoId, int Cantidad);
