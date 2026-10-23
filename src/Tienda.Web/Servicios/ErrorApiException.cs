namespace Tienda.Web.Servicios;

/// <summary>Indica un error de la API con un mensaje apto para mostrar al visitante.</summary>
public sealed class ErrorApiException : Exception
{
    /// <summary>Crea la excepción con el mensaje que se mostrará en la página.</summary>
    /// <param name="message">Mensaje legible para el visitante.</param>
    /// <param name="innerException">Error original, si lo hay.</param>
    public ErrorApiException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
