using FestivosApi.Aplicacion.Mapeos;
using FestivosApi.Core.Excepciones;
using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Core.Interfaces.Servicios;
using FestivosApi.Core.Logica;
using FestivosApi.Dominio.Constantes;
using FestivosApi.Dominio.DTOs;
using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Aplicacion.Servicios
{
    public class FestivoServicio : IFestivoServicio
    {
        private readonly IFestivoRepositorio _festivos;
        private readonly IPaisRepositorio _paises;
        private readonly ITipoFestivoRepositorio _tipos;

        public FestivoServicio(IFestivoRepositorio festivos, IPaisRepositorio paises, ITipoFestivoRepositorio tipos)
        {
            _festivos = festivos;
            _paises = paises;
            _tipos = tipos;
        }

        // =====================================================================
        // CRUD
        // =====================================================================

        public async Task<IReadOnlyList<FestivoDto>> ObtenerTodosAsync() =>
            (await _festivos.ObtenerTodosAsync()).Select(f => f.ToDto()).ToList();

        public async Task<FestivoDto> ObtenerPorIdAsync(int id) =>
            (await ObtenerEntidadAsync(id)).ToDto();

        public async Task<FestivoDto> CrearAsync(FestivoRequest request)
        {
            var (pais, tipo, nombre) = await ValidarAsync(request, excluirId: null);

            // Solo se asignan las claves foráneas: las propiedades de navegación se dejan nulas a propósito.
            var festivo = await _festivos.AgregarAsync(new Festivo
            {
                IdPais = request.IdPais,
                Nombre = nombre,
                Dia = request.Dia,
                Mes = request.Mes,
                DiasPascua = request.DiasPascua,
                IdTipo = request.IdTipo
            });

            return festivo.ToDto(pais.Nombre, tipo.Tipo);
        }

        public async Task<FestivoDto> ActualizarAsync(int id, FestivoRequest request)
        {
            var festivo = await ObtenerEntidadAsync(id);
            var (pais, tipo, nombre) = await ValidarAsync(request, excluirId: id);

            festivo.IdPais = request.IdPais;
            festivo.Nombre = nombre;
            festivo.Dia = request.Dia;
            festivo.Mes = request.Mes;
            festivo.DiasPascua = request.DiasPascua;
            festivo.IdTipo = request.IdTipo;

            await _festivos.ActualizarAsync(festivo);
            return festivo.ToDto(pais.Nombre, tipo.Tipo);
        }

        public async Task EliminarAsync(int id)
        {
            var festivo = await ObtenerEntidadAsync(id);
            await _festivos.EliminarAsync(festivo);
        }

        // =====================================================================
        // Consultas de fechas
        // =====================================================================

        public async Task<VerificacionFestivoDto> VerificarAsync(int idPais, int anio, int mes, int dia)
        {
            CalculadorFestivos.ValidarAnio(anio);

            if (mes < 1 || mes > 12 || dia < 1 || dia > DateTime.DaysInMonth(anio, mes))
                throw new ReglaNegocioException($"La fecha {anio}-{mes}-{dia} no es válida.");

            var fecha = new DateOnly(anio, mes, dia);
            var delAnio = await ObtenerFestivosDelAnioAsync(idPais, anio);

            var coincidencias = delAnio
                .Where(f => f.Fecha == fecha)
                .Select(f => f.Nombre)
                .ToList();

            var fechaTexto = fecha.ToString("yyyy-MM-dd");
            var esFestivo = coincidencias.Count > 0;
            var mensaje = esFestivo
                ? $"El {fechaTexto} es festivo: {string.Join(", ", coincidencias)}."
                : $"El {fechaTexto} no es festivo.";

            return new VerificacionFestivoDto(fecha, esFestivo, coincidencias, mensaje);
        }

        public async Task<IReadOnlyList<FestivoFechaDto>> ObtenerFestivosDelAnioAsync(int idPais, int anio)
        {
            CalculadorFestivos.ValidarAnio(anio);

            if (!await _paises.ExisteAsync(idPais))
                throw new EntidadNoEncontradaException($"No existe un país con id {idPais}.");

            var festivos = await _festivos.ObtenerPorPaisAsync(idPais);

            return festivos
                .Select(f => new FestivoFechaDto(
                    f.Id,
                    f.Nombre,
                    CalculadorFestivos.CalcularFecha(f, anio),
                    f.IdTipo,
                    f.Tipo?.Tipo ?? string.Empty))
                .OrderBy(f => f.Fecha)
                .ThenBy(f => f.Nombre, StringComparer.Ordinal)
                .ToList();
        }

        // =====================================================================
        // Internos
        // =====================================================================

        private async Task<Festivo> ObtenerEntidadAsync(int id) =>
            await _festivos.ObtenerPorIdAsync(id)
                ?? throw new EntidadNoEncontradaException($"No existe un festivo con id {id}.");

        /// <summary>
        /// Valida referencias y coherencia entre el tipo y los campos de fecha.
        /// Tipos 1 y 2: Dia y Mes obligatorios, DiasPascua prohibido.
        /// Tipos 3 y 4: DiasPascua obligatorio, Dia y Mes prohibidos.
        /// </summary>
        private async Task<(Pais Pais, TipoFestivo Tipo, string Nombre)> ValidarAsync(FestivoRequest r, int? excluirId)
        {
            var nombre = r.Nombre?.Trim();
            if (string.IsNullOrEmpty(nombre))
                throw new ReglaNegocioException("El nombre del festivo es obligatorio.");

            var pais = await _paises.ObtenerPorIdAsync(r.IdPais)
                ?? throw new ReglaNegocioException($"El país con id {r.IdPais} no existe.");

            var tipo = await _tipos.ObtenerPorIdAsync(r.IdTipo)
                ?? throw new ReglaNegocioException($"El tipo de festivo con id {r.IdTipo} no existe.");

            switch (r.IdTipo)
            {
                case TipoFestivoId.Fijo:
                case TipoFestivoId.LeyPuente:
                    if (r.Dia is null || r.Mes is null)
                        throw new ReglaNegocioException(
                            $"Los festivos de tipo '{tipo.Tipo}' requieren 'dia' y 'mes'.");
                    if (r.DiasPascua is not null)
                        throw new ReglaNegocioException(
                            $"Los festivos de tipo '{tipo.Tipo}' no deben tener 'diasPascua'.");
                    // Se valida contra un año no bisiesto: un festivo el 29 de febrero no existiría todos los años.
                    if (r.Mes < 1 || r.Mes > 12 || r.Dia < 1 || r.Dia > DateTime.DaysInMonth(2023, r.Mes.Value))
                        throw new ReglaNegocioException(
                            $"El día {r.Dia} y el mes {r.Mes} no forman una fecha válida (se valida sobre un año no bisiesto).");
                    break;

                case TipoFestivoId.Pascua:
                case TipoFestivoId.PascuaLeyPuente:
                    if (r.DiasPascua is null)
                        throw new ReglaNegocioException(
                            $"Los festivos de tipo '{tipo.Tipo}' requieren 'diasPascua'.");
                    if (r.Dia is not null || r.Mes is not null)
                        throw new ReglaNegocioException(
                            $"Los festivos de tipo '{tipo.Tipo}' no deben tener 'dia' ni 'mes'.");
                    break;

                default:
                    throw new ReglaNegocioException($"El tipo de festivo {r.IdTipo} no está soportado.");
            }

            if (await _festivos.ExisteNombreAsync(r.IdPais, nombre, excluirId))
                throw new ConflictoException($"El país '{pais.Nombre}' ya tiene un festivo llamado '{nombre}'.");

            return (pais, tipo, nombre);
        }
    }
}
