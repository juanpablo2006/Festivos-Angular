using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Dominio.Entidades;
using FestivosApi.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;
namespace FestivosApi.Infraestructura.Repositorios
{
    public class TipoFestivoRepositorio : ITipoFestivoRepositorio
    {
        private readonly ApplicationDbContext _contexto;
        public TipoFestivoRepositorio(ApplicationDbContext contexto) => _contexto = contexto;
        public async Task<IReadOnlyList<TipoFestivo>> ObtenerTodosAsync() =>
            await _contexto.TiposFestivo.AsNoTracking().OrderBy(t => t.Id).ToListAsync();
        public async Task<TipoFestivo?> ObtenerPorIdAsync(int id) =>
            await _contexto.TiposFestivo.FirstOrDefaultAsync(t => t.Id == id);
        public Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null) =>
            _contexto.TiposFestivo.AnyAsync(t => t.Tipo.ToLower() == nombre.ToLower() && (!excluirId.HasValue || t.Id != excluirId));
        public Task<bool> TieneFestivosAsync(int id) => _contexto.Festivos.AnyAsync(f => f.IdTipo == id);
        public async Task<TipoFestivo> AgregarAsync(TipoFestivo tipo) {
            _contexto.TiposFestivo.Add(tipo); await _contexto.SaveChangesAsync(); return tipo;
        }
        public async Task ActualizarAsync(TipoFestivo tipo) {
            await _contexto.SaveChangesAsync();
        }
        public async Task EliminarAsync(TipoFestivo tipo) {
            _contexto.TiposFestivo.Remove(tipo); await _contexto.SaveChangesAsync();
        }
    }
}
