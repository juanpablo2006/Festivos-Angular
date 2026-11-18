using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Dominio.Entidades;
using FestivosApi.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;

namespace FestivosApi.Infraestructura.Repositorios
{
    public class PaisRepositorio : IPaisRepositorio
    {
        private readonly ApplicationDbContext _contexto;

        public PaisRepositorio(ApplicationDbContext contexto) => _contexto = contexto;

        public async Task<IReadOnlyList<Pais>> ObtenerTodosAsync() =>
            await _contexto.Paises.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

        public async Task<Pais?> ObtenerPorIdAsync(int id) =>
            await _contexto.Paises.FirstOrDefaultAsync(p => p.Id == id);

        public async Task<bool> ExisteAsync(int id) =>
            await _contexto.Paises.AnyAsync(p => p.Id == id);

        public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null)
        {
            var buscado = nombre.ToLower();
            return await _contexto.Paises.AnyAsync(p =>
                p.Nombre.ToLower() == buscado && (excluirId == null || p.Id != excluirId));
        }

        public async Task<bool> TieneFestivosAsync(int id) =>
            await _contexto.Festivos.AnyAsync(f => f.IdPais == id);

        public async Task<Pais> AgregarAsync(Pais pais)
        {
            _contexto.Paises.Add(pais);
            await _contexto.SaveChangesAsync();
            return pais;
        }

        public async Task ActualizarAsync(Pais pais)
        {
            // Las entidades obtenidas por el repositorio ya están rastreadas: basta con guardar.
            if (_contexto.Entry(pais).State == EntityState.Detached)
                _contexto.Paises.Update(pais);

            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(Pais pais)
        {
            _contexto.Paises.Remove(pais);
            await _contexto.SaveChangesAsync();
        }
    }
}
