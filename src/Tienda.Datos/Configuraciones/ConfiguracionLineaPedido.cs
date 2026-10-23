using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Core.Entidades;

namespace Tienda.Datos.Configuraciones;

/// <summary>Tabla de líneas de pedido: guardan el nombre y el precio del momento de la compra (P-02).</summary>
internal class ConfiguracionLineaPedido : IEntityTypeConfiguration<LineaPedido>
{
    private const int PrecisionPrecio = 18;
    private const int EscalaPrecio = 2;

    public void Configure(EntityTypeBuilder<LineaPedido> entidad)
    {
        entidad.HasKey(linea => linea.Id);
        entidad.Property(linea => linea.NombreProducto).IsRequired().HasMaxLength(LargosDeCampos.Nombre);
        entidad.Property(linea => linea.PrecioUnitario).HasPrecision(PrecisionPrecio, EscalaPrecio);
        entidad.HasOne<Producto>().WithMany().HasForeignKey(linea => linea.ProductoId).OnDelete(DeleteBehavior.Restrict);
    }
}
