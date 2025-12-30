using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Enums
{
    public enum MovementType
    {
        Input = 1,      // Compra o Devolución
        Output = 2,     // Venta
        Adjustment = 3, // Corrección de inventario
        Transfer = 4    // Entre bodegas
    }
}
