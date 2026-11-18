using FestivosApi.Dominio.Entidades;
namespace FestivosApi.Core.Interfaces.Repositorios
{
    public interface ITipoFestivoRepositorio
    {
        Task<IReadOnlyList<TipoFestivo>> ObtenerTodosAsync();
        Task<TipoFestivo?> ObtenerPorIdAsync(int id);
        Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
        Task<bool> TieneFestivosAsync(int id);
        Task<TipoFestivo> AgregarAsync(TipoFestivo tipo);
        Task ActualizarAsync(TipoFestivo tipo);
        Task EliminarAsync(TipoFestivo tipo);
    }
}
