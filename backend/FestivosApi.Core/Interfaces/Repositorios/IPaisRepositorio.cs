using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Core.Interfaces.Repositorios
{
    public interface IPaisRepositorio
    {
        Task<IReadOnlyList<Pais>> ObtenerTodosAsync();
        Task<Pais?> ObtenerPorIdAsync(int id);
        Task<bool> ExisteAsync(int id);
        Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
        Task<bool> TieneFestivosAsync(int id);
        Task<Pais> AgregarAsync(Pais pais);
        Task ActualizarAsync(Pais pais);
        Task EliminarAsync(Pais pais);
    }
}
