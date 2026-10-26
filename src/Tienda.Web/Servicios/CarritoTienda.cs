using Tienda.Contratos;
using Tienda.Web.Modelos;

namespace Tienda.Web.Servicios;

/// <summary>Mantiene los productos del carrito durante la navegación por la tienda.</summary>
public sealed class CarritoTienda
{
    private readonly List<LineaCarrito> _lineas = [];

    /// <summary>Las líneas agregadas al carrito.</summary>
    public IReadOnlyList<LineaCarrito> Lineas => _lineas.AsReadOnly();

    /// <summary>El total estimado con los precios actuales del catálogo.</summary>
    public decimal TotalEstimado => _lineas.Sum(linea => linea.Subtotal);

    /// <summary>Agrega un producto o incrementa su cantidad si ya está en el carrito.</summary>
    /// <param name="producto">Producto que se agregará.</param>
    public void Agregar(ProductoDetalleRespuesta producto)
    {
        int indice = _lineas.FindIndex(linea => linea.ProductoId == producto.Id);
        if (indice < 0)
        {
            _lineas.Add(new LineaCarrito(producto.Id, producto.Sku, producto.Nombre, producto.Precio, 1));
            return;
        }

        LineaCarrito linea = _lineas[indice];
        _lineas[indice] = linea with { Cantidad = linea.Cantidad + 1 };
    }

    /// <summary>Actualiza la cantidad de una línea del carrito.</summary>
    /// <param name="productoId">Identificador del producto.</param>
    /// <param name="cantidad">Nueva cantidad, que debe ser positiva.</param>
    public void CambiarCantidad(int productoId, int cantidad)
    {
        int indice = _lineas.FindIndex(linea => linea.ProductoId == productoId);
        if (indice < 0 || cantidad < 1)
        {
            return;
        }

        _lineas[indice] = _lineas[indice] with { Cantidad = cantidad };
    }

    /// <summary>Elimina una línea del carrito.</summary>
    /// <param name="productoId">Identificador del producto.</param>
    public void Quitar(int productoId) => _lineas.RemoveAll(linea => linea.ProductoId == productoId);

    /// <summary>Vacía el carrito.</summary>
    public void Vaciar() => _lineas.Clear();
}
