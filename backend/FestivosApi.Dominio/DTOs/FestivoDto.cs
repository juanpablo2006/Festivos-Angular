namespace FestivosApi.Dominio.DTOs
{
    /// <summary>Festivo tal como está definido en la base de datos (la regla, no la fecha calculada).</summary>
    public record FestivoDto(
        int Id,
        int IdPais,
        string Pais,
        string Nombre,
        int? Dia,
        int? Mes,
        int? DiasPascua,
        int IdTipo,
        string Tipo);
}
