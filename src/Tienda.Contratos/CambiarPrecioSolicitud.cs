namespace Tienda.Contratos;

/// <summary>Solicitud del personal de la tienda para cambiar el precio de un producto (R-10).</summary>
/// <param name="Precio">Precio nuevo en pesos, mayor que cero (R-02).</param>
public record CambiarPrecioSolicitud(decimal Precio);
