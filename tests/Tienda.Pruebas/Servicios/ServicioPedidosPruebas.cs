using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;
using Tienda.Core.Servicios;
using Tienda.Pruebas.Falsos;

namespace Tienda.Pruebas.Servicios;

public class ServicioPedidosPruebas
{
    private readonly RepositorioPedidosFalso _pedidos = new();
    private readonly RepositorioProductosFalso _productos = new();
    private readonly UnidadDeTrabajoFalsa _unidad = new();
    private readonly ServicioPedidos _servicio;

    public ServicioPedidosPruebas() =>
        _servicio = new ServicioPedidos(_pedidos, _productos, _unidad,
            new RelojFijo(new DateTimeOffset(2026, 10, 21, 19, 0, 0, TimeSpan.Zero)));

    private Producto Nevera(int stock) => _productos.Existente("NEV-300", "Nevera No Frost 300 L", 1, 2_899_000m, stock);

    private Producto Licuadora(int stock) => _productos.Existente("LIC-001", "Licuadora 10 velocidades", 5, 389_000m, stock);

    private static DatosPedido Pedido(params DatosLineaPedido[] lineas) =>
        new("Laura Gómez", "52123456", "laura@correo.com", "Calle 45 # 12-30, Bogotá", lineas);

    // CA-09 · R-05 · R-07
    [Fact]
    public async Task RegistrarAsync_DosProductosConStock_BajaLosStocksYCalculaElTotal()
    {
        Producto nevera = Nevera(stock: 5);
        Producto licuadora = Licuadora(stock: 10);

        Pedido pedido = await _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 1), new DatosLineaPedido(licuadora.Id, 2)));

        Assert.Equal(3_677_000m, pedido.Total);
        Assert.Equal(4, nevera.Stock);
        Assert.Equal(8, licuadora.Stock);
        Assert.Equal(32, pedido.Codigo.Length);
        Assert.Single(_pedidos.Todos);
        Assert.Equal(1, _unidad.VecesGuardado);
    }

    // CA-09 · R-05 (borde: pedir exactamente el stock que hay)
    [Fact]
    public async Task RegistrarAsync_ExactamenteElStockQueHay_DejaElStockEnCero()
    {
        Producto nevera = Nevera(stock: 2);

        await _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 2)));

        Assert.Equal(0, nevera.Stock);
    }

    // CA-10 · R-04
    [Fact]
    public async Task RegistrarAsync_SinLineas_LanzaDatosInvalidosYNoGuarda()
    {
        await Assert.ThrowsAsync<DatosInvalidosException>(() => _servicio.RegistrarAsync(Pedido()));

        Assert.Equal(0, _unidad.VecesGuardado);
    }

    // CA-10 · R-04
    [Fact]
    public async Task RegistrarAsync_LineaConCantidadCero_LanzaDatosInvalidosYNoCambiaElStock()
    {
        Producto nevera = Nevera(stock: 5);

        await Assert.ThrowsAsync<DatosInvalidosException>(() => _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 0))));

        Assert.Equal(5, nevera.Stock);
    }

    // CA-11 · R-04
    [Fact]
    public async Task RegistrarAsync_MismoProductoEnDosLineas_LanzaDatosInvalidos()
    {
        Producto nevera = Nevera(stock: 5);

        await Assert.ThrowsAsync<DatosInvalidosException>(() =>
            _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 1), new DatosLineaPedido(nevera.Id, 1))));
    }

    // CA-12 · R-05 (la línea sin stock va de última: si el código bajara el stock
    // mientras valida, la licuadora alcanzaría a descontarse y la prueba lo atraparía)
    [Fact]
    public async Task RegistrarAsync_UltimaLineaSinStock_NoCambiaNingunStockNiGuarda()
    {
        Producto nevera = Nevera(stock: 1);
        Producto licuadora = Licuadora(stock: 10);

        var error = await Assert.ThrowsAsync<ReglaDelNegocioException>(() =>
            _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(licuadora.Id, 1), new DatosLineaPedido(nevera.Id, 2))));

        Assert.Equal("R-05", error.Regla);
        Assert.Equal(1, nevera.Stock);
        Assert.Equal(10, licuadora.Stock);
        Assert.Empty(_pedidos.Todos);
        Assert.Equal(0, _unidad.VecesGuardado);
    }

    // CA-13 · R-06
    [Fact]
    public async Task RegistrarAsync_CualquierLinea_TomaElPrecioDelProducto()
    {
        Producto nevera = Nevera(stock: 5);

        Pedido pedido = await _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 1)));

        Assert.Equal(2_899_000m, pedido.Lineas[0].PrecioUnitario);
        Assert.Equal("Nevera No Frost 300 L", pedido.Lineas[0].NombreProducto);
    }

    // CA-14 · R-08
    [Fact]
    public async Task RegistrarAsync_ProductoInactivo_LanzaReglaYNoGuarda()
    {
        Producto inactivo = _productos.Existente("NEV-100", "Nevera minibar 100 L", 1, 899_000m, 4, activo: false);

        var error = await Assert.ThrowsAsync<ReglaDelNegocioException>(() => _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(inactivo.Id, 1))));

        Assert.Equal("R-08", error.Regla);
        Assert.Equal(4, inactivo.Stock);
        Assert.Equal(0, _unidad.VecesGuardado);
    }

    // CA-14
    [Fact]
    public async Task RegistrarAsync_ProductoQueNoExiste_LanzaNoEncontrado()
    {
        await Assert.ThrowsAsync<NoEncontradoException>(() => _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(9999, 1))));
    }

    // CA-15
    [Fact]
    public async Task RegistrarAsync_CorreoInvalido_LanzaDatosInvalidosYNoCambiaElStock()
    {
        Producto nevera = Nevera(stock: 5);
        var datos = new DatosPedido("Laura Gómez", "52123456", "laura-sin-arroba", "Calle 45", [new DatosLineaPedido(nevera.Id, 1)]);

        await Assert.ThrowsAsync<DatosInvalidosException>(() => _servicio.RegistrarAsync(datos));

        Assert.Equal(5, nevera.Stock);
    }

    // CA-16
    [Fact]
    public async Task ObtenerPorCodigoAsync_CodigoDeUnPedido_DevuelveElPedido()
    {
        Producto nevera = Nevera(stock: 5);
        Pedido registrado = await _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 1)));

        Pedido encontrado = await _servicio.ObtenerPorCodigoAsync(registrado.Codigo);

        Assert.Same(registrado, encontrado);
    }

    // CA-17 · R-09
    [Theory]
    [InlineData("1")]
    [InlineData("codigo-que-no-existe")]
    public async Task ObtenerPorCodigoAsync_CodigoQueNoExiste_LanzaNoEncontrado(string codigo)
    {
        Producto nevera = Nevera(stock: 5);
        await _servicio.RegistrarAsync(Pedido(new DatosLineaPedido(nevera.Id, 1)));

        await Assert.ThrowsAsync<NoEncontradoException>(() => _servicio.ObtenerPorCodigoAsync(codigo));
    }
}
