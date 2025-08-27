using Application.DTOs;
using Application.Interfaces;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly IRepository<Sucursal> _repo;

        public SucursalService(IRepository<Sucursal> repo) => _repo = repo;

        public async Task<IEnumerable<SucursalDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct)
        {
            var q = _repo.Query();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(s => s.Nombre.Contains(search) || s.Direccion.Contains(search));

            return await q.OrderBy(s => s.IdSucursal)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(s => new SucursalDto(s.IdSucursal, s.Nombre, s.Direccion))
                .ToListAsync(ct);
        }

        public Task<SucursalDto?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.Query()
                .Where(s => s.IdSucursal == id)
                .Select(s => new SucursalDto(s.IdSucursal, s.Nombre, s.Direccion))
                .FirstOrDefaultAsync(ct);

        public async Task<SucursalDto> CreateAsync(SucursalCreateDto dto, CancellationToken ct)
        {
            var entity = new Sucursal
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion
            };
            await _repo.AddAsync(entity, ct);
            return new SucursalDto(entity.IdSucursal, entity.Nombre, entity.Direccion);
        }

        public async Task UpdateAsync(int id, SucursalUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Sucursal no encontrada");
            entity.Nombre = dto.Nombre;
            entity.Direccion = dto.Direccion;
            await _repo.UpdateAsync(entity, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Sucursal no encontrada");
            await _repo.DeleteAsync(entity, ct);
        }
    }
}
