using System.Globalization;

namespace Tienda.Web.Servicios;

/// <summary>Da formato a los valores que se muestran en la tienda.</summary>
public static class Formato
{
    /// <summary>Muestra un valor en pesos, sin decimales y con puntos de miles.</summary>
    /// <param name="valor">Valor en pesos.</param>
    /// <returns>El valor con el formato «$ 2.899.000».</returns>
    public static string Pesos(decimal valor) =>
        $"$ {valor.ToString("#,##0", CultureInfo.InvariantCulture).Replace(',', '.')}";
}
