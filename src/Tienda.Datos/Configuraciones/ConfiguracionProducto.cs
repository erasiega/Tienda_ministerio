using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Core.Entidades;

namespace Tienda.Datos.Configuraciones;

/// <summary>Tabla de productos: el SKU tiene índice único (R-01).</summary>
internal class ConfiguracionProducto : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> entidad)
    {
        entidad.HasKey(producto => producto.Id);
        entidad.Property(producto => producto.Sku).IsRequired().HasMaxLength(LargosDeCampos.Codigo);
        entidad.HasIndex(producto => producto.Sku).IsUnique();
        entidad.Property(producto => producto.Nombre).IsRequired().HasMaxLength(LargosDeCampos.Nombre);
        entidad.Property(producto => producto.Descripcion).HasMaxLength(LargosDeCampos.Descripcion);
        entidad.Property(producto => producto.ImagenUrl).HasMaxLength(LargosDeCampos.Ruta);
        entidad.Property(producto => producto.PrecioVenta).HasPrecision(18, 2);
        entidad.Property(producto => producto.CostoCompra).HasPrecision(18, 2);
        entidad.HasOne(producto => producto.Categoria)
            .WithMany()
            .HasForeignKey(producto => producto.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
