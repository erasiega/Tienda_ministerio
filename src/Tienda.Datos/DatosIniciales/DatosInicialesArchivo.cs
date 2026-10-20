namespace Tienda.Datos.DatosIniciales;

/// <summary>Forma del archivo <c>datos-iniciales.json</c>.</summary>
internal class DatosInicialesArchivo
{
    public List<CategoriaInicial> Categorias { get; set; } = [];

    public List<ProductoInicial> Productos { get; set; } = [];
}
