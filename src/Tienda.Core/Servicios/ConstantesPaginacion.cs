namespace Tienda.Core.Servicios;

/// <summary>Valores compartidos para validar y solicitar páginas de productos.</summary>
public static class ConstantesPaginacion
{
    /// <summary>Número mínimo de página permitido.</summary>
    public const int PaginaMinima = 1;

    /// <summary>Número de página usado cuando no se especifica.</summary>
    public const int PaginaPredeterminada = PaginaMinima;

    /// <summary>Tamaño mínimo de página permitido.</summary>
    public const int TamanoPaginaMinimo = 1;

    /// <summary>Tamaño de página usado cuando no se especifica.</summary>
    public const int TamanoPaginaPredeterminado = 12;

    /// <summary>Tamaño máximo de página permitido.</summary>
    public const int TamanoPaginaMaximo = 50;
}
