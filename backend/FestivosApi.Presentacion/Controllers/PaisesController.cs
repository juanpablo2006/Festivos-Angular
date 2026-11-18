using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FestivosApi.Presentacion.Controllers
{
    /// <summary>Administración de países.</summary>
    [ApiController]
    [Route("api/paises")]
    [Produces("application/json")]
    public class PaisesController : ControllerBase
    {
        private readonly IPaisServicio _servicio;

        public PaisesController(IPaisServicio servicio) => _servicio = servicio;

        /// <summary>Lista todos los países.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<PaisDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<PaisDto>>> Listar() =>
            Ok(await _servicio.ObtenerTodosAsync());

        /// <summary>Obtiene un país por su id.</summary>
        /// <response code="404">El país no existe.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PaisDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PaisDto>> ObtenerPorId(int id) =>
            Ok(await _servicio.ObtenerPorIdAsync(id));

        /// <summary>Crea un país.</summary>
        /// <response code="201">País creado.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">Ya existe un país con ese nombre.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PaisDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PaisDto>> Crear([FromBody] PaisRequest request)
        {
            var creado = await _servicio.CrearAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }

        /// <summary>Actualiza el nombre de un país.</summary>
        /// <response code="404">El país no existe.</response>
        /// <response code="409">Ya existe otro país con ese nombre.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PaisDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PaisDto>> Actualizar(int id, [FromBody] PaisRequest request) =>
            Ok(await _servicio.ActualizarAsync(id, request));

        /// <summary>Elimina un país que no tenga festivos asociados.</summary>
        /// <response code="204">País eliminado.</response>
        /// <response code="404">El país no existe.</response>
        /// <response code="409">El país tiene festivos asociados.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _servicio.EliminarAsync(id);
            return NoContent();
        }
    }
}
