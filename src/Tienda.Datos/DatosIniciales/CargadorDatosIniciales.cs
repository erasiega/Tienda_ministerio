using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Tienda.Core.Entidades;

namespace Tienda.Datos.DatosIniciales;

/// <summary>Carga las categorías y los productos de ejemplo la primera vez que se crea la base de datos.</summary>
internal static class CargadorDatosIniciales
{
    private const string NombreDelArchivo = "datos-iniciales.json";

    /// <summary>Carga los datos si la base de datos no tiene categorías.</summary>
    public static async Task CargarSiEstaVaciaAsync(TiendaDbContext contexto)
    {
        if (await contexto.Categorias.AnyAsync())
        {
            return;
        }

        string ruta = Path.Combine(AppContext.BaseDirectory, "DatosIniciales", NombreDelArchivo);
        DatosInicialesArchivo datos = JsonConvert.DeserializeObject<DatosInicialesArchivo>(await File.ReadAllTextAsync(ruta))
            ?? throw new InvalidOperationException($"El archivo {NombreDelArchivo} está vacío.");

        var categoriasPorSlug = new Dictionary<string, Categoria>();
        foreach (CategoriaInicial inicial in datos.Categorias)
        {
            var categoria = new Categoria(inicial.Nombre, inicial.Slug);
            contexto.Categorias.Add(categoria);
            categoriasPorSlug[categoria.Slug] = categoria;
        }
        await contexto.SaveChangesAsync();

        foreach (ProductoInicial inicial in datos.Productos)
        {
            var producto = new Producto(inicial.Sku, inicial.Nombre, inicial.Descripcion, categoriasPorSlug[inicial.Categoria].Id,
                inicial.Precio, inicial.Costo, inicial.Stock, inicial.Imagen);
            if (!inicial.Activo)
            {
                producto.Desactivar();
            }
            contexto.Productos.Add(producto);
        }
        await contexto.SaveChangesAsync();
    }
}
