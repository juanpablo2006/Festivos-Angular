using FestivosApi.Dominio.DTOs;
namespace FestivosApi.Core.Interfaces.Servicios
{
    public interface ITipoFestivoServicio
    {
        Task<IReadOnlyList<TipoFestivoDto>> ObtenerTodosAsync();
        Task<TipoFestivoDto> ObtenerPorIdAsync(int id);
        Task<TipoFestivoDto> CrearAsync(TipoFestivoRequest request);
        Task<TipoFestivoDto> ActualizarAsync(int id, TipoFestivoRequest request);
        Task EliminarAsync(int id);
    }
}
