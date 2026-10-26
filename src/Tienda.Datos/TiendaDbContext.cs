using Microsoft.EntityFrameworkCore;
using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Datos;

/// <summary>
/// Contexto de EF Core. También es la unidad de trabajo: un solo <c>SaveChanges</c> guarda
/// todos los cambios pendientes, completos o ninguno (R-05).
/// </summary>
public class TiendaDbContext : DbContext, IUnidadDeTrabajo
{
    /// <summary>Crea el contexto con las opciones configuradas en <see cref="ExtensionesDatos"/>.</summary>
    /// <param name="opciones">Opciones de EF Core, con el motor elegido.</param>
    public TiendaDbContext(DbContextOptions<TiendaDbContext> opciones) : base(opciones)
    {
    }

    /// <summary>Categorías del catálogo.</summary>
    public DbSet<Categoria> Categorias => Set<Categoria>();

    /// <summary>Productos del catálogo.</summary>
    public DbSet<Producto> Productos => Set<Producto>();

    /// <summary>Pedidos de la tienda web, con sus líneas.</summary>
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    /// <inheritdoc />
    public Task GuardarCambiosAsync(CancellationToken cancelacion = default) => SaveChangesAsync(cancelacion);

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TiendaDbContext).Assembly);
}
