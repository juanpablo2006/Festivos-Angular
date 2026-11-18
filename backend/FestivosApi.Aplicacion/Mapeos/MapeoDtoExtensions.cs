using FestivosApi.Dominio.DTOs;
using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Aplicacion.Mapeos
{
    internal static class MapeoDtoExtensions
    {
        public static PaisDto ToDto(this Pais p) => new(p.Id, p.Nombre);

        public static TipoFestivoDto ToDto(this TipoFestivo t) => new(t.Id, t.Tipo);

        /// <summary>Usa los nombres de país y tipo ya conocidos por el llamador (no depende de propiedades de navegación).</summary>
        public static FestivoDto ToDto(this Festivo f, string pais, string tipo) =>
            new(f.Id, f.IdPais, pais, f.Nombre, f.Dia, f.Mes, f.DiasPascua, f.IdTipo, tipo);

        /// <summary>Usa las propiedades de navegación (el repositorio las carga con Include).</summary>
        public static FestivoDto ToDto(this Festivo f) =>
            f.ToDto(f.Pais?.Nombre ?? string.Empty, f.Tipo?.Tipo ?? string.Empty);
    }
}
