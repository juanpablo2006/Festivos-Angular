using FestivosApi.Core.Excepciones;
using FestivosApi.Dominio.Constantes;
using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Core.Logica
{
    /// <summary>
    /// Lógica pura (sin acceso a datos) para resolver la fecha de un festivo en un año dado.
    /// </summary>
    public static class CalculadorFestivos
    {
        /// <summary>Primer año del calendario gregoriano (algoritmo de Pascua válido desde aquí).</summary>
        public const int AnioMinimo = 1583;

        /// <summary>Último año soportado (deja margen para los traslados sin salirse de DateOnly).</summary>
        public const int AnioMaximo = 9998;

        /// <summary>
        /// Calcula el Domingo de Pascua (calendario gregoriano) con el algoritmo de Meeus/Jones/Butcher.
        /// </summary>
        public static DateOnly CalcularDomingoPascua(int anio)
        {
            ValidarAnio(anio);

            int a = anio % 19;
            int b = anio / 100;
            int c = anio % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int mes = (h + l - 7 * m + 114) / 31;
            int dia = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateOnly(anio, mes, dia);
        }

        /// <summary>
        /// Ley de puente festivo: si la fecha ya es lunes se mantiene; si no, pasa al lunes siguiente.
        /// </summary>
        public static DateOnly SiguienteLunes(DateOnly fecha)
        {
            int diasHastaLunes = ((int)DayOfWeek.Monday - (int)fecha.DayOfWeek + 7) % 7;
            return fecha.AddDays(diasHastaLunes);
        }

        /// <summary>
        /// Resuelve la fecha del festivo en el año indicado según su tipo:
        /// 1 = fija; 2 = fija trasladada al lunes; 3 = Pascua + días; 4 = (Pascua + días) trasladada al lunes.
        /// </summary>
        public static DateOnly CalcularFecha(Festivo festivo, int anio)
        {
            ValidarAnio(anio);

            return festivo.IdTipo switch
            {
                TipoFestivoId.Fijo => FechaFija(festivo, anio),
                TipoFestivoId.LeyPuente => SiguienteLunes(FechaFija(festivo, anio)),
                TipoFestivoId.Pascua => FechaPascua(festivo, anio),
                TipoFestivoId.PascuaLeyPuente => SiguienteLunes(FechaPascua(festivo, anio)),
                _ => throw new ReglaNegocioException(
                    $"El festivo '{festivo.Nombre}' tiene un tipo no soportado ({festivo.IdTipo}).")
            };
        }

        /// <summary>Lanza <see cref="ReglaNegocioException"/> si el año está fuera del rango soportado.</summary>
        public static void ValidarAnio(int anio)
        {
            if (anio < AnioMinimo || anio > AnioMaximo)
                throw new ReglaNegocioException(
                    $"El año debe estar entre {AnioMinimo} y {AnioMaximo}.");
        }

        private static DateOnly FechaFija(Festivo festivo, int anio)
        {
            if (festivo.Dia is null || festivo.Mes is null)
                throw new ReglaNegocioException(
                    $"El festivo '{festivo.Nombre}' es de fecha fija pero no tiene día y mes definidos.");

            try
            {
                return new DateOnly(anio, festivo.Mes.Value, festivo.Dia.Value);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ReglaNegocioException(
                    $"El festivo '{festivo.Nombre}' tiene una fecha inválida ({festivo.Dia}/{festivo.Mes}) para el año {anio}.");
            }
        }

        private static DateOnly FechaPascua(Festivo festivo, int anio)
        {
            if (festivo.DiasPascua is null)
                throw new ReglaNegocioException(
                    $"El festivo '{festivo.Nombre}' se basa en la Pascua pero no tiene 'DiasPascua' definido.");

            return CalcularDomingoPascua(anio).AddDays(festivo.DiasPascua.Value);
        }
    }
}
