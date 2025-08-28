using Domain.Entities;

namespace Application.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct = default);
    Task<bool> NombreExistsAsync(string nombre, CancellationToken ct = default);
    Task<Usuario> AddAsync(Usuario u, CancellationToken ct = default);
}
