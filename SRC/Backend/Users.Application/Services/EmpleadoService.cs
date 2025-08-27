using Application.DTOs;
using Application.Interfaces;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IRepository<Empleado> _repo;

        public EmpleadoService(IRepository<Empleado> repo) => _repo = repo;

        public async Task<IEnumerable<EmpleadoDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct)
        {
            var q = _repo.Query();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => e.Nombre.Contains(search));

            return await q.OrderBy(e => e.IdEmpleado)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(e => new EmpleadoDto(e.IdEmpleado, e.Nombre, e.Activo))
                .ToListAsync(ct);
        }

        public Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.Query()
                .Where(e => e.IdEmpleado == id)
                .Select(e => new EmpleadoDto(e.IdEmpleado, e.Nombre, e.Activo))
                .FirstOrDefaultAsync(ct);

        public async Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto, CancellationToken ct)
        {
            var entity = new Empleado
            {
                Nombre = dto.Nombre,
                Activo = dto.Activo
            };
            await _repo.AddAsync(entity, ct);
            return new EmpleadoDto(entity.IdEmpleado, entity.Nombre, entity.Activo);
        }

        public async Task UpdateAsync(int id, EmpleadoUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Empleado no encontrado");
            entity.Nombre = dto.Nombre;
            entity.Activo = dto.Activo;
            await _repo.UpdateAsync(entity, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Empleado no encontrado");
            await _repo.DeleteAsync(entity, ct);
        }
    }
}
