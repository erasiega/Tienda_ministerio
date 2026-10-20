namespace Tienda.Core.Excepciones;

/// <summary>
/// Los datos recibidos no cumplen una validación (por ejemplo, un precio en cero).
/// La API la responde con 400.
/// </summary>
public class DatosInvalidosException : Exception
{
    /// <summary>Crea la excepción con el mensaje para el usuario.</summary>
    /// <param name="mensaje">Qué dato está mal y por qué.</param>
    public DatosInvalidosException(string mensaje) : base(mensaje)
    {
    }
}
