using Domain.Entities;
using Domain.Entitties;

namespace Application.Interfaces;

public interface IEmpleadoRepository
{
    Task<(IReadOnlyList<Empleado> Items, int Total)> GetAllAsync(
        string? search, bool? activo, int page, int size, CancellationToken ct = default);

    Task<Empleado?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Empleado> AddAsync(Empleado entity, CancellationToken ct = default);
    Task UpdateAsync(Empleado entity, CancellationToken ct = default);
    Task DeleteAsync(Empleado entity, CancellationToken ct = default);
    Task<bool> ExistsByNombreAsync(string nombre, CancellationToken ct = default);
}
