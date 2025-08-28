using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Ventas.Application.DTOs;

namespace Test.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly IVentaService _service;
        public VentasController(IVentaService service) => _service = service;

     
        [HttpGet]
        public async Task<ActionResult<List<VentaDto>>> GetAll(CancellationToken ct)
            => Ok(await _service.GetAllAsync(ct));

      
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDto>> GetById(int id, CancellationToken ct)
        {
            var v = await _service.GetByIdAsync(id, ct);
            return v is null ? NotFound() : Ok(v);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateVentaRequest req, CancellationToken ct)
        {
            var id = await _service.CreateAsync(req, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await _service.DeleteAsync(id, ct);
            return NoContent();
        }

      
        [HttpGet("fecha-max")]
        public async Task<ActionResult<FechaMaxVentasDto>> FechaConMasVentas(CancellationToken ct)
            => Ok(await _service.GetFechaConMasVentasAsync(ct));
    }

}
