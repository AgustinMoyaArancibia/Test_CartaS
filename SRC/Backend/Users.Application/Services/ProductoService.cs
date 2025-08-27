using Application.DTOs;
using Application.Interfaces;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore; 

namespace Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IRepository<Producto> _repo;

        public ProductoService(IRepository<Producto> repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct)
        {
            var q = _repo.Query();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.Nombre.Contains(search));

            return await q
                .OrderBy(p => p.IdProducto)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(p => new ProductoDto(p.IdProducto, p.Nombre))
                .ToListAsync(ct);
        }

        public Task<ProductoDto?> GetByIdAsync(int id, CancellationToken ct) =>
            _repo.Query()
                .Where(p => p.IdProducto == id)
                .Select(p => new ProductoDto(p.IdProducto, p.Nombre))
                .FirstOrDefaultAsync(ct);

        public async Task<ProductoDto> CreateAsync(ProductoCreateDto dto, CancellationToken ct)
        {
            var entity = new Producto { Nombre = dto.Nombre };
            await _repo.AddAsync(entity, ct);
            return new ProductoDto(entity.IdProducto, entity.Nombre);
        }

        public async Task UpdateAsync(int id, ProductoUpdateDto dto, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Producto no encontrado");
            entity.Nombre = dto.Nombre;
            await _repo.UpdateAsync(entity, ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            var entity = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Producto no encontrado");
            await _repo.DeleteAsync(entity, ct);
        }
    }
}
