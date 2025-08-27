using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct);
        Task<ProductoDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<ProductoDto> CreateAsync(ProductoCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, ProductoUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
