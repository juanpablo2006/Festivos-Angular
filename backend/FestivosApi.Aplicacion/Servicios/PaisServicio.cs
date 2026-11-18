using FestivosApi.Aplicacion.Mapeos;
using FestivosApi.Core.Excepciones;
using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Dominio.DTOs;
using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Aplicacion.Servicios
{
    public class PaisServicio : IPaisServicio
    {
        private readonly IPaisRepositorio _paises;

        public PaisServicio(IPaisRepositorio paises) => _paises = paises;

        public async Task<IReadOnlyList<PaisDto>> ObtenerTodosAsync() =>
            (await _paises.ObtenerTodosAsync()).Select(p => p.ToDto()).ToList();

        public async Task<PaisDto> ObtenerPorIdAsync(int id) =>
            (await ObtenerEntidadAsync(id)).ToDto();

        public async Task<PaisDto> CrearAsync(PaisRequest request)
        {
            var nombre = NormalizarNombre(request.Nombre);

            if (await _paises.ExisteNombreAsync(nombre))
                throw new ConflictoException($"Ya existe un país llamado '{nombre}'.");

            var pais = await _paises.AgregarAsync(new Pais { Nombre = nombre });
            return pais.ToDto();
        }

        public async Task<PaisDto> ActualizarAsync(int id, PaisRequest request)
        {
            var pais = await ObtenerEntidadAsync(id);
            var nombre = NormalizarNombre(request.Nombre);

            if (await _paises.ExisteNombreAsync(nombre, excluirId: id))
                throw new ConflictoException($"Ya existe otro país llamado '{nombre}'.");

            pais.Nombre = nombre;
            await _paises.ActualizarAsync(pais);
            return pais.ToDto();
        }

        public async Task EliminarAsync(int id)
        {
            var pais = await ObtenerEntidadAsync(id);

            if (await _paises.TieneFestivosAsync(id))
                throw new ConflictoException(
                    $"No se puede eliminar el país '{pais.Nombre}' porque tiene festivos asociados. Elimine primero sus festivos.");

            await _paises.EliminarAsync(pais);
        }

        private async Task<Pais> ObtenerEntidadAsync(int id) =>
            await _paises.ObtenerPorIdAsync(id)
                ?? throw new EntidadNoEncontradaException($"No existe un país con id {id}.");

        private static string NormalizarNombre(string? nombre)
        {
            var limpio = nombre?.Trim();
            if (string.IsNullOrEmpty(limpio))
                throw new ReglaNegocioException("El nombre del país es obligatorio.");
            return limpio;
        }
    }
}
