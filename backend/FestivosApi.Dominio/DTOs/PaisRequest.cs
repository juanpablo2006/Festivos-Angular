using System.ComponentModel.DataAnnotations;

namespace FestivosApi.Dominio.DTOs
{
    /// <summary>Datos para crear o actualizar un país.</summary>
    public class PaisRequest
    {
        /// <summary>Nombre del país (único).</summary>
        /// <example>Colombia</example>
        [Required, StringLength(100, MinimumLength = 1)]
        public string Nombre { get; set; } = string.Empty;
    }
}
