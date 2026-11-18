using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Dominio.Semillas
{
    /// <summary>
    /// Festivos nacionales seleccionados de Perú y Chile, compatibles con el calculador
    /// existente (fecha fija o desplazamiento respecto del domingo de Pascua).
    /// No pretende ser un calendario jurídico exhaustivo: algunas fiestas nacionales
    /// tienen reglas de traslado que no están implementadas en los tipos 1-4.
    /// </summary>
    public static class DatosPaisesAdicionales
    {
        public static IReadOnlyDictionary<string, List<Festivo>> CrearFestivosPorPais() =>
            new Dictionary<string, List<Festivo>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Perú"] = new()
                {
                    F("Año Nuevo", 1, 1),
                    P("Jueves Santo", -3),
                    P("Viernes Santo", -2),
                    F("Día del Trabajo", 1, 5),
                    F("Batalla de Arica y Día de la Bandera", 7, 6),
                    F("San Pedro y San Pablo", 29, 6),
                    F("Día de la Fuerza Aérea del Perú", 23, 7),
                    F("Fiestas Patrias - Independencia", 28, 7),
                    F("Fiestas Patrias - Gran Parada Militar", 29, 7),
                    F("Batalla de Junín", 6, 8),
                    F("Santa Rosa de Lima", 30, 8),
                    F("Combate de Angamos", 8, 10),
                    F("Todos los Santos", 1, 11),
                    F("Inmaculada Concepción", 8, 12),
                    F("Batalla de Ayacucho", 9, 12),
                    F("Navidad", 25, 12)
                },
                ["Chile"] = new()
                {
                    F("Año Nuevo", 1, 1),
                    P("Viernes Santo", -2),
                    P("Sábado Santo", -1),
                    F("Día del Trabajo", 1, 5),
                    F("Día de las Glorias Navales", 21, 5),
                    F("Virgen del Carmen", 16, 7),
                    F("Asunción de la Virgen", 15, 8),
                    F("Independencia Nacional", 18, 9),
                    F("Día de las Glorias del Ejército", 19, 9),
                    F("Todos los Santos", 1, 11),
                    F("Inmaculada Concepción", 8, 12),
                    F("Navidad", 25, 12)
                }
            };

        private static Festivo F(string nombre, int dia, int mes) =>
            new() { Nombre = nombre, Dia = dia, Mes = mes, IdTipo = 1 };

        private static Festivo P(string nombre, int diasPascua) =>
            new() { Nombre = nombre, DiasPascua = diasPascua, IdTipo = 3 };
    }
}
