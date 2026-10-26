namespace Tienda.Web.Modelos;

/// <summary>Un producto y su cantidad en el carrito.</summary>
public sealed record LineaCarrito(int ProductoId, string Sku, string Nombre, decimal Precio, int Cantidad)
{
    /// <summary>Subtotal estimado con el precio actual del catálogo.</summary>
    public decimal Subtotal => Precio * Cantidad;
}
