using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Dominio.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FestivosApi.Presentacion.Controllers
{
    /// <summary>Consulta de fechas festivas y administración de la definición de festivos.</summary>
    [ApiController]
    [Route("api/festivos")]
    [Produces("application/json")]
    public class FestivosController : ControllerBase
    {
        private readonly IFestivoServicio _servicio;

        public FestivosController(IFestivoServicio servicio) => _servicio = servicio;

        // ---------------------------------------------------------------------
        // Consultas de fechas
        // ---------------------------------------------------------------------

        /// <summary>Verifica si una fecha es festivo.</summary>
        /// <param name="anio">Año (1583 a 9998).</param>
        /// <param name="mes">Mes (1 a 12).</param>
        /// <param name="dia">Día del mes.</param>
        /// <param name="idPais">Id del país. Por defecto 1 (Colombia).</param>
        /// <response code="200">Resultado de la verificación (festivo o no).</response>
        /// <response code="400">La fecha o el año no son válidos.</response>
        /// <response code="404">El país no existe.</response>
        [HttpGet("verificar/{anio}/{mes}/{dia}")]
        [ProducesResponseType(typeof(VerificacionFestivoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VerificacionFestivoDto>> Verificar(
            int anio, int mes, int dia, [FromQuery] int idPais = 1) =>
            Ok(await _servicio.VerificarAsync(idPais, anio, mes, dia));

        /// <summary>Lista los festivos de un año con su fecha calculada, ordenados cronológicamente.</summary>
        /// <param name="anio">Año (1583 a 9998).</param>
        /// <param name="idPais">Id del país. Por defecto 1 (Colombia).</param>
        /// <response code="200">Festivos del año con su fecha.</response>
        /// <response code="400">El año no es válido.</response>
        /// <response code="404">El país no existe.</response>
        [HttpGet("obtener/{anio}")]
        [ProducesResponseType(typeof(IReadOnlyList<FestivoFechaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<FestivoFechaDto>>> Obtener(
            int anio, [FromQuery] int idPais = 1) =>
            Ok(await _servicio.ObtenerFestivosDelAnioAsync(idPais, anio));

        // ---------------------------------------------------------------------
        // CRUD de la definición de festivos
        // ---------------------------------------------------------------------

        /// <summary>Lista todos los festivos definidos (la regla, no la fecha calculada).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<FestivoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<FestivoDto>>> Listar() =>
            Ok(await _servicio.ObtenerTodosAsync());

        /// <summary>Obtiene un festivo por su id.</summary>
        /// <response code="404">El festivo no existe.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FestivoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FestivoDto>> ObtenerPorId(int id) =>
            Ok(await _servicio.ObtenerPorIdAsync(id));

        /// <summary>Crea un festivo.</summary>
        /// <remarks>
        /// Tipos 1 (Fijo) y 2 (Ley de puente): enviar <c>dia</c> y <c>mes</c>.
        /// Tipos 3 y 4 (basados en Pascua): enviar <c>diasPascua</c>.
        /// </remarks>
        /// <response code="201">Festivo creado.</response>
        /// <response code="400">Datos inválidos o incoherentes con el tipo.</response>
        /// <response code="409">El país ya tiene un festivo con ese nombre.</response>
        [HttpPost]
        [ProducesResponseType(typeof(FestivoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<FestivoDto>> Crear([FromBody] FestivoRequest request)
        {
            var creado = await _servicio.CrearAsync(request);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
        }

        /// <summary>Actualiza un festivo existente.</summary>
        /// <response code="400">Datos inválidos o incoherentes con el tipo.</response>
        /// <response code="404">El festivo no existe.</response>
        /// <response code="409">El país ya tiene otro festivo con ese nombre.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(FestivoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<FestivoDto>> Actualizar(int id, [FromBody] FestivoRequest request) =>
            Ok(await _servicio.ActualizarAsync(id, request));

        /// <summary>Elimina un festivo.</summary>
        /// <response code="204">Festivo eliminado.</response>
        /// <response code="404">El festivo no existe.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _servicio.EliminarAsync(id);
            return NoContent();
        }
    }
}
