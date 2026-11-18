using System.ComponentModel.DataAnnotations;

namespace FestivosApi.Dominio.DTOs
{
    /// <summary>
    /// Datos para crear o actualizar un festivo.
    /// Tipos 1 y 2 requieren <see cref="Dia"/> y <see cref="Mes"/>; tipos 3 y 4 requieren <see cref="DiasPascua"/>.
    /// </summary>
    public class FestivoRequest
    {
        /// <summary>Id del país al que pertenece el festivo.</summary>
        /// <example>1</example>
        [Range(1, int.MaxValue)]
        public int IdPais { get; set; }

        /// <summary>Nombre del festivo.</summary>
        /// <example>Día de la Independencia</example>
        [Required, StringLength(150, MinimumLength = 1)]
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Día del mes (solo tipos 1 y 2).</summary>
        /// <example>20</example>
        [Range(1, 31)]
        public int? Dia { get; set; }

        /// <summary>Mes (solo tipos 1 y 2).</summary>
        /// <example>7</example>
        [Range(1, 12)]
        public int? Mes { get; set; }

        /// <summary>Días de desplazamiento respecto al Domingo de Pascua (solo tipos 3 y 4). Puede ser negativo.</summary>
        /// <example>40</example>
        [Range(-365, 365)]
        public int? DiasPascua { get; set; }

        /// <summary>Id del tipo de festivo (1 a 4).</summary>
        /// <example>1</example>
        [Range(1, int.MaxValue)]
        public int IdTipo { get; set; }
    }
}
