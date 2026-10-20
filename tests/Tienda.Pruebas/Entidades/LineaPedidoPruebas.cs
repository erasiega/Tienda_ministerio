using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;

namespace Tienda.Pruebas.Entidades;

public class LineaPedidoPruebas
{
    // CA-10 · R-04
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Crear_ConCantidadCeroONegativa_LanzaDatosInvalidos(int cantidad)
    {
        Assert.Throws<DatosInvalidosException>(() => new LineaPedido(1, "Nevera", cantidad, 2_899_000m));
    }

    // R-07
    [Fact]
    public void Subtotal_DosLicuadoras_EsCantidadPorPrecio()
    {
        var linea = new LineaPedido(2, "Licuadora 10 velocidades", 2, 389_000m);

        Assert.Equal(778_000m, linea.Subtotal);
    }
}
