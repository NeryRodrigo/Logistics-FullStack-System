using LogisticsCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Entities
{
    public class ProductStock : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public Guid WarehouseId { get; private set; }
        public int Quantity { get; private set; }
        public int MinStockLimit { get; private set; } // Alerta para reponer

        // Navegación
        public virtual Product Product { get; private set; }
        public virtual Warehouse Warehouse { get; private set; }

        private ProductStock() { }

        public ProductStock(Guid productId, Guid warehouseId, int minStockLimit)
        {
            ProductId = productId;
            WarehouseId = warehouseId;
            MinStockLimit = minStockLimit;
            Quantity = 0; // Empieza en 0 siempre
        }

        public void AdjustQuantity(int amount)
        {
            if (Quantity + amount < 0) throw new InvalidOperationException("No hay stock suficiente en esta bodega.");
            Quantity += amount;
            UpdateLastModified();
        }
    }
}
