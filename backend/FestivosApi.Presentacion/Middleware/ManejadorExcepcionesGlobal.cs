using FestivosApi.Core.Excepciones;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FestivosApi.Presentacion.Middleware
{
    /// <summary>
    /// Traduce las excepciones de negocio a respuestas HTTP estándar (ProblemDetails, RFC 7807).
    /// Cualquier otra excepción se registra y se responde con un 500 genérico, sin filtrar detalles internos.
    /// </summary>
    public class ManejadorExcepcionesGlobal : IExceptionHandler
    {
        private readonly ILogger<ManejadorExcepcionesGlobal> _logger;

        public ManejadorExcepcionesGlobal(ILogger<ManejadorExcepcionesGlobal> logger) => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (status, titulo, detalle) = exception switch
            {
                EntidadNoEncontradaException e => (StatusCodes.Status404NotFound, "Recurso no encontrado", e.Message),
                ReglaNegocioException e => (StatusCodes.Status400BadRequest, "Solicitud inválida", e.Message),
                ConflictoException e => (StatusCodes.Status409Conflict, "Conflicto", e.Message),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor",
                      "Ocurrió un error inesperado. Inténtelo de nuevo más tarde.")
            };

            if (status == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Error no controlado en {Metodo} {Ruta}",
                    httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = status;
            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = titulo,
                Detail = detalle,
                Instance = httpContext.Request.Path
            }, cancellationToken);

            return true;
        }
    }
}
