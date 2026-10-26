using Tienda.Core.Entidades;
using Tienda.Core.Excepciones;
using Tienda.Core.Repositorios;

namespace Tienda.Core.Servicios;

/// <summary>Registro y consulta de pedidos. Un pedido baja el stock de cada producto, completo o nada (R-05).</summary>
public class ServicioPedidos
{
    private readonly IRepositorioPedidos _pedidos;
    private readonly IRepositorioProductos _productos;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly TimeProvider _reloj;

    /// <summary>Crea el servicio con sus dependencias.</summary>
    /// <param name="pedidos">Repositorio de pedidos.</param>
    /// <param name="productos">Repositorio de productos.</param>
    /// <param name="unidadDeTrabajo">Guarda todo en una sola operación.</param>
    /// <param name="reloj">Da la fecha del pedido; en las pruebas se puede fijar.</param>
    public ServicioPedidos(IRepositorioPedidos pedidos, IRepositorioProductos productos,
        IUnidadDeTrabajo unidadDeTrabajo, TimeProvider reloj)
    {
        _pedidos = pedidos;
        _productos = productos;
        _unidadDeTrabajo = unidadDeTrabajo;
        _reloj = reloj;
    }

    /// <summary>
    /// Registra un pedido. Primero valida todas las líneas (existencia, estado y stock);
    /// solo si todas sirven baja los stocks y guarda, en una sola operación (R-05).
    /// El precio de cada línea es el precio de venta del producto (R-06) y el total lo calcula el pedido (R-07).
    /// </summary>
    /// <param name="datos">Datos de quien compra y las líneas.</param>
    /// <returns>El pedido registrado, con su código (R-09).</returns>
    /// <exception cref="DatosInvalidosException">Si no hay líneas, si se repite un producto o si faltan datos (R-04, CA-15).</exception>
    /// <exception cref="NoEncontradoException">Si un producto no existe.</exception>
    /// <exception cref="ReglaDelNegocioException">Si un producto está inactivo (R-08) o no tiene stock (R-05).</exception>
    public async Task<Pedido> RegistrarAsync(DatosPedido datos)
    {
        if (datos.Lineas is null || datos.Lineas.Count == 0)
        {
            throw new DatosInvalidosException("Un pedido debe tener al menos una línea (R-04).");
        }
        foreach (DatosLineaPedido linea in datos.Lineas)
        {
            if (linea.Cantidad <= 0)
            {
                throw new DatosInvalidosException("La cantidad debe ser mayor que cero (R-04).");
            }
        }
        if (datos.Lineas.Select(linea => linea.ProductoId).Distinct().Count() != datos.Lineas.Count)
        {
            throw new DatosInvalidosException("Un pedido no puede tener el mismo producto en dos líneas (R-04).");
        }

        var productosPedidos = new List<(Producto Producto, int Cantidad)>();
        foreach (DatosLineaPedido linea in datos.Lineas)
        {
            Producto producto = await _productos.ObtenerPorIdAsync(linea.ProductoId)
                ?? throw new NoEncontradoException($"No existe el producto {linea.ProductoId}.");
            if (!producto.Activo)
            {
                throw new ReglaDelNegocioException($"El producto {producto.Sku} no está disponible para la venta.", "R-08");
            }
            if (!producto.TieneStockPara(linea.Cantidad))
            {
                throw new ReglaDelNegocioException(
                    $"No hay stock suficiente de {producto.Sku}: se piden {linea.Cantidad}. No se registró el pedido.", "R-05");
            }
            productosPedidos.Add((producto, linea.Cantidad));
        }

        var pedido = new Pedido(datos.NombreCliente, datos.DocumentoCliente, datos.CorreoCliente, datos.DireccionEntrega,
            _reloj.GetLocalNow().DateTime,
            productosPedidos.Select(item => new LineaPedido(item.Producto.Id, item.Producto.Nombre, item.Cantidad, item.Producto.PrecioVenta)));

        foreach ((Producto producto, int cantidad) in productosPedidos)
        {
            producto.DisminuirStock(cantidad);
        }
        _pedidos.Agregar(pedido);
        await _unidadDeTrabajo.GuardarCambiosAsync();
        return pedido;
    }

    /// <summary>Busca un pedido por su código (CA-16, CA-17).</summary>
    /// <param name="codigo">Código que recibió quien compró.</param>
    /// <returns>El pedido con sus líneas.</returns>
    /// <exception cref="NoEncontradoException">Si no hay un pedido con ese código (R-09).</exception>
    public async Task<Pedido> ObtenerPorCodigoAsync(string codigo)
    {
        string codigoLimpio = codigo?.Trim().ToLowerInvariant() ?? string.Empty;
        return await _pedidos.ObtenerPorCodigoAsync(codigoLimpio)
            ?? throw new NoEncontradoException("No existe un pedido con ese código.");
    }
}
