using LogisticsCore.Domain.Common;
using LogisticsCore.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Entities
{
    public class Movement : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public Guid WarehouseId { get; private set; }
        public MovementType Type { get; private set; }
        public int Quantity { get; private set; } // Siempre positivo
        public DateTime Date { get; private set; }
        public string? Reference { get; private set; } // Ej: "Orden #123"

        private Movement() { }

        public Movement(Guid productId, Guid warehouseId, MovementType type, int quantity, string? reference)
        {
            if (quantity <= 0) throw new ArgumentException("La cantidad del movimiento debe ser positiva");

            ProductId = productId;
            WarehouseId = warehouseId;
            Type = type;
            Quantity = quantity;
            Reference = reference;
            Date = DateTime.UtcNow;
        }
    }
}
