using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas.Application.DTOs;

namespace Application.Interfaces
{
    public interface IVentaService
    {
        Task<List<VentaDto>> GetAllAsync(CancellationToken ct = default);
        Task<VentaDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<int> CreateAsync(CreateVentaRequest req, CancellationToken ct = default);
        Task DeleteAsync(int id, CancellationToken ct = default);

        Task<FechaMaxVentasDto> GetFechaConMasVentasAsync(CancellationToken ct = default);
    }
}
