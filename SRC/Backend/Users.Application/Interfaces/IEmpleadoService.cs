
using Application.DTOs;

namespace Application.Interfaces;

public interface IEmpleadoService
{
    Task<(IReadOnlyList<EmpleadoDto> Items, int Total)>
        GetAllAsync(string? search, bool? activo, int page, int size, CancellationToken ct = default);

    Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto req, CancellationToken ct = default);

    Task UpdateAsync(int id, EmpleadoUpdateDto req, CancellationToken ct = default);

    Task DeleteAsync(int id, CancellationToken ct = default);
}
