namespace FestivosApi.Dominio.Constantes
{
    /// <summary>
    /// Identificadores de los tipos de festivo. Cada tipo determina la regla de cálculo de la fecha.
    /// </summary>
    public static class TipoFestivoId
    {
        /// <summary>Fecha fija (día y mes), sin traslado.</summary>
        public const int Fijo = 1;

        /// <summary>Fecha fija (día y mes) que se traslada al lunes siguiente (Ley de puente festivo).</summary>
        public const int LeyPuente = 2;

        /// <summary>Desplazamiento en días respecto al Domingo de Pascua, sin traslado.</summary>
        public const int Pascua = 3;

        /// <summary>Desplazamiento respecto al Domingo de Pascua, trasladado al lunes siguiente.</summary>
        public const int PascuaLeyPuente = 4;
    }
}
