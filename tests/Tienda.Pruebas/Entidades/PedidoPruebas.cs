using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;

namespace Tienda.Pruebas.Entidades;

public class PedidoPruebas
{
    private static readonly DateTime Fecha = new(2026, 10, 21, 10, 0, 0);

    private static Pedido CrearPedido(params LineaPedido[] lineas) =>
        new("Laura Gómez", "52123456", "laura@correo.com", "Calle 45 # 12-30, Bogotá", Fecha, lineas);

    private static LineaPedido Nevera(int cantidad = 1) => new(1, "Nevera No Frost 300 L", cantidad, 2_899_000m);

    private static LineaPedido Licuadora(int cantidad = 2) => new(2, "Licuadora 10 velocidades", cantidad, 389_000m);

    // CA-09 · R-07
    [Fact]
    public void Total_UnaNeveraYDosLicuadoras_Es3677000()
    {
        Pedido pedido = CrearPedido(Nevera(1), Licuadora(2));

        Assert.Equal(3_677_000m, pedido.Total);
    }

    // CA-10 · R-04
    [Fact]
    public void Crear_SinLineas_LanzaDatosInvalidos()
    {
        Assert.Throws<DatosInvalidosException>(() => CrearPedido());
    }

    // CA-11 · R-04
    [Fact]
    public void Crear_ConElMismoProductoEnDosLineas_LanzaDatosInvalidos()
    {
        Assert.Throws<DatosInvalidosException>(() => CrearPedido(Nevera(1), Nevera(2)));
    }

    // CA-15
    [Theory]
    [InlineData("", "52123456", "laura@correo.com", "Calle 45")]
    [InlineData("Laura", "", "laura@correo.com", "Calle 45")]
    [InlineData("Laura", "52123456", "laura@correo.com", " ")]
    [InlineData("Laura", "52123456", "laura.correo.com", "Calle 45")]
    [InlineData("Laura", "52123456", "laura@correo", "Calle 45")]
    public void Crear_ConDatosDelClienteInvalidos_LanzaDatosInvalidos(string nombre, string documento, string correo, string direccion)
    {
        Assert.Throws<DatosInvalidosException>(() => new Pedido(nombre, documento, correo, direccion, Fecha, [Nevera()]));
    }

    // R-09
    [Fact]
    public void Crear_DosPedidos_TienenCodigosDistintosDe32Caracteres()
    {
        Pedido primero = CrearPedido(Nevera());
        Pedido segundo = CrearPedido(Nevera());

        Assert.Equal(32, primero.Codigo.Length);
        Assert.NotEqual(primero.Codigo, segundo.Codigo);
    }
}
