using Tienda.Core.Excepciones;

namespace Tienda.Core.Entidades;

/// <summary>
/// Un electrodoméstico del catálogo. Protege sus reglas: el precio y el stock
/// solo cambian con sus métodos (R-02, R-03).
/// </summary>
public class Producto
{
    /// <summary>Constructor para EF Core.</summary>
    private Producto()
    {
    }

    /// <summary>Crea un producto activo.</summary>
    /// <param name="sku">Código único (R-01); se guarda en mayúsculas.</param>
    /// <param name="nombre">Nombre comercial.</param>
    /// <param name="descripcion">Descripción para la página del producto.</param>
    /// <param name="categoriaId">Categoría a la que pertenece.</param>
    /// <param name="precioVenta">Precio de venta, mayor que cero (R-02).</param>
    /// <param name="costoCompra">Costo de compra; es interno y no se muestra al visitante.</param>
    /// <param name="stock">Unidades disponibles, cero o más (R-03).</param>
    /// <param name="imagenUrl">Ruta de la imagen.</param>
    public Producto(string sku, string nombre, string descripcion, int categoriaId,
        decimal precioVenta, decimal costoCompra, int stock, string imagenUrl)
    {
        Sku = Validacion.TextoObligatorio(sku, "El SKU").ToUpperInvariant();
        Nombre = Validacion.TextoObligatorio(nombre, "El nombre");
        Descripcion = descripcion?.Trim() ?? string.Empty;
        CategoriaId = categoriaId;
        Validacion.ValorPositivo(precioVenta, "El precio de venta", "R-02");
        PrecioVenta = precioVenta;
        Validacion.ValorPositivo(costoCompra, "El costo de compra", "R-02");
        CostoCompra = costoCompra;
        if (stock < 0)
        {
            throw new DatosInvalidosException("El stock no puede ser negativo (R-03).");
        }
        Stock = stock;
        ImagenUrl = imagenUrl?.Trim() ?? string.Empty;
        Activo = true;
    }

    /// <summary>Identificador.</summary>
    public int Id { get; private set; }

    /// <summary>Código único, en mayúsculas (R-01).</summary>
    public string Sku { get; private set; } = string.Empty;

    /// <summary>Nombre comercial.</summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>Descripción para la página del producto.</summary>
    public string Descripcion { get; private set; } = string.Empty;

    /// <summary>Categoría a la que pertenece.</summary>
    public int CategoriaId { get; private set; }

    /// <summary>La categoría, cuando se carga desde la base de datos.</summary>
    public Categoria? Categoria { get; private set; }

    /// <summary>Precio de venta en pesos (R-02).</summary>
    public decimal PrecioVenta { get; private set; }

    /// <summary>Costo de compra en pesos. Es un dato interno de la tienda.</summary>
    public decimal CostoCompra { get; private set; }

    /// <summary>Unidades disponibles. Nunca es negativo (R-03).</summary>
    public int Stock { get; private set; }

    /// <summary>Ruta de la imagen.</summary>
    public string ImagenUrl { get; private set; } = string.Empty;

    /// <summary>Si es falso, no se muestra ni se vende (R-08).</summary>
    public bool Activo { get; private set; }

    /// <summary>Lo que ve el visitante: si hay al menos una unidad (P-06).</summary>
    public bool Disponible => Stock > 0;

    /// <summary>Cambia el precio de venta (R-02).</summary>
    /// <param name="nuevoPrecio">Precio nuevo, mayor que cero.</param>
    public void CambiarPrecio(decimal nuevoPrecio)
    {
        Validacion.ValorPositivo(nuevoPrecio, "El precio de venta", "R-02");
        PrecioVenta = nuevoPrecio;
    }

    /// <summary>Indica si hay stock para la cantidad pedida.</summary>
    /// <param name="cantidad">Unidades pedidas.</param>
    /// <returns><c>true</c> si la cantidad cabe en el stock.</returns>
    public bool TieneStockPara(int cantidad) => cantidad <= Stock;

    /// <summary>Baja el stock por un pedido. Si no alcanza, no cambia nada (R-03).</summary>
    /// <param name="cantidad">Unidades pedidas, mayor que cero.</param>
    public void DisminuirStock(int cantidad)
    {
        Validacion.CantidadPositiva(cantidad, "R-04");
        if (!TieneStockPara(cantidad))
        {
            throw new ReglaDelNegocioException(
                $"No hay stock suficiente de {Sku}: hay {Stock} y se piden {cantidad}.", "R-03");
        }
        Stock -= cantidad;
    }

    /// <summary>Saca el producto del catálogo sin borrarlo (R-08).</summary>
    public void Desactivar() => Activo = false;
}
