using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;               
using Microsoft.EntityFrameworkCore;     
namespace Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _db;
    public UsuarioRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> GetByNombreAsync(string nombre, CancellationToken ct = default)
        => _db.Usuarios.FirstOrDefaultAsync(u => u.Nombre == nombre, ct);

    public Task<bool> NombreExistsAsync(string nombre, CancellationToken ct = default)
        => _db.Usuarios.AnyAsync(u => u.Nombre == nombre, ct);

    public async Task<Usuario> AddAsync(Usuario u, CancellationToken ct = default)
    {
        _db.Usuarios.Add(u);
        await _db.SaveChangesAsync(ct);
        return u;
    }
}
