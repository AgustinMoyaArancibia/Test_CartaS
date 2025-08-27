using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public record ProductoDto(int IdProducto, string Nombre);
    public record ProductoCreateDto(string Nombre);
    public record ProductoUpdateDto(string Nombre);
}
