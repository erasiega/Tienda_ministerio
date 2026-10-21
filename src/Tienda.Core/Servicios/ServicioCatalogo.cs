using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;
using Tienda.Core.Repositorios;

namespace Tienda.Core.Servicios;

/// <summary>Casos de uso del catálogo: categorías, búsqueda de productos, detalle y cambio de precio.</summary>
public class ServicioCatalogo
{
    private readonly IRepositorioCategorias _categorias;
    private readonly IRepositorioProductos _productos;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    /// <summary>Crea el servicio con sus dependencias.</summary>
    /// <param name="categorias">Repositorio de categorías.</param>
    /// <param name="productos">Repositorio de productos.</param>
    /// <param name="unidadDeTrabajo">Guarda los cambios.</param>
    public ServicioCatalogo(IRepositorioCategorias categorias, IRepositorioProductos productos, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _categorias = categorias;
        _productos = productos;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    /// <summary>Lista las categorías con cuántos productos activos tiene cada una (CA-01).</summary>
    /// <returns>Las categorías con su cantidad de productos.</returns>
    public async Task<IReadOnlyList<CategoriaConCantidad>> ListarCategoriasAsync()
    {
        IReadOnlyList<Categoria> categorias = await _categorias.ListarAsync();
        IReadOnlyDictionary<int, int> cantidadesPorCategoria = await _productos.ContarActivosPorCategoriaAsync();
        var categoriasConCantidad = new List<CategoriaConCantidad>();
        foreach (Categoria categoria in categorias)
        {
            categoriasConCantidad.Add(new CategoriaConCantidad(
                categoria,
                cantidadesPorCategoria.TryGetValue(categoria.Id, out int cantidadProductos) ? cantidadProductos : 0));
        }
        return categoriasConCantidad;
    }

    /// <summary>Busca productos activos por categoría y por nombre, de a una página (CA-02 a CA-06).</summary>
    /// <param name="slugCategoria">Slug de la categoría; vacío para todas.</param>
    /// <param name="texto">Texto que debe estar en el nombre; vacío para no filtrar.</param>
    /// <param name="pagina">Número de página, desde 1.</param>
    /// <param name="tamano">Productos por página, de 1 a 50.</param>
    /// <returns>La página de productos.</returns>
    /// <exception cref="DatosInvalidosException">Si la página o el tamaño no son válidos.</exception>
    /// <exception cref="NoEncontradoException">Si la categoría no existe.</exception>
    public async Task<Pagina<Producto>> BuscarProductosAsync(string? slugCategoria, string? texto, int pagina, int tamano)
    {
        if (pagina < ConstantesPaginacion.PaginaMinima)
        {
            throw new DatosInvalidosException($"La página debe ser {ConstantesPaginacion.PaginaMinima} o mayor.");
        }
        if (tamano < ConstantesPaginacion.TamanoPaginaMinimo || tamano > ConstantesPaginacion.TamanoPaginaMaximo)
        {
            throw new DatosInvalidosException(
                $"El tamaño de la página debe estar entre {ConstantesPaginacion.TamanoPaginaMinimo} y {ConstantesPaginacion.TamanoPaginaMaximo}.");
        }

        int? categoriaId = null;
        if (!string.IsNullOrWhiteSpace(slugCategoria))
        {
            Categoria categoria = await _categorias.ObtenerPorSlugAsync(slugCategoria.Trim().ToLowerInvariant())
                ?? throw new NoEncontradoException($"No existe la categoría {slugCategoria}.");
            categoriaId = categoria.Id;
        }

        string? textoLimpio = string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        var (productos, total) = await _productos.BuscarActivosAsync(
            categoriaId, textoLimpio, (pagina - ConstantesPaginacion.PaginaMinima) * tamano, tamano);
        return new Pagina<Producto>(productos, total, pagina, tamano);
    }

    /// <summary>Busca un producto activo para mostrar su detalle (CA-07, CA-08).</summary>
    /// <param name="id">Identificador.</param>
    /// <returns>El producto.</returns>
    /// <exception cref="NoEncontradoException">Si no existe o está inactivo (R-08).</exception>
    public async Task<Producto> ObtenerProductoAsync(int id)
    {
        Producto? producto = await _productos.ObtenerPorIdAsync(id);
        if (producto is null || !producto.Activo)
        {
            throw new NoEncontradoException($"No existe el producto {id}.");
        }
        return producto;
    }

    /// <summary>Cambia el precio de venta de un producto (CA-18, CA-20). Quién puede hacerlo lo controla la API (R-10).</summary>
    /// <param name="id">Producto.</param>
    /// <param name="nuevoPrecio">Precio nuevo, mayor que cero (R-02).</param>
    /// <returns>El producto con el precio nuevo.</returns>
    /// <exception cref="NoEncontradoException">Si el producto no existe.</exception>
    public async Task<Producto> CambiarPrecioAsync(int id, decimal nuevoPrecio)
    {
        Producto producto = await _productos.ObtenerPorIdAsync(id)
            ?? throw new NoEncontradoException($"No existe el producto {id}.");
        producto.CambiarPrecio(nuevoPrecio);
        await _unidadDeTrabajo.GuardarCambiosAsync();
        return producto;
    }
}
