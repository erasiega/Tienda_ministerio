using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tienda.Core.Excepciones;

namespace Tienda.Api.Errores;

/// <summary>
/// Traduce las excepciones a respuestas <see cref="ProblemDetails"/> con el código HTTP correcto:
/// 400 datos inválidos, 404 no existe, 409 regla del negocio violada y 500 lo inesperado.
/// Un error inesperado nunca muestra detalles internos: esos van al registro (V-03).
/// </summary>
public class ManejadorDeErrores : IExceptionHandler
{
    private const string MensajeErrorInesperado = "Ocurrió un error inesperado.";

    private readonly ILogger<ManejadorDeErrores> _registro;

    /// <summary>Crea el manejador.</summary>
    /// <param name="registro">Registro donde se guarda el detalle de los errores inesperados.</param>
    public ManejadorDeErrores(ILogger<ManejadorDeErrores> registro) => _registro = registro;

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(HttpContext contexto, Exception excepcion, CancellationToken cancelacion)
    {
        (int estado, string titulo) = excepcion switch
        {
            DatosInvalidosException => (StatusCodes.Status400BadRequest, "Datos inválidos"),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, "Solicitud inválida"),
            NoEncontradoException => (StatusCodes.Status404NotFound, "No encontrado"),
            ReglaDelNegocioException => (StatusCodes.Status409Conflict, "Regla del negocio"),
            _ => (StatusCodes.Status500InternalServerError, "Error interno"),
        };

        string detalle = excepcion.Message;
        if (estado == StatusCodes.Status500InternalServerError)
        {
            _registro.LogError(excepcion, "Error inesperado en {Metodo} {Ruta}", contexto.Request.Method, contexto.Request.Path);
            detalle = MensajeErrorInesperado;
        }

        var problema = new ProblemDetails { Status = estado, Title = titulo, Detail = detalle };
        if (excepcion is ReglaDelNegocioException regla)
        {
            problema.Extensions["regla"] = regla.Regla;
        }

        contexto.Response.StatusCode = estado;
        await contexto.Response.WriteAsJsonAsync(problema, cancelacion);
        return true;
    }
}
