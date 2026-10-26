using System.Net.Http.Json;
using System.Text.Json;
using Tienda.Contratos;

namespace Tienda.Web.Servicios;

/// <summary>Permite al cliente web consultar la API de la tienda.</summary>
public sealed class ClienteTienda
{
    private const string MensajeErrorCategorias = "No fue posible cargar las categorías.";
    private const string MensajeErrorProductos = "No fue posible cargar los productos.";
    private const string MensajeErrorDetalleProducto = "No fue posible cargar el producto.";
    private const string MensajeErrorCrearPedido = "No fue posible hacer el pedido.";
    private const string MensajeErrorObtenerPedido = "No fue posible consultar el pedido.";
    private const string MensajeSinConexion =
        "No se pudo conectar con la tienda. Revisa que la API esté corriendo.";

    private readonly HttpClient _httpClient;

    /// <summary>Crea el cliente con el HTTP configurado para la API.</summary>
    /// <param name="httpClient">Cliente HTTP con la dirección base de la API.</param>
    public ClienteTienda(HttpClient httpClient) => _httpClient = httpClient;

    /// <summary>Obtiene las categorías disponibles en la tienda.</summary>
    /// <returns>Las categorías y la cantidad de productos activos de cada una.</returns>
    /// <exception cref="ErrorApiException">La API no pudo atender o devolver la consulta.</exception>
    public async Task<IReadOnlyList<CategoriaRespuesta>> ObtenerCategoriasAsync()
    {
        try
        {
            using HttpResponseMessage respuesta = await _httpClient.GetAsync("api/categorias");
            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta, MensajeErrorCategorias));
            }

            try
            {
                List<CategoriaRespuesta>? categorias =
                    await respuesta.Content.ReadFromJsonAsync<List<CategoriaRespuesta>>();
                return categorias
                    ?? throw new ErrorApiException("La API devolvió una respuesta vacía para las categorías.");
            }
            catch (JsonException excepcion)
            {
                throw new ErrorApiException("La respuesta de categorías no tiene un formato válido.", excepcion);
            }
        }
        catch (HttpRequestException excepcion)
        {
            throw new ErrorApiException(MensajeSinConexion, excepcion);
        }
    }

    /// <summary>Busca productos activos por categoría y nombre en una página.</summary>
    /// <param name="categoria">Slug de la categoría, o <see langword="null"/> para todas.</param>
    /// <param name="buscar">Texto del nombre que se desea encontrar, o <see langword="null"/>.</param>
    /// <param name="pagina">Número de página, desde 1.</param>
    /// <param name="tamano">Cantidad de productos por página.</param>
    /// <returns>Los productos y la información de paginación.</returns>
    /// <exception cref="ErrorApiException">La API no pudo atender o devolver la consulta.</exception>
    public async Task<PaginaRespuesta<ProductoResumenRespuesta>> ObtenerProductosAsync(
        string? categoria,
        string? buscar,
        int pagina,
        int tamano)
    {
        var parametros = new List<string>();
        if (!string.IsNullOrWhiteSpace(categoria))
        {
            parametros.Add($"categoria={Uri.EscapeDataString(categoria)}");
        }

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            parametros.Add($"buscar={Uri.EscapeDataString(buscar)}");
        }

        parametros.Add($"pagina={pagina}");
        parametros.Add($"tamano={tamano}");

        try
        {
            using HttpResponseMessage respuesta =
                await _httpClient.GetAsync($"api/productos?{string.Join("&", parametros)}");
            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta, MensajeErrorProductos));
            }

            try
            {
                PaginaRespuesta<ProductoResumenRespuesta>? productos =
                    await respuesta.Content.ReadFromJsonAsync<PaginaRespuesta<ProductoResumenRespuesta>>();
                return productos
                    ?? throw new ErrorApiException("La API devolvió una respuesta vacía para los productos.");
            }
            catch (JsonException excepcion)
            {
                throw new ErrorApiException("La respuesta de productos no tiene un formato válido.", excepcion);
            }
        }
        catch (HttpRequestException excepcion)
        {
            throw new ErrorApiException(MensajeSinConexion, excepcion);
        }
    }

    /// <summary>Obtiene el detalle de un producto activo.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>El detalle del producto.</returns>
    /// <exception cref="ErrorApiException">La API no pudo atender o devolver la consulta.</exception>
    public async Task<ProductoDetalleRespuesta> ObtenerProductoAsync(int id)
    {
        try
        {
            using HttpResponseMessage respuesta = await _httpClient.GetAsync($"api/productos/{id}");
            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta, MensajeErrorDetalleProducto));
            }

            try
            {
                return await respuesta.Content.ReadFromJsonAsync<ProductoDetalleRespuesta>()
                    ?? throw new ErrorApiException("La API devolvió una respuesta vacía para el producto.");
            }
            catch (JsonException excepcion)
            {
                throw new ErrorApiException("La respuesta del producto no tiene un formato válido.", excepcion);
            }
        }
        catch (HttpRequestException excepcion)
        {
            throw new ErrorApiException(MensajeSinConexion, excepcion);
        }
    }

    /// <summary>Registra un pedido usando el precio y el total calculados por el servidor.</summary>
    /// <param name="solicitud">Datos de entrega y líneas del pedido.</param>
    /// <returns>El pedido registrado por el servidor.</returns>
    /// <exception cref="ErrorApiException">La API no pudo atender o devolver el pedido.</exception>
    public async Task<PedidoRespuesta> CrearPedidoAsync(CrearPedidoSolicitud solicitud)
    {
        try
        {
            using HttpResponseMessage respuesta = await _httpClient.PostAsJsonAsync("api/pedidos", solicitud);
            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta, MensajeErrorCrearPedido));
            }

            try
            {
                return await respuesta.Content.ReadFromJsonAsync<PedidoRespuesta>()
                    ?? throw new ErrorApiException("La API devolvió una respuesta vacía para el pedido.");
            }
            catch (JsonException excepcion)
            {
                throw new ErrorApiException("La respuesta del pedido no tiene un formato válido.", excepcion);
            }
        }
        catch (HttpRequestException excepcion)
        {
            throw new ErrorApiException(MensajeSinConexion, excepcion);
        }
    }

    /// <summary>Consulta un pedido por su código.</summary>
    /// <param name="codigo">Código recibido al hacer el pedido.</param>
    /// <returns>El pedido con los valores calculados por el servidor.</returns>
    /// <exception cref="ErrorApiException">La API no pudo atender o devolver el pedido.</exception>
    public async Task<PedidoRespuesta> ObtenerPedidoAsync(string codigo)
    {
        try
        {
            using HttpResponseMessage respuesta =
                await _httpClient.GetAsync($"api/pedidos/{Uri.EscapeDataString(codigo)}");
            if (!respuesta.IsSuccessStatusCode)
            {
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta, MensajeErrorObtenerPedido));
            }

            try
            {
                return await respuesta.Content.ReadFromJsonAsync<PedidoRespuesta>()
                    ?? throw new ErrorApiException("La API devolvió una respuesta vacía para el pedido.");
            }
            catch (JsonException excepcion)
            {
                throw new ErrorApiException("La respuesta del pedido no tiene un formato válido.", excepcion);
            }
        }
        catch (HttpRequestException excepcion)
        {
            throw new ErrorApiException(MensajeSinConexion, excepcion);
        }
    }

    private static async Task<string> ObtenerMensajeErrorAsync(
        HttpResponseMessage respuesta,
        string mensajePredeterminado)
    {
        string contenido = await respuesta.Content.ReadAsStringAsync();
        string mensajeGenerico = $"{mensajePredeterminado} (código HTTP {(int)respuesta.StatusCode}).";
        try
        {
            using JsonDocument documento = JsonDocument.Parse(contenido);
            if (documento.RootElement.TryGetProperty("detail", out JsonElement detalle)
                && detalle.ValueKind == JsonValueKind.String
                && detalle.GetString() is { } mensaje
                && !string.IsNullOrWhiteSpace(mensaje))
            {
                return mensaje;
            }
        }
        catch (JsonException)
        {
            return mensajeGenerico;
        }

        return mensajeGenerico;
    }
}
