using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitties
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal ImporteTotal { get; set; }

        // Claves foráneas
        public int IdCliente { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public int IdEmpleado { get; set; }
        public Empleado Empleado { get; set; } = null!;

        public int IdSucursal { get; set; }
        public Sucursal Sucursal { get; set; } = null!;

        // Relaciones
        public ICollection<VentaDetalle> Detalles { get; set; } = new List<VentaDetalle>();
    }
}
