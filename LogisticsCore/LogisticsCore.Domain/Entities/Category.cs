using LogisticsCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }

        // Relación: Una categoría tiene muchos productos
        public virtual ICollection<Product> Products { get; private set; }

        private Category() { } // EF Core

        public Category(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre es obligatorio");
            Name = name;
            Description = description;
        }
    }
}
