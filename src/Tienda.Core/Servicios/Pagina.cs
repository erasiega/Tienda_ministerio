namespace Tienda.Core.Servicios;

/// <summary>Una página de resultados.</summary>
/// <typeparam name="T">Tipo de los elementos.</typeparam>
/// <param name="Elementos">Elementos de esta página.</param>
/// <param name="Total">Total de elementos que cumplen el filtro, en todas las páginas.</param>
/// <param name="NumeroPagina">Número de la página, desde 1.</param>
/// <param name="Tamano">Cantidad máxima de elementos por página.</param>
public record Pagina<T>(IReadOnlyList<T> Elementos, int Total, int NumeroPagina, int Tamano);
