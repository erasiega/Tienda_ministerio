using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Pruebas.Falsos;

/// <summary>Repositorio de productos en memoria: hace con una lista lo que en la aplicación hace SQLite.</summary>
internal sealed class RepositorioProductosFalso : IRepositorioProductos
{
    private readonly List<Producto> _productos = [];

    public Producto Existente(string sku, string nombre, int categoriaId, decimal precio = 1_000_000m, int stock = 5, bool activo = true)
    {
        var producto = new Producto(sku, nombre, "", categoriaId, precio, precio / 2, stock, "");
        Ids.Asignar(producto, _productos.Count + 1);
        if (!activo)
        {
            producto.Desactivar();
        }
        _productos.Add(producto);
        return producto;
    }

    public Task<Producto?> ObtenerPorIdAsync(int id, CancellationToken cancelacion = default) =>
        Task.FromResult(_productos.FirstOrDefault(producto => producto.Id == id));

    public Task<(IReadOnlyList<Producto> Productos, int Total)> BuscarActivosAsync(int? categoriaId, string? texto,
        int saltar, int tomar, CancellationToken cancelacion = default)
    {
        List<Producto> filtrados = _productos
            .Where(producto => producto.Activo)
            .Where(producto => categoriaId is null || producto.CategoriaId == categoriaId)
            .Where(producto => texto is null || producto.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .OrderBy(producto => producto.Nombre)
            .ToList();
        IReadOnlyList<Producto> pagina = filtrados.Skip(saltar).Take(tomar).ToList();
        return Task.FromResult((pagina, filtrados.Count));
    }

    public Task<IReadOnlyDictionary<int, int>> ContarActivosPorCategoriaAsync(CancellationToken cancelacion = default) =>
        Task.FromResult<IReadOnlyDictionary<int, int>>(_productos
            .Where(producto => producto.Activo)
            .GroupBy(producto => producto.CategoriaId)
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Count()));
}
