using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;
using Tienda.Core.Servicios;
using Tienda.Pruebas.Falsos;

namespace Tienda.Pruebas.Servicios;

public class ServicioCatalogoPruebas
{
    private readonly RepositorioCategoriasFalso _categorias = new();
    private readonly RepositorioProductosFalso _productos = new();
    private readonly UnidadDeTrabajoFalsa _unidad = new();
    private readonly ServicioCatalogo _servicio;
    private readonly Categoria _neveras;
    private readonly Categoria _televisores;

    public ServicioCatalogoPruebas()
    {
        _servicio = new ServicioCatalogo(_categorias, _productos, _unidad);
        _neveras = _categorias.Existente("Neveras", "neveras");
        _televisores = _categorias.Existente("Televisores", "televisores");
    }

    // CA-01 · R-08
    [Fact]
    public async Task ListarCategoriasAsync_ConUnProductoInactivo_SoloCuentaLosActivos()
    {
        _productos.Existente("NEV-300", "Nevera No Frost 300 L", _neveras.Id);
        _productos.Existente("NEV-250", "Nevera 250 L", _neveras.Id);
        _productos.Existente("NEV-450", "Nevera Side by Side 450 L", _neveras.Id);
        _productos.Existente("NEV-100", "Nevera minibar 100 L", _neveras.Id, activo: false);

        IReadOnlyList<CategoriaConCantidad> categorias = await _servicio.ListarCategoriasAsync();

        Assert.Equal(2, categorias.Count);
        Assert.Equal(3, categorias.Single(c => c.Categoria.Slug == "neveras").CantidadProductos);
        Assert.Equal(0, categorias.Single(c => c.Categoria.Slug == "televisores").CantidadProductos);
    }

    // CA-02
    [Fact]
    public async Task BuscarProductosAsync_PorCategoria_SoloTraeLosDeEsaCategoria()
    {
        _productos.Existente("NEV-300", "Nevera No Frost 300 L", _neveras.Id);
        _productos.Existente("TV-55", "Televisor 55 pulgadas 4K", _televisores.Id);

        Pagina<Producto> pagina = await _servicio.BuscarProductosAsync("neveras", null, 1, 12);

        Assert.Equal("NEV-300", Assert.Single(pagina.Elementos).Sku);
    }

    // CA-03
    [Fact]
    public async Task BuscarProductosAsync_Pagina2DeTamano12Con30Productos_Trae12YElTotalEs30()
    {
        for (int numero = 1; numero <= 30; numero++)
        {
            _productos.Existente($"TV-{numero:00}", $"Televisor {numero:00}", _televisores.Id);
        }

        Pagina<Producto> pagina = await _servicio.BuscarProductosAsync(null, null, 2, 12);

        Assert.Equal(12, pagina.Elementos.Count);
        Assert.Equal(30, pagina.Total);
        Assert.Equal("Televisor 13", pagina.Elementos[0].Nombre);
    }

    // CA-04
    [Fact]
    public async Task BuscarProductosAsync_ConTextoEnMinusculas_EncuentraSinImportarMayusculas()
    {
        _productos.Existente("NEV-300", "Nevera No Frost 300 L", _neveras.Id);
        _productos.Existente("NEV-250", "Nevera 250 L", _neveras.Id);

        Pagina<Producto> pagina = await _servicio.BuscarProductosAsync(null, "no frost", 1, 12);

        Assert.Equal("NEV-300", Assert.Single(pagina.Elementos).Sku);
    }

    // CA-05 · R-08
    [Fact]
    public async Task BuscarProductosAsync_ProductoInactivo_NoAparece()
    {
        _productos.Existente("NEV-100", "Nevera minibar 100 L", _neveras.Id, activo: false);

        Pagina<Producto> pagina = await _servicio.BuscarProductosAsync(null, "nevera", 1, 12);

        Assert.Empty(pagina.Elementos);
        Assert.Equal(0, pagina.Total);
    }

    // CA-06 (bordes: página 0, tamaño 0 y tamaño 51)
    [Theory]
    [InlineData(0, 12)]
    [InlineData(1, 0)]
    [InlineData(1, 51)]
    public async Task BuscarProductosAsync_ConPaginaOTamanoInvalidos_LanzaDatosInvalidos(int pagina, int tamano)
    {
        await Assert.ThrowsAsync<DatosInvalidosException>(() => _servicio.BuscarProductosAsync(null, null, pagina, tamano));
    }

    // CA-06 (borde: tamaño 50 sí sirve)
    [Fact]
    public async Task BuscarProductosAsync_ConTamano50_NoLanza()
    {
        Pagina<Producto> pagina = await _servicio.BuscarProductosAsync(null, null, 1, 50);

        Assert.Equal(50, pagina.Tamano);
    }

    [Fact]
    public async Task BuscarProductosAsync_CategoriaQueNoExiste_LanzaNoEncontrado()
    {
        await Assert.ThrowsAsync<NoEncontradoException>(() => _servicio.BuscarProductosAsync("motos", null, 1, 12));
    }

    // CA-08 · R-08
    [Fact]
    public async Task ObtenerProductoAsync_InactivoOQueNoExiste_LanzaNoEncontrado()
    {
        Producto inactivo = _productos.Existente("NEV-100", "Nevera minibar 100 L", _neveras.Id, activo: false);

        await Assert.ThrowsAsync<NoEncontradoException>(() => _servicio.ObtenerProductoAsync(inactivo.Id));
        await Assert.ThrowsAsync<NoEncontradoException>(() => _servicio.ObtenerProductoAsync(9999));
    }

    // CA-18 · R-10
    [Fact]
    public async Task CambiarPrecioAsync_ConPrecioValido_LoCambiaYGuarda()
    {
        Producto nevera = _productos.Existente("NEV-300", "Nevera No Frost 300 L", _neveras.Id, precio: 2_899_000m);

        await _servicio.CambiarPrecioAsync(nevera.Id, 2_799_000m);

        Assert.Equal(2_799_000m, nevera.PrecioVenta);
        Assert.Equal(1, _unidad.VecesGuardado);
    }

    // CA-20 · R-02
    [Fact]
    public async Task CambiarPrecioAsync_ACero_LanzaDatosInvalidosYNoGuarda()
    {
        Producto nevera = _productos.Existente("NEV-300", "Nevera No Frost 300 L", _neveras.Id, precio: 2_899_000m);

        await Assert.ThrowsAsync<DatosInvalidosException>(() => _servicio.CambiarPrecioAsync(nevera.Id, 0m));

        Assert.Equal(2_899_000m, nevera.PrecioVenta);
        Assert.Equal(0, _unidad.VecesGuardado);
    }
}
