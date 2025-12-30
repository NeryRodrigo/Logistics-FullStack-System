using LogisticsCore.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Domain.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; private set; }
        public string Address { get; private set; }

        private Warehouse() { }

        public Warehouse(string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre requerido");
            Name = name;
            Address = address;
        }
    }
}
