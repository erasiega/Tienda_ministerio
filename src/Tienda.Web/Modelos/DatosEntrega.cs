using System.ComponentModel.DataAnnotations;

namespace Tienda.Web.Modelos;

/// <summary>Datos necesarios para entregar un pedido.</summary>
public sealed class DatosEntrega
{
    /// <summary>Nombre de quien recibe el pedido.</summary>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Documento de quien recibe el pedido.</summary>
    [Required(ErrorMessage = "El documento es obligatorio.")]
    public string Documento { get; set; } = string.Empty;

    /// <summary>Correo de contacto para el pedido.</summary>
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Escribe un correo electrónico válido.")]
    public string Correo { get; set; } = string.Empty;

    /// <summary>Dirección donde se entregará el pedido.</summary>
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = string.Empty;
}
