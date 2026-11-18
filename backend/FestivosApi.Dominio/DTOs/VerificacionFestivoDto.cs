namespace FestivosApi.Dominio.DTOs
{
    /// <summary>Resultado de verificar si una fecha es festiva.</summary>
    /// <param name="Fecha">Fecha consultada.</param>
    /// <param name="EsFestivo">Indica si la fecha es festivo en el país consultado.</param>
    /// <param name="Festivos">Nombres de los festivos que caen en esa fecha (vacío si no es festivo).</param>
    /// <param name="Mensaje">Mensaje legible con el resultado.</param>
    public record VerificacionFestivoDto(DateOnly Fecha, bool EsFestivo, IReadOnlyList<string> Festivos, string Mensaje);
}
