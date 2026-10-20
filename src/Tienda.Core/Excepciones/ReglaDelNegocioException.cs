namespace Tienda.Core.Excepciones;

/// <summary>
/// La operación viola una regla del negocio del enunciado (por ejemplo, pedir más de lo que hay en stock).
/// La API la responde con 409.
/// </summary>
public class ReglaDelNegocioException : Exception
{
    /// <summary>Crea la excepción con el mensaje y la regla violada.</summary>
    /// <param name="mensaje">Qué pasó, en palabras del usuario.</param>
    /// <param name="regla">ID de la regla del enunciado, por ejemplo «R-05».</param>
    public ReglaDelNegocioException(string mensaje, string regla) : base(mensaje)
    {
        Regla = regla;
    }

    /// <summary>ID de la regla del enunciado que se violó.</summary>
    public string Regla { get; }
}
