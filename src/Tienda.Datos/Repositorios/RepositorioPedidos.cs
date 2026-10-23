using Microsoft.EntityFrameworkCore;
using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Datos.Repositorios;

/// <summary>Implementación con EF Core de <see cref="IRepositorioPedidos"/>.</summary>
public class RepositorioPedidos : IRepositorioPedidos
{
    private readonly TiendaDbContext _contexto;

    /// <summary>Crea el repositorio.</summary>
    /// <param name="contexto">Contexto de EF Core.</param>
    public RepositorioPedidos(TiendaDbContext contexto) => _contexto = contexto;

    /// <inheritdoc />
    public void Agregar(Pedido pedido) => _contexto.Pedidos.Add(pedido);

    /// <inheritdoc />
    public Task<Pedido?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancelacion = default) =>
        _contexto.Pedidos.Include(pedido => pedido.Lineas).AsNoTracking()
            .FirstOrDefaultAsync(pedido => pedido.Codigo == codigo, cancelacion);
}
