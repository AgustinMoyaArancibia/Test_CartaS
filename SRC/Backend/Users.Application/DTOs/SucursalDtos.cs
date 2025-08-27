using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record SucursalDto(int IdSucursal, string Nombre, string Direccion);
    public record SucursalCreateDto(string Nombre, string Direccion);
    public record SucursalUpdateDto(string Nombre, string Direccion);
}
