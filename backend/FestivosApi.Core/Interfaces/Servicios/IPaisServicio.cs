using FestivosApi.Dominio.DTOs;

namespace FestivosApi.Core.Interfaces.Servicios
{
    public interface IPaisServicio
    {
        Task<IReadOnlyList<PaisDto>> ObtenerTodosAsync();
        Task<PaisDto> ObtenerPorIdAsync(int id);
        Task<PaisDto> CrearAsync(PaisRequest request);
        Task<PaisDto> ActualizarAsync(int id, PaisRequest request);
        Task EliminarAsync(int id);
    }
}
