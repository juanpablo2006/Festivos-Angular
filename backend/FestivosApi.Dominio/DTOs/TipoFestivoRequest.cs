using System.ComponentModel.DataAnnotations;
namespace FestivosApi.Dominio.DTOs
{
    public class TipoFestivoRequest
    {
        [Required, StringLength(100, MinimumLength = 1)]
        public string Tipo { get; set; } = string.Empty;
    }
}
