using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record ClienteDto(int IdCliente, string Dni, string Nombre, string DireccionEnvio);
    public record ClienteCreateDto(string Dni, string Nombre, string DireccionEnvio);
    public record ClienteUpdateDto(string Dni, string Nombre, string DireccionEnvio);
}
