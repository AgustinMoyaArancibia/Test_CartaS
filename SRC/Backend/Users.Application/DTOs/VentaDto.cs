namespace Ventas.Application.DTOs;

public record VentaDetalleDto(int IdProducto, string Producto, int Cantidad, decimal PrecioUnitario, decimal SubTotal);

public record VentaDto(
    int IdVenta,
    DateTime FechaVenta,
    int IdCliente, string Cliente,
    int IdEmpleado, string Empleado,
    int IdSucursal, string Sucursal,
    decimal ImporteTotal,
    List<VentaDetalleDto> Detalles
);

public record CreateVentaDetalleRequest(int IdProducto, int Cantidad, decimal PrecioUnitario);
public record CreateVentaRequest(
    DateTime FechaVenta,
    int IdCliente,
    int IdEmpleado,
    int IdSucursal,
    List<CreateVentaDetalleRequest> Detalles
);

public record FechaMaxVentasDto(DateTime Fecha, int CantidadVentas);
