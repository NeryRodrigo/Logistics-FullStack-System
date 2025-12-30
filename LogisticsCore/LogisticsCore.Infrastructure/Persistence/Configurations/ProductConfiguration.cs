using LogisticsCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Nombre de la tabla
            builder.ToTable("Products");

            // Clave primaria
            builder.HasKey(p => p.Id);

            // Configuración de propiedades
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Sku)
                .IsRequired()
                .HasMaxLength(20);

            // IMPORTANTE: Definir precisión para moneda (18 dígitos, 2 decimales)
            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();
        }
    }
}
