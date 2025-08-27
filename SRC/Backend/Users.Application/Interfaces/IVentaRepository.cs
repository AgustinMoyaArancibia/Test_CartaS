using Domain.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVentaRepository
    {
        Task<Venta?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Venta>> GetAllAsync(CancellationToken ct = default);
        Task<Venta> AddAsync(Venta entity, CancellationToken ct = default);
        Task UpdateAsync(Venta entity, CancellationToken ct = default);
        Task DeleteAsync(Venta entity, CancellationToken ct = default);

        // Reporte: fecha con más ventas (cuenta cabeceras por día)
        Task<(DateTime Fecha, int Cantidad)> GetFechaConMasVentasAsync(CancellationToken ct = default);
    }
}
