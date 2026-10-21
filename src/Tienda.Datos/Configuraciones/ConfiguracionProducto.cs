using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Core.Entidades;

namespace Tienda.Datos.Configuraciones;

/// <summary>Tabla de productos: el SKU tiene índice único (R-01).</summary>
internal class ConfiguracionProducto : IEntityTypeConfiguration<Producto>
{
    private const int PrecisionPrecio = 18;
    private const int EscalaPrecio = 2;

    public void Configure(EntityTypeBuilder<Producto> entidad)
    {
        entidad.HasKey(producto => producto.Id);
        entidad.Property(producto => producto.Sku).IsRequired().HasMaxLength(LargosDeCampos.Codigo);
        entidad.HasIndex(producto => producto.Sku).IsUnique();
        entidad.Property(producto => producto.Nombre).IsRequired().HasMaxLength(LargosDeCampos.Nombre);
        entidad.Property(producto => producto.Descripcion).HasMaxLength(LargosDeCampos.Descripcion);
        entidad.Property(producto => producto.ImagenUrl).HasMaxLength(LargosDeCampos.Ruta);
        entidad.Property(producto => producto.PrecioVenta).HasPrecision(PrecisionPrecio, EscalaPrecio);
        entidad.Property(producto => producto.CostoCompra).HasPrecision(PrecisionPrecio, EscalaPrecio);
        entidad.HasOne(producto => producto.Categoria)
            .WithMany()
            .HasForeignKey(producto => producto.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
