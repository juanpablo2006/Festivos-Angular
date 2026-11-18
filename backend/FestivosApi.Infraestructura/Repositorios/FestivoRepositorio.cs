using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Dominio.Entidades;
using FestivosApi.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

namespace FestivosApi.Infraestructura.Repositorios
{
    public class FestivoRepositorio : IFestivoRepositorio
    {
        private readonly ApplicationDbContext _contexto;

        public FestivoRepositorio(ApplicationDbContext contexto) => _contexto = contexto;

        public async Task<IReadOnlyList<Festivo>> ObtenerTodosAsync() =>
            await _contexto.Festivos
                .AsNoTracking()
                .Include(f => f.Pais)
                .Include(f => f.Tipo)
                .OrderBy(f => f.Id)
                .ToListAsync();

        public async Task<IReadOnlyList<Festivo>> ObtenerPorPaisAsync(int idPais) =>
            await _contexto.Festivos
                .AsNoTracking()
                .Include(f => f.Pais)
                .Include(f => f.Tipo)
                .Where(f => f.IdPais == idPais)
                .OrderBy(f => f.Id)
                .ToListAsync();

        public async Task<Festivo?> ObtenerPorIdAsync(int id) =>
            await _contexto.Festivos
                .Include(f => f.Pais)
                .Include(f => f.Tipo)
                .FirstOrDefaultAsync(f => f.Id == id);

        public async Task<bool> ExisteNombreAsync(int idPais, string nombre, int? excluirId = null)
        {
            var buscado = nombre.ToLower();
            return await _contexto.Festivos.AnyAsync(f =>
                f.IdPais == idPais &&
                f.Nombre.ToLower() == buscado &&
                (excluirId == null || f.Id != excluirId));
        }

        public async Task<Festivo> AgregarAsync(Festivo festivo)
        {
            _contexto.Festivos.Add(festivo);
            await _contexto.SaveChangesAsync();
            return festivo;
        }

        public async Task ActualizarAsync(Festivo festivo)
        {
            // Las entidades obtenidas por el repositorio ya están rastreadas: basta con guardar.
            if (_contexto.Entry(festivo).State == EntityState.Detached)
                _contexto.Festivos.Update(festivo);

            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(Festivo festivo)
        {
            _contexto.Festivos.Remove(festivo);
            await _contexto.SaveChangesAsync();
        }
    }
}
