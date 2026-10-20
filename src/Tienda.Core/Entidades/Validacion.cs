using Tienda.Core.Excepciones;

namespace Tienda.Core.Entidades;

/// <summary>Validaciones que comparten las entidades. Cada una lanza <see cref="DatosInvalidosException"/>.</summary>
internal static class Validacion
{
    /// <summary>Exige un texto con contenido y lo devuelve sin espacios a los lados.</summary>
    public static string TextoObligatorio(string? valor, string nombreDelCampo)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DatosInvalidosException($"{nombreDelCampo} es obligatorio.");
        }
        return valor.Trim();
    }

    /// <summary>Exige una cantidad mayor que cero.</summary>
    public static void CantidadPositiva(int cantidad, string regla)
    {
        if (cantidad <= 0)
        {
            throw new DatosInvalidosException($"La cantidad debe ser mayor que cero ({regla}).");
        }
    }

    /// <summary>Exige un valor en pesos mayor que cero.</summary>
    public static void ValorPositivo(decimal valor, string nombreDelCampo, string regla)
    {
        if (valor <= 0)
        {
            throw new DatosInvalidosException($"{nombreDelCampo} debe ser mayor que cero ({regla}).");
        }
    }

    /// <summary>Exige un correo con forma de correo: algo, una arroba, un dominio con punto.</summary>
    public static string Correo(string? valor)
    {
        string correo = TextoObligatorio(valor, "El correo");
        int arroba = correo.IndexOf('@');
        if (arroba <= 0 || arroba != correo.LastIndexOf('@') || !correo[(arroba + 1)..].Contains('.') || correo.EndsWith('.'))
        {
            throw new DatosInvalidosException("El correo no es válido.");
        }
        return correo;
    }
}
