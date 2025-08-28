using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default);
    Task<int> RegisterAsync(RegisterRequest req, CancellationToken ct = default);
}
