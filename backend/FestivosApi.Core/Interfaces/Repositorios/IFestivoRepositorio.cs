using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Core.Interfaces.Repositorios
{
    public interface IFestivoRepositorio
    {
        /// <summary>Todos los festivos, con <c>Pais</c> y <c>Tipo</c> cargados.</summary>
        Task<IReadOnlyList<Festivo>> ObtenerTodosAsync();

        /// <summary>Festivos de un país, con <c>Pais</c> y <c>Tipo</c> cargados.</summary>
        Task<IReadOnlyList<Festivo>> ObtenerPorPaisAsync(int idPais);

        Task<Festivo?> ObtenerPorIdAsync(int id);
        Task<bool> ExisteNombreAsync(int idPais, string nombre, int? excluirId = null);
        Task<Festivo> AgregarAsync(Festivo festivo);
        Task ActualizarAsync(Festivo festivo);
        Task EliminarAsync(Festivo festivo);
    }
}
