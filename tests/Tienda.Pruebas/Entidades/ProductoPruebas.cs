using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;

namespace Tienda.Pruebas.Entidades;

public class ProductoPruebas
{
    private static Producto Nevera(int stock = 5) =>
        new("NEV-300", "Nevera No Frost 300 L", "Nevera de dos puertas", 1, 2_899_000m, 2_150_000m, stock, "/imagenes/nev-300.jpg");

    [Fact]
    public void Crear_ConDatosValidos_QuedaActivoYConElSkuEnMayusculas()
    {
        var producto = new Producto(" nev-300 ", "Nevera No Frost 300 L", "", 1, 2_899_000m, 2_150_000m, 5, "");

        Assert.True(producto.Activo);
        Assert.Equal("NEV-300", producto.Sku);
    }

    // CA-20 · R-02
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Crear_ConPrecioCeroONegativo_LanzaDatosInvalidos(decimal precio)
    {
        Action crear = () => new Producto("NEV-300", "Nevera", "", 1, precio, 2_150_000m, 5, "");

        Assert.Throws<DatosInvalidosException>(crear);
    }

    // R-03
    [Fact]
    public void Crear_ConStockNegativo_LanzaDatosInvalidos()
    {
        Action crear = () => new Producto("NEV-300", "Nevera", "", 1, 2_899_000m, 2_150_000m, -1, "");

        Assert.Throws<DatosInvalidosException>(crear);
    }

    // CA-18 · R-02
    [Fact]
    public void CambiarPrecio_ConPrecioValido_LoCambia()
    {
        Producto producto = Nevera();

        producto.CambiarPrecio(2_799_000m);

        Assert.Equal(2_799_000m, producto.PrecioVenta);
    }

    // CA-20 · R-02 (borde: cero no sirve y el precio no cambia)
    [Fact]
    public void CambiarPrecio_ACero_LanzaDatosInvalidosYNoCambiaElPrecio()
    {
        Producto producto = Nevera();

        Assert.Throws<DatosInvalidosException>(() => producto.CambiarPrecio(0m));

        Assert.Equal(2_899_000m, producto.PrecioVenta);
    }

    // CA-09 · R-03 (borde: pedir exactamente lo que hay)
    [Fact]
    public void DisminuirStock_ExactamenteLoQueHay_DejaElStockEnCero()
    {
        Producto producto = Nevera(stock: 2);

        producto.DisminuirStock(2);

        Assert.Equal(0, producto.Stock);
        Assert.False(producto.Disponible);
    }

    // CA-12 · R-03 (borde: una unidad más de lo que hay)
    [Fact]
    public void DisminuirStock_UnaUnidadMasDeLoQueHay_LanzaReglaYNoCambiaElStock()
    {
        Producto producto = Nevera(stock: 1);

        var error = Assert.Throws<ReglaDelNegocioException>(() => producto.DisminuirStock(2));

        Assert.Equal("R-03", error.Regla);
        Assert.Equal(1, producto.Stock);
    }

    // CA-10 · R-04
    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void DisminuirStock_ConCantidadCeroONegativa_LanzaDatosInvalidos(int cantidad)
    {
        Producto producto = Nevera();

        Assert.Throws<DatosInvalidosException>(() => producto.DisminuirStock(cantidad));
        Assert.Equal(5, producto.Stock);
    }

    // CA-05 · R-08
    [Fact]
    public void Desactivar_ProductoActivo_QuedaInactivoYConservaElStock()
    {
        Producto producto = Nevera(stock: 5);

        producto.Desactivar();

        Assert.False(producto.Activo);
        Assert.Equal(5, producto.Stock);
    }
}
