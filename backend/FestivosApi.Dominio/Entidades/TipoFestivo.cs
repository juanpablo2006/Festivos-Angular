using System.Text.Json.Serialization;

namespace FestivosApi.Dominio.Entidades
{
    public class TipoFestivo
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Festivo>? Festivos { get; set; }
    }
}
