using FestivosApi.Dominio.DTOs;

namespace FestivosApi.Core.Interfaces.Servicios
{
    public interface IFestivoServicio
    {
        // ----- CRUD de la definición de festivos -----
        Task<IReadOnlyList<FestivoDto>> ObtenerTodosAsync();
        Task<FestivoDto> ObtenerPorIdAsync(int id);
        Task<FestivoDto> CrearAsync(FestivoRequest request);
        Task<FestivoDto> ActualizarAsync(int id, FestivoRequest request);
        Task EliminarAsync(int id);

        // ----- Consultas de fechas -----

        /// <summary>Indica si la fecha dada es festivo en el país indicado.</summary>
        Task<VerificacionFestivoDto> VerificarAsync(int idPais, int anio, int mes, int dia);

        /// <summary>Lista los festivos del país resueltos a fechas concretas para el año dado, ordenados por fecha.</summary>
        Task<IReadOnlyList<FestivoFechaDto>> ObtenerFestivosDelAnioAsync(int idPais, int anio);
    }
}
