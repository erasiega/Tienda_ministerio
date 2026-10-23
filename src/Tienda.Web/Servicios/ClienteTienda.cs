using System.Net.Http.Json;
using System.Text.Json;
using Tienda.Contratos;

namespace Tienda.Web.Servicios;

/// <summary>Permite al cliente web consultar la API de la tienda.</summary>
public sealed class ClienteTienda
{
    private const string MensajeErrorCategorias = "No fue posible cargar las categorías.";

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
                throw new ErrorApiException(await ObtenerMensajeErrorAsync(respuesta));
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
            throw new ErrorApiException("No se pudo conectar con el servidor de la tienda.", excepcion);
        }
    }

    private static async Task<string> ObtenerMensajeErrorAsync(HttpResponseMessage respuesta)
    {
        string contenido = await respuesta.Content.ReadAsStringAsync();
        string mensajeGenerico = $"{MensajeErrorCategorias} (código HTTP {(int)respuesta.StatusCode}).";
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
