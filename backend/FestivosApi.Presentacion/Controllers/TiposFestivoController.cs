using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace FestivosApi.Presentacion.Controllers
{
    [ApiController]
    [Route("api/tipos-festivo")]
    [Produces("application/json")]
    public class TiposFestivoController : ControllerBase
    {
        private readonly ITipoFestivoServicio _servicio;
        public TiposFestivoController(ITipoFestivoServicio servicio) => _servicio = servicio;
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TipoFestivoDto>>> Listar() => Ok(await _servicio.ObtenerTodosAsync());
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TipoFestivoDto>> ObtenerPorId(int id) => Ok(await _servicio.ObtenerPorIdAsync(id));
        [HttpPost]
        public async Task<ActionResult<TipoFestivoDto>> Crear([FromBody] TipoFestivoRequest request) {
            var creado = await _servicio.CrearAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TipoFestivoDto>> Actualizar(int id, [FromBody] TipoFestivoRequest request) =>
            Ok(await _servicio.ActualizarAsync(id, request));
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id) {
            await _servicio.EliminarAsync(id);
            return NoContent();
        }
    }
}
