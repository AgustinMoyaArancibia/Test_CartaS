namespace Domain.Entities;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;  
    public string Pass { get; set; } = string.Empty;    
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
