using FestivosApi.Aplicacion.Mapeos;
using FestivosApi.Core.Excepciones;
using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Dominio.DTOs;
using FestivosApi.Dominio.Entidades;
namespace FestivosApi.Aplicacion.Servicios
{
    public class TipoFestivoServicio : ITipoFestivoServicio
    {
        private readonly ITipoFestivoRepositorio _tipos;
        public TipoFestivoServicio(ITipoFestivoRepositorio tipos) => _tipos = tipos;
        public async Task<IReadOnlyList<TipoFestivoDto>> ObtenerTodosAsync() =>
            (await _tipos.ObtenerTodosAsync()).Select(t => t.ToDto()).ToList();
        private async Task<TipoFestivo> Entidad(int id) =>
            await _tipos.ObtenerPorIdAsync(id) ?? throw new EntidadNoEncontradaException($"No existe un tipo de festivo con id {id}.");
        public async Task<TipoFestivoDto> ObtenerPorIdAsync(int id) => (await Entidad(id)).ToDto();
        private static string Nombre(TipoFestivoRequest request) {
            var nombre = request.Tipo?.Trim();
            if (string.IsNullOrWhiteSpace(nombre)) throw new ReglaNegocioException("El nombre del tipo es obligatorio.");
            return nombre;
        }
        public async Task<TipoFestivoDto> CrearAsync(TipoFestivoRequest request) {
            var nombre = Nombre(request);
            if (await _tipos.ExisteNombreAsync(nombre)) throw new ConflictoException("Ya existe un tipo con ese nombre.");
            var nuevo = await _tipos.AgregarAsync(new TipoFestivo { Tipo = nombre });
            return nuevo.ToDto();
        }
        public async Task<TipoFestivoDto> ActualizarAsync(int id, TipoFestivoRequest request) {
            var entidad = await Entidad(id);
            var nombre = Nombre(request);
            if (await _tipos.ExisteNombreAsync(nombre, id)) throw new ConflictoException("Ya existe otro tipo con ese nombre.");
            entidad.Tipo = nombre;
            await _tipos.ActualizarAsync(entidad);
            return entidad.ToDto();
        }
        public async Task EliminarAsync(int id) {
            var entidad = await Entidad(id);
            if (id >= 1 && id <= 4) throw new ConflictoException("Los cuatro tipos oficiales son necesarios para el cálculo y no se pueden eliminar.");
            if (await _tipos.TieneFestivosAsync(id)) throw new ConflictoException("El tipo tiene festivos asociados.");
            await _tipos.EliminarAsync(entidad);
        }
    }
}
