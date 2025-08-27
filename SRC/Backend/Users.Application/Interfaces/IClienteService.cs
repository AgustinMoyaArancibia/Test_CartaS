using Application.DTOs;

namespace Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync(string? search, int page, int size, CancellationToken ct);
        Task<ClienteDto?> GetByIdAsync(int id, CancellationToken ct);
        Task<ClienteDto> CreateAsync(ClienteCreateDto dto, CancellationToken ct);
        Task UpdateAsync(int id, ClienteUpdateDto dto, CancellationToken ct);
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
