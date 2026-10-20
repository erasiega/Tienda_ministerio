namespace Tienda.Core.Excepciones;

/// <summary>El recurso pedido no existe. La API la responde con 404.</summary>
public class NoEncontradoException : Exception
{
    /// <summary>Crea la excepción con el mensaje para el usuario.</summary>
    /// <param name="mensaje">Qué no se encontró.</param>
    public NoEncontradoException(string mensaje) : base(mensaje)
    {
    }
}
