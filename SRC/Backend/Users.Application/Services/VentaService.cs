using Application.Interfaces;
using Domain.Entitties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ventas.Application.DTOs;

namespace Application.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _repo;
        public VentaService(IVentaRepository repo) => _repo = repo;

        public async Task<List<VentaDto>> GetAllAsync(CancellationToken ct = default)
            => (await _repo.GetAllAsync(ct)).Select(MapToDto).ToList();

        public async Task<VentaDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var v = await _repo.GetByIdAsync(id, ct);
            return v is null ? null : MapToDto(v);
        }

        public async Task<int> CreateAsync(CreateVentaRequest req, CancellationToken ct = default)
        {
            if (req.Detalles is null || req.Detalles.Count == 0)
                throw new InvalidOperationException("La venta debe tener al menos un detalle.");

            var venta = new Venta
            {
                FechaVenta = req.FechaVenta,
                IdCliente = req.IdCliente,
                IdEmpleado = req.IdEmpleado,
                IdSucursal = req.IdSucursal,
                ImporteTotal = 0m,
                Detalles = req.Detalles.Select(d => new VentaDetalle
                {
                    IdProducto = d.IdProducto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };
     
            venta.ImporteTotal = venta.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario);

            var created = await _repo.AddAsync(venta, ct);
            return created.IdVenta;
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var v = await _repo.GetByIdAsync(id, ct) ?? throw new KeyNotFoundException("Venta no encontrada.");
            await _repo.DeleteAsync(v, ct);
        }

        public async Task<FechaMaxVentasDto> GetFechaConMasVentasAsync(CancellationToken ct = default)
        {
            var (fecha, cantidad) = await _repo.GetFechaConMasVentasAsync(ct);
            return new FechaMaxVentasDto(fecha.Date, cantidad);
        }

      
        private static VentaDto MapToDto(Venta v)
            => new(
                v.IdVenta,
                v.FechaVenta,
                v.IdCliente, v.Cliente?.Nombre ?? "",
                v.IdEmpleado, v.Empleado?.Nombre ?? "",
                v.IdSucursal, v.Sucursal?.Nombre ?? "",
                v.ImporteTotal,
                v.Detalles.Select(d => new VentaDetalleDto(
                    d.IdProducto,
                    d.Producto?.Nombre ?? "",
                    d.Cantidad,
                    d.PrecioUnitario,
                    d.SubTotal
                )).ToList()
            );
    }
}
