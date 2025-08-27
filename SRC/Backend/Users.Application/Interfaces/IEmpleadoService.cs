using Application.DTOs;

namespace Application.Interfaces
{
    public interface IEmpleadoService
    {
        Task<IEnumerable<EmpleadoDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct);
        Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, EmpleadoUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
