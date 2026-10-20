using Microsoft.EntityFrameworkCore;
using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Datos.Repositorios;

/// <summary>Implementación con EF Core de <see cref="IRepositorioCategorias"/>.</summary>
public class RepositorioCategorias : IRepositorioCategorias
{
    private readonly TiendaDbContext _contexto;

    /// <summary>Crea el repositorio.</summary>
    /// <param name="contexto">Contexto de EF Core.</param>
    public RepositorioCategorias(TiendaDbContext contexto) => _contexto = contexto;

    /// <inheritdoc />
    public async Task<IReadOnlyList<Categoria>> ListarAsync(CancellationToken cancelacion = default) =>
        await _contexto.Categorias.AsNoTracking().OrderBy(categoria => categoria.Nombre).ToListAsync(cancelacion);

    /// <inheritdoc />
    public Task<Categoria?> ObtenerPorSlugAsync(string slug, CancellationToken cancelacion = default) =>
        _contexto.Categorias.AsNoTracking().FirstOrDefaultAsync(categoria => categoria.Slug == slug, cancelacion);
}
