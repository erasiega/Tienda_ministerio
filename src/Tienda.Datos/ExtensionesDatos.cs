using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tienda.Core.Repositorios;
using Tienda.Datos.DatosIniciales;
using Tienda.Datos.Repositorios;

namespace Tienda.Datos;

/// <summary>
/// Registro de la capa de datos. Es el único lugar donde se elige el motor de base de datos (C-03).
/// </summary>
public static class ExtensionesDatos
{
    /// <summary>Registra el contexto, la unidad de trabajo y los repositorios.</summary>
    /// <param name="servicios">Contenedor de servicios de la aplicación.</param>
    /// <param name="motor">Motor configurado en appsettings.json, por ejemplo «Sqlite».</param>
    /// <param name="conexion">Cadena de conexión.</param>
    /// <returns>El mismo contenedor, para encadenar llamadas.</returns>
    /// <exception cref="InvalidOperationException">Si el motor no está soportado o falta la conexión.</exception>
    public static IServiceCollection AgregarDatos(this IServiceCollection servicios, string? motor, string? conexion)
    {
        if (string.IsNullOrWhiteSpace(conexion))
        {
            throw new InvalidOperationException("Falta la cadena de conexión en BaseDeDatos:Conexion.");
        }
        if (!Enum.TryParse(motor, ignoreCase: true, out MotorBaseDeDatos motorElegido))
        {
            throw new InvalidOperationException($"Motor de base de datos no soportado: «{motor}».");
        }

        servicios.AddDbContext<TiendaDbContext>(opciones =>
        {
            switch (motorElegido)
            {
                case MotorBaseDeDatos.Sqlite:
                    opciones.UseSqlite(conexion);
                    break;
            }
        });

        servicios.AddScoped<IUnidadDeTrabajo>(proveedor => proveedor.GetRequiredService<TiendaDbContext>());
        servicios.AddScoped<IRepositorioCategorias, RepositorioCategorias>();
        servicios.AddScoped<IRepositorioProductos, RepositorioProductos>();
        return servicios;
    }

    /// <summary>
    /// Crea la base de datos con sus tablas si no existe y carga los datos iniciales la primera vez.
    /// Cuando se usen migraciones, este método se reemplaza por aplicarlas.
    /// </summary>
    /// <param name="servicios">Proveedor de servicios de la aplicación ya construida.</param>
    public static async Task CrearBaseDeDatosAsync(this IServiceProvider servicios)
    {
        using IServiceScope alcance = servicios.CreateScope();
        TiendaDbContext contexto = alcance.ServiceProvider.GetRequiredService<TiendaDbContext>();
        await contexto.Database.EnsureCreatedAsync();
        await CargadorDatosIniciales.CargarSiEstaVaciaAsync(contexto);
    }
}
