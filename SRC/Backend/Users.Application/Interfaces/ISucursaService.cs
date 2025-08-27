using Application.DTOs;

namespace Application.Interfaces
{
    public interface ISucursalService
    {
        Task<IEnumerable<SucursalDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct);
        Task<SucursalDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<SucursalDto> CreateAsync(SucursalCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, SucursalUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
