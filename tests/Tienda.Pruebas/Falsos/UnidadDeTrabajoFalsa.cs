using Tienda.Core.Repositorios;

namespace Tienda.Pruebas.Falsos;

/// <summary>Cuenta cuántas veces se guardó, para comprobar que una operación fallida no guarda nada.</summary>
internal sealed class UnidadDeTrabajoFalsa : IUnidadDeTrabajo
{
    public int VecesGuardado { get; private set; }

    public Task GuardarCambiosAsync(CancellationToken cancelacion = default)
    {
        VecesGuardado++;
        return Task.CompletedTask;
    }
}
