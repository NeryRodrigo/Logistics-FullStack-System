using LogisticsCore.Domain.Common;
using LogisticsCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Infrastructure.Persistence
{
    public class LogisticsDbContext : DbContext
    {
        public LogisticsDbContext(DbContextOptions<LogisticsDbContext> options) : base(options)
        {
        }

        // Tus tablas
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }      // <--- ESTA ES LA QUE TE FALTA
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<Movement> Movements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esto aplica automáticamente todas las configuraciones de la carpeta 'Configurations'
            // (como la de ProductConfiguration que acabamos de hacer)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LogisticsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // TRUCO SENIOR: Interceptar el guardado para auditar fechas automáticamente
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // Si la propiedad CreatedAt no tiene setter público, usamos el constructor o reflexión, 
                        // pero como en BaseEntity ya las seteamos al iniciar, aquí solo validamos o forzamos UTC.
                        // En este caso, el BaseEntity ya lo hace, pero para LastModifiedAt:
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateLastModified();
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
