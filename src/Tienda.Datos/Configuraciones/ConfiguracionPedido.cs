using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Core.Entidades;

namespace Tienda.Datos.Configuraciones;

/// <summary>Tabla de pedidos: el código tiene índice único porque es como se consulta un pedido (R-09).</summary>
internal class ConfiguracionPedido : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> entidad)
    {
        entidad.HasKey(pedido => pedido.Id);
        entidad.Property(pedido => pedido.Codigo).IsRequired().HasMaxLength(LargosDeCampos.Codigo);
        entidad.HasIndex(pedido => pedido.Codigo).IsUnique();
        entidad.Property(pedido => pedido.NombreCliente).IsRequired().HasMaxLength(LargosDeCampos.Nombre);
        entidad.Property(pedido => pedido.DocumentoCliente).IsRequired().HasMaxLength(LargosDeCampos.Corto);
        entidad.Property(pedido => pedido.CorreoCliente).IsRequired().HasMaxLength(LargosDeCampos.Nombre);
        entidad.Property(pedido => pedido.DireccionEntrega).IsRequired().HasMaxLength(LargosDeCampos.Ruta);
        entidad.HasMany(pedido => pedido.Lineas).WithOne().HasForeignKey("PedidoId").OnDelete(DeleteBehavior.Cascade);
        entidad.Navigation(pedido => pedido.Lineas).HasField("_lineas").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
