using System.Text.Json.Serialization;

namespace FestivosApi.Dominio.Entidades
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        [JsonIgnore]
        public List<Festivo>? Festivos { get; set; }
    }
}
