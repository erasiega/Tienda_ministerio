using Tienda.Core.Entidades;

namespace Tienda.Core.Repositorios;

/// <summary>Acceso a los pedidos, con sus líneas. El núcleo solo conoce esta interfaz; la implementa <c>Tienda.Datos</c>.</summary>
public interface IRepositorioPedidos
{
    /// <summary>Marca un pedido nuevo para guardarlo con <see cref="IUnidadDeTrabajo"/>.</summary>
    /// <param name="pedido">Pedido nuevo.</param>
    void Agregar(Pedido pedido);

    /// <summary>Busca un pedido por su código (R-09).</summary>
    /// <param name="codigo">Código de 32 caracteres que recibió quien compró.</param>
    /// <param name="cancelacion">Permite cancelar la consulta.</param>
    /// <returns>El pedido con sus líneas, o <c>null</c> si no existe.</returns>
    Task<Pedido?> ObtenerPorCodigoAsync(string codigo, CancellationToken cancelacion = default);
}
