namespace Tienda.Core.Entidades;

/// <summary>Un producto dentro de un pedido, con el precio del momento de la compra (P-02).</summary>
public class LineaPedido
{
    /// <summary>Constructor para EF Core.</summary>
    private LineaPedido()
    {
    }

    /// <summary>Crea una línea.</summary>
    /// <param name="productoId">Producto que se compra.</param>
    /// <param name="nombreProducto">Nombre del producto en el momento de la compra.</param>
    /// <param name="cantidad">Unidades, mayor que cero (R-04).</param>
    /// <param name="precioUnitario">Precio de venta del producto, puesto por el servidor (R-06).</param>
    public LineaPedido(int productoId, string nombreProducto, int cantidad, decimal precioUnitario)
    {
        Validacion.CantidadPositiva(cantidad, "R-04");
        Validacion.ValorPositivo(precioUnitario, "El precio unitario", "R-02");
        ProductoId = productoId;
        NombreProducto = Validacion.TextoObligatorio(nombreProducto, "El nombre del producto");
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }

    /// <summary>Identificador.</summary>
    public int Id { get; private set; }

    /// <summary>Producto que se compra.</summary>
    public int ProductoId { get; private set; }

    /// <summary>Nombre del producto en el momento de la compra.</summary>
    public string NombreProducto { get; private set; } = string.Empty;

    /// <summary>Unidades.</summary>
    public int Cantidad { get; private set; }

    /// <summary>Precio de una unidad en el momento de la compra.</summary>
    public decimal PrecioUnitario { get; private set; }

    /// <summary>Cantidad por precio unitario.</summary>
    public decimal Subtotal => Cantidad * PrecioUnitario;
}
