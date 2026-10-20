using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tienda.Core.Excepciones;

namespace Tienda.Api.Errores;

/// <summary>
/// Traduce las excepciones a respuestas <see cref="ProblemDetails"/> con el código HTTP correcto:
/// 400 datos inválidos, 404 no existe, 409 regla del negocio violada.
/// </summary>
public class ManejadorDeErrores : IExceptionHandler
{
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

        var problema = new ProblemDetails
        {
            Status = estado,
            Title = titulo,
            Detail = estado == StatusCodes.Status500InternalServerError ? excepcion.ToString() : excepcion.Message,
        };
        if (excepcion is ReglaDelNegocioException regla)
        {
            problema.Extensions["regla"] = regla.Regla;
        }

        contexto.Response.StatusCode = estado;
        await contexto.Response.WriteAsJsonAsync(problema, cancelacion);
        return true;
    }
}
