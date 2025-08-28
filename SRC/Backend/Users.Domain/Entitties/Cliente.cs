using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entitties
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? DireccionEnvio { get; set; }


        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}
