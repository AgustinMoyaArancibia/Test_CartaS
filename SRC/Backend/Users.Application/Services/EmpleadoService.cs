using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Entitties;

namespace Application.Services;

public class EmpleadoService : IEmpleadoService
{
    private readonly IEmpleadoRepository _repo;

    public EmpleadoService(IEmpleadoRepository repo)
        => _repo = repo;

    public async Task<(IReadOnlyList<EmpleadoDto> Items, int Total)> GetAllAsync(
        string? search, bool? activo, int page, int size, CancellationToken ct = default)
    {
        var (items, total) = await _repo.GetAllAsync(search, activo, page, size, ct);

        var dtos = items.Select(e => new EmpleadoDto(e.IdEmpleado, e.Nombre, e.Activo))
                        .ToList()
                        .AsReadOnly();

        return (dtos, total);
    }

    public async Task<EmpleadoDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity is null ? null : new EmpleadoDto(entity.IdEmpleado, entity.Nombre, entity.Activo);
    }

    public async Task<EmpleadoDto> CreateAsync(EmpleadoCreateDto req, CancellationToken ct = default)
    {
        var entity = new Empleado { Nombre = req.Nombre, Activo = req.Activo };
        var created = await _repo.AddAsync(entity, ct);
        return new EmpleadoDto(created.IdEmpleado, created.Nombre, created.Activo);
    }

    public async Task UpdateAsync(int id, EmpleadoUpdateDto req, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Empleado no encontrado");
        existing.Nombre = req.Nombre;
        existing.Activo = req.Activo;
        await _repo.UpdateAsync(existing, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Empleado no encontrado");
        await _repo.DeleteAsync(existing, ct);
    }
}
