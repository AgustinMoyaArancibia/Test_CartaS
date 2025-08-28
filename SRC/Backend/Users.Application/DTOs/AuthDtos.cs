namespace Application.DTOs;


public record LoginRequest(string Nombre, string Pass);
public record RegisterRequest(string Nombre, string Pass);
public record LoginResponse(string Token, DateTime ExpiresAt);
