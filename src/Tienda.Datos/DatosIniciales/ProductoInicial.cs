namespace Tienda.Datos.DatosIniciales;

/// <summary>Un producto del archivo de datos iniciales.</summary>
internal class ProductoInicial
{
    public string Sku { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public decimal Costo { get; set; }

    public int Stock { get; set; }

    public string Imagen { get; set; } = string.Empty;

    public bool Activo { get; set; }
}
