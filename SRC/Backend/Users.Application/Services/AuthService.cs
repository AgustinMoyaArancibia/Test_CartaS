using System.IdentityModel.Tokens.Jwt;      // JwtSecurityToken, JwtSecurityTokenHandler
using System.Security.Claims;
using System.Text;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;      

namespace Application.Services;

public class JwtOptions
{
    public string Issuer { get; set; } = "ventas-api";
    public string Audience { get; set; } = "ventas-client";
    public string Key { get; set; } = "";   
    public int ExpMinutes { get; set; } = 60;
}

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _users;
    private readonly IPasswordHasher<Usuario> _hasher;
    private readonly JwtOptions _opts;

    public AuthService(IUsuarioRepository users, IPasswordHasher<Usuario> hasher, IOptions<JwtOptions> opts)
    {
        _users = users;
        _hasher = hasher;
        _opts = opts.Value;
    }

    public async Task<int> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        if (await _users.NombreExistsAsync(req.Nombre, ct))
            throw new InvalidOperationException("El usuario ya existe.");

        var u = new Usuario { Nombre = req.Nombre };
        u.Pass = _hasher.HashPassword(u, req.Pass);

        var created = await _users.AddAsync(u, ct);
        return created.IdUsuario;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var u = await _users.GetByNombreAsync(req.Nombre, ct)
                ?? throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

        var result = _hasher.VerifyHashedPassword(u, u.Pass, req.Pass);
        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

        var expires = DateTime.UtcNow.AddMinutes(_opts.ExpMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, u.IdUsuario.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, u.Nombre)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _opts.Issuer,
            audience: _opts.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResponse(jwt, expires);
    }
}
