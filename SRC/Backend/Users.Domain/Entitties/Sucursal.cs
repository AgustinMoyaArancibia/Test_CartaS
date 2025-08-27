using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitties
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;

        // Relaciones
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
