namespace Tienda.Core.Entidades;

/// <summary>Una categoría del catálogo, como «Neveras» o «Televisores».</summary>
public class Categoria
{
    /// <summary>Constructor para EF Core.</summary>
    private Categoria()
    {
    }

    /// <summary>Crea una categoría.</summary>
    /// <param name="nombre">Nombre que ve el visitante.</param>
    /// <param name="slug">Identificador para la dirección web, en minúsculas y sin espacios (por ejemplo «neveras»).</param>
    public Categoria(string nombre, string slug)
    {
        Nombre = Validacion.TextoObligatorio(nombre, "El nombre de la categoría");
        Slug = Validacion.TextoObligatorio(slug, "El slug de la categoría").ToLowerInvariant();
    }

    /// <summary>Identificador.</summary>
    public int Id { get; private set; }

    /// <summary>Nombre que ve el visitante.</summary>
    public string Nombre { get; private set; } = string.Empty;

    /// <summary>Identificador para la dirección web, en minúsculas.</summary>
    public string Slug { get; private set; } = string.Empty;
}
