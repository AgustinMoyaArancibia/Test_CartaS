using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record EmpleadoDto(int IdEmpleado, string Nombre, bool Activo);
    public record EmpleadoCreateDto(string Nombre, bool Activo);
    public record EmpleadoUpdateDto(string Nombre, bool Activo);
}
