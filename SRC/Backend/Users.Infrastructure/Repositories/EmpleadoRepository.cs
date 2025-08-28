
using Application.Interfaces;
using Domain.Entities;                 
using Domain.Entitties;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly AppDbContext _db;

        public EmpleadoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<(IReadOnlyList<Empleado> Items, int Total)> GetAllAsync(
            string? search, bool? activo, int page, int size, CancellationToken ct = default)
        {
           
            page = page <= 0 ? 1 : page;
            size = size <= 0 ? 10 : size;

            var query = _db.Empleados.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.Nombre.Contains(search));

            if (activo is not null)
                query = query.Where(e => e.Activo == activo.Value);

            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(e => e.IdEmpleado)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);

            return (items, total);
        }

        public async Task<Empleado?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _db.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id, ct);

        public async Task<Empleado> AddAsync(Empleado entity, CancellationToken ct = default)
        {
            _db.Empleados.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Empleado entity, CancellationToken ct = default)
        {
            _db.Empleados.Update(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Empleado entity, CancellationToken ct = default)
        {
            _db.Empleados.Remove(entity);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default)
            => await _db.Empleados.AnyAsync(e => e.Nombre == nombre, ct);
    }
}
