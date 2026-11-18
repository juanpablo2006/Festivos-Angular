namespace FestivosApi.Dominio.DTOs
{
    /// <summary>Un festivo ya resuelto a una fecha concreta para un año determinado.</summary>
    public record FestivoFechaDto(int IdFestivo, string Nombre, DateOnly Fecha, int IdTipo, string Tipo);
}
