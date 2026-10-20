namespace Tienda.Contratos;

/// <summary>Una página de resultados.</summary>
/// <typeparam name="T">Tipo de los elementos.</typeparam>
/// <param name="Elementos">Elementos de esta página.</param>
/// <param name="Total">Total de elementos en todas las páginas.</param>
/// <param name="Pagina">Número de la página, desde 1.</param>
/// <param name="Tamano">Elementos por página.</param>
/// <param name="TotalPaginas">Cantidad de páginas.</param>
public record PaginaRespuesta<T>(IReadOnlyList<T> Elementos, int Total, int Pagina, int Tamano, int TotalPaginas);
