using LogisticsCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Sku { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public bool IsActive { get; private set; }

        // --- NUEVAS PROPIEDADES ---
        public Guid CategoryId { get; private set; }
        // 'virtual' permite a EF Core hacer Lazy Loading si se necesita
        public virtual Category Category { get; private set; }

        // Constructor privado necesario para Entity Framework
        private Product()
        {
            Name = null!;
            Sku = null!;
            Category = null!; // Engañamos al compilador para evitar advertencias CS8618
        }

        // Constructor público actualizado
        public Product(string name, string sku, decimal price, int stock, Guid categoryId)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            if (price <= 0) throw new ArgumentException("Price must be greater than zero");

            // Validación de la relación: Un producto NO puede existir sin categoría
            if (categoryId == Guid.Empty) throw new ArgumentException("Category is required");

            Name = name;
            Sku = sku;
            Price = price;
            Stock = stock;
            CategoryId = categoryId;
            IsActive = true;
        }

        public void UpdateDetails(string name, string sku, decimal price, Guid categoryId)
        {
            // Validaciones de dominio
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
            if (price <= 0) throw new ArgumentException("Price must be greater than zero");
            if (categoryId == Guid.Empty) throw new ArgumentException("Category is required");

            Name = name;
            Sku = sku;
            Price = price;
            CategoryId = categoryId;
            UpdateLastModified();
        }

        public void AddStock(int quantity)
        {
            Stock += quantity;
            UpdateLastModified();
        }
    }
}
