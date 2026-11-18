namespace FestivosApi.Dominio.Entidades
{
    public class Festivo
    {
        public int Id { get; set; }

        public int IdPais { get; set; }
        public Pais? Pais { get; set; }

        public string Nombre { get; set; } = string.Empty;

        // Nulos cuando el festivo se calcula a partir del domingo de pascua (Tipo 3 y 4)
        public int? Dia { get; set; }
        public int? Mes { get; set; }

        // Nulo cuando el festivo es de fecha fija (Tipo 1 y 2)
        public int? DiasPascua { get; set; }

        public int IdTipo { get; set; }
        public TipoFestivo? Tipo { get; set; }
    }
}
