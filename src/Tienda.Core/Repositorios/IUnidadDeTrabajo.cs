namespace Tienda.Core.Repositorios;

/// <summary>
/// Guarda en una sola operación todos los cambios pendientes de los repositorios.
/// Así un pedido con varias líneas se guarda completo o no se guarda (R-05).
/// </summary>
public interface IUnidadDeTrabajo
{
    /// <summary>Guarda todos los cambios pendientes.</summary>
    /// <param name="cancelacion">Permite cancelar la operación.</param>
    Task GuardarCambiosAsync(CancellationToken cancelacion = default);
}
