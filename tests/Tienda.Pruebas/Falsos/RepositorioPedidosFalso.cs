using Tienda.Core.Entidades;
using Tienda.Core.Repositorios;

namespace Tienda.Pruebas.Falsos;

/// <summary>Repositorio de pedidos en memoria.</summary>
internal sealed class RepositorioPedidosFalso : IRepositorioPedidos
{
    private readonly List<Pedido> _pedidos = [];

    public IReadOnlyList<Pedido> Todos => _pedidos;

    public void Agregar(Pedido pedido)
    {
        Ids.Asignar(pedido, _pedidos.Count + 1);
        _pedidos.Add(pedido);
    }

    public Task<Pedido?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancelacion = default) =>
        Task.FromResult(_pedidos.FirstOrDefault(pedido => pedido.Codigo == codigo));
}
