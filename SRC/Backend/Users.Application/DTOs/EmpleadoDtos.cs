namespace Application.DTOs;

public record EmpleadoDto(int IdEmpleado, string Nombre, bool Activo);
public record EmpleadoCreateDto(string Nombre, bool Activo);
public record EmpleadoUpdateDto(string Nombre, bool Activo);
