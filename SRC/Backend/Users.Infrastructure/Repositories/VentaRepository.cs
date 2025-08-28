using Application.Interfaces;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class VentaRepository : IVentaRepository
    {
        private readonly AppDbContext _db;
        public VentaRepository(AppDbContext db) => _db = db;

        public async Task<Venta?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _db.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Empleado)
                .Include(v => v.Sucursal)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id, ct);

        public async Task<List<Venta>> GetAllAsync(CancellationToken ct = default)
            => await _db.Ventas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.Empleado)
                .Include(v => v.Sucursal)
                .Include(v => v.Detalles)
                    .ThenInclude(d => d.Producto)
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync(ct);

        public async Task<Venta> AddAsync(Venta entity, CancellationToken ct = default)
        {
            _db.Ventas.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Venta entity, CancellationToken ct = default)
        {
            _db.Ventas.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Venta entity, CancellationToken ct = default)
        {
            _db.Ventas.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<(DateTime Fecha, int Cantidad)> GetFechaConMasVentasAsync(CancellationToken ct = default)
        {
            var q = await _db.Ventas
                .GroupBy(v => v.FechaVenta.Date)
                .Select(g => new { Fecha = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .ThenBy(x => x.Fecha) 
                .FirstOrDefaultAsync(ct);

            if (q is null) return (DateTime.MinValue, 0);
            return (q.Fecha, q.Cantidad);
        }
    }
}
