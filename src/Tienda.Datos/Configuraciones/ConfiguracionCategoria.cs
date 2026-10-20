using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tienda.Core.Entidades;

namespace Tienda.Datos.Configuraciones;

/// <summary>Tabla de categorías: el slug es único porque identifica la categoría en la dirección web.</summary>
internal class ConfiguracionCategoria : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> entidad)
    {
        entidad.HasKey(categoria => categoria.Id);
        entidad.Property(categoria => categoria.Nombre).IsRequired().HasMaxLength(LargosDeCampos.Corto);
        entidad.Property(categoria => categoria.Slug).IsRequired().HasMaxLength(LargosDeCampos.Corto);
        entidad.HasIndex(categoria => categoria.Slug).IsUnique();
    }
}
