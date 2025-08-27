using Application.DTOs;
using Application.Interfaces;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IRepository<Cliente> _repo;

        public ClienteService(IRepository<Cliente> repo) => _repo = repo;

        public async Task<IEnumerable<ClienteDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct)
        {
            var q = _repo.Query();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(c => c.Nombre.Contains(search) || c.Dni.Contains(search));

            return await q.OrderBy(c => c.IdCliente)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(c => new ClienteDto(c.IdCliente, c.Dni, c.Nombre, c.DireccionEnvio))
                .ToListAsync(ct);
        }

        public Task<ClienteDto?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.Query()
                .Where(c => c.IdCliente == id)
                .Select(c => new ClienteDto(c.IdCliente, c.Dni, c.Nombre, c.DireccionEnvio))
                .FirstOrDefaultAsync(ct);

        public async Task<ClienteDto> CreateAsync(ClienteCreateDto dto, CancellationToken ct)
        {
            var entity = new Cliente
            {
                Dni = dto.Dni,
                Nombre = dto.Nombre,
                DireccionEnvio = dto.DireccionEnvio
            };
            await _repo.AddAsync(entity, ct);
            return new ClienteDto(entity.IdCliente, entity.Dni, entity.Nombre, entity.DireccionEnvio);
        }

        public async Task UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Cliente no encontrado");
            entity.Dni = dto.Dni;
            entity.Nombre = dto.Nombre;
            entity.DireccionEnvio = dto.DireccionEnvio;
            await _repo.UpdateAsync(entity, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Cliente no encontrado");
            await _repo.DeleteAsync(entity, ct);
        }
    }
}
