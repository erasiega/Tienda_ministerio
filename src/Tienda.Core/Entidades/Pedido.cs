using Tienda.Core.Excepciones;

namespace Tienda.Core.Entidades;

/// <summary>
/// Un pedido de la tienda web, con los datos de quien compra. Se crea completo y válido:
/// al menos una línea, sin productos repetidos (R-04), y con un código que no se puede adivinar (R-09).
/// </summary>
public class Pedido
{
    private readonly List<LineaPedido> _lineas = [];

    /// <summary>Constructor para EF Core.</summary>
    private Pedido()
    {
    }

    /// <summary>Crea un pedido.</summary>
    /// <param name="nombreCliente">Nombre de quien compra.</param>
    /// <param name="documentoCliente">Documento de quien compra.</param>
    /// <param name="correoCliente">Correo de quien compra.</param>
    /// <param name="direccionEntrega">Dirección de entrega.</param>
    /// <param name="fecha">Fecha y hora del pedido; la pone el servidor.</param>
    /// <param name="lineas">Productos, cantidades y precios (R-04).</param>
    public Pedido(string nombreCliente, string documentoCliente, string correoCliente,
        string direccionEntrega, DateTime fecha, IEnumerable<LineaPedido> lineas)
    {
        NombreCliente = Validacion.TextoObligatorio(nombreCliente, "El nombre");
        DocumentoCliente = Validacion.TextoObligatorio(documentoCliente, "El documento");
        CorreoCliente = Validacion.Correo(correoCliente);
        DireccionEntrega = Validacion.TextoObligatorio(direccionEntrega, "La dirección de entrega");
        _lineas.AddRange(lineas);
        if (_lineas.Count == 0)
        {
            throw new DatosInvalidosException("Un pedido debe tener al menos una línea (R-04).");
        }
        if (_lineas.Select(linea => linea.ProductoId).Distinct().Count() != _lineas.Count)
        {
            throw new DatosInvalidosException("Un pedido no puede tener el mismo producto en dos líneas (R-04).");
        }
        Fecha = fecha;
        Codigo = Guid.NewGuid().ToString("N");
    }

    /// <summary>Identificador interno. No se usa para consultar el pedido desde afuera (R-09).</summary>
    public int Id { get; private set; }

    /// <summary>Código aleatorio de 32 caracteres con el que quien compra consulta su pedido (R-09).</summary>
    public string Codigo { get; private set; } = string.Empty;

    /// <summary>Fecha y hora del pedido.</summary>
    public DateTime Fecha { get; private set; }

    /// <summary>Nombre de quien compra.</summary>
    public string NombreCliente { get; private set; } = string.Empty;

    /// <summary>Documento de quien compra.</summary>
    public string DocumentoCliente { get; private set; } = string.Empty;

    /// <summary>Correo de quien compra.</summary>
    public string CorreoCliente { get; private set; } = string.Empty;

    /// <summary>Dirección de entrega.</summary>
    public string DireccionEntrega { get; private set; } = string.Empty;

    /// <summary>Productos del pedido.</summary>
    public IReadOnlyList<LineaPedido> Lineas => _lineas;

    /// <summary>Suma de los subtotales; la calcula el servidor (R-07).</summary>
    public decimal Total => _lineas.Sum(linea => linea.Subtotal);
}
