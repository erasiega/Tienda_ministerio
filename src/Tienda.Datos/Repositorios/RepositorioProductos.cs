using Microsoft.EntityFrameworkCore;
using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Datos.Repositorios;

/// <summary>Implementación con EF Core de <see cref="IRepositorioProductos"/>.</summary>
public class RepositorioProductos : IRepositorioProductos
{
    private readonly TiendaDbContext _contexto;

    /// <summary>Crea el repositorio.</summary>
    /// <param name="contexto">Contexto de EF Core.</param>
    public RepositorioProductos(TiendaDbContext contexto) => _contexto = contexto;

    /// <inheritdoc />
    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancelacion = default) =>
        _contexto.Productos.Include(producto => producto.Categoria).FirstOrDefaultAsync(producto => producto.Id == id, cancelacion);

    /// <inheritdoc />
    public async Task<(IReadOnlyList<Producto> Productos, int Total)> BuscarActivosAsync(int? categoriaId, string? texto,
        int saltar, int tomar, CancellationToken cancelacion = default)
    {
        // LIKE de SQLite no distingue mayúsculas, así la búsqueda cumple CA-04.
        IQueryable<Producto> consulta = _contexto.Productos.Where(producto =>
            producto.Activo && (texto == null || EF.Functions.Like(producto.Nombre, $"%{texto}%")));

        if (categoriaId is not null)
        {
            consulta = consulta.Where(producto => producto.CategoriaId == categoriaId);
        }

        int total = await consulta.CountAsync(cancelacion);
        List<Producto> productos = await consulta
            .Include(producto => producto.Categoria)
            .OrderBy(producto => producto.Nombre)
            .Skip(saltar)
            .Take(tomar)
            .AsNoTracking()
            .ToListAsync(cancelacion);
        return (productos, total);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyDictionary<int, int>> ContarActivosPorCategoriaAsync(CancellationToken cancelacion = default) =>
        await _contexto.Productos
            .Where(producto => producto.Activo)
            .GroupBy(producto => producto.CategoriaId)
            .ToDictionaryAsync(grupo => grupo.Key, grupo => grupo.Count(), cancelacion);
}
