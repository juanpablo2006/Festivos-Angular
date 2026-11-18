using FestivosApi.Dominio.Constantes;
using FestivosApi.Dominio.Entidades;

namespace FestivosApi.Dominio.Semillas
{
    /// <summary>
    /// Datos iniciales: 4 tipos de festivo, el país Colombia y sus 19 festivos.
    /// Cada método devuelve instancias nuevas para evitar compartir estado mutable entre contextos o pruebas.
    /// </summary>
    public static class DatosColombia
    {
        public const int IdPais = 1;

        public static List<TipoFestivo> CrearTipos() => new()
        {
            new TipoFestivo { Id = TipoFestivoId.Fijo, Tipo = "Fijo" },
            new TipoFestivo { Id = TipoFestivoId.LeyPuente, Tipo = "Ley de Puente festivo" },
            new TipoFestivo { Id = TipoFestivoId.Pascua, Tipo = "Basado en el domingo de pascua" },
            new TipoFestivo { Id = TipoFestivoId.PascuaLeyPuente, Tipo = "Basado en el domingo de pascua y Ley de Puente festivo" }
        };

        public static List<Pais> CrearPaises() => new()
        {
            new Pais { Id = IdPais, Nombre = "Colombia" }
        };

        public static List<Festivo> CrearFestivos() => new()
        {
            new Festivo { Id = 1, IdPais = IdPais, Nombre = "Año nuevo", Dia = 1, Mes = 1, DiasPascua = null, IdTipo = 1 },
            new Festivo { Id = 2, IdPais = IdPais, Nombre = "Santos Reyes", Dia = 6, Mes = 1, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 3, IdPais = IdPais, Nombre = "San José", Dia = 19, Mes = 3, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 4, IdPais = IdPais, Nombre = "Jueves Santo", Dia = null, Mes = null, DiasPascua = -3, IdTipo = 3 },
            new Festivo { Id = 5, IdPais = IdPais, Nombre = "Viernes Santo", Dia = null, Mes = null, DiasPascua = -2, IdTipo = 3 },
            new Festivo { Id = 6, IdPais = IdPais, Nombre = "Domingo de Pascua", Dia = null, Mes = null, DiasPascua = 0, IdTipo = 3 },
            new Festivo { Id = 7, IdPais = IdPais, Nombre = "Día del Trabajo", Dia = 1, Mes = 5, DiasPascua = null, IdTipo = 1 },
            new Festivo { Id = 8, IdPais = IdPais, Nombre = "Ascensión del Señor", Dia = null, Mes = null, DiasPascua = 40, IdTipo = 4 },
            new Festivo { Id = 9, IdPais = IdPais, Nombre = "Corpus Christi", Dia = null, Mes = null, DiasPascua = 61, IdTipo = 4 },
            new Festivo { Id = 10, IdPais = IdPais, Nombre = "Sagrado Corazón de Jesús", Dia = null, Mes = null, DiasPascua = 68, IdTipo = 4 },
            new Festivo { Id = 11, IdPais = IdPais, Nombre = "San Pedro y San Pablo", Dia = 29, Mes = 6, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 12, IdPais = IdPais, Nombre = "Independencia Colombia", Dia = 20, Mes = 7, DiasPascua = null, IdTipo = 1 },
            new Festivo { Id = 13, IdPais = IdPais, Nombre = "Batalla de Boyacá", Dia = 7, Mes = 8, DiasPascua = null, IdTipo = 1 },
            new Festivo { Id = 14, IdPais = IdPais, Nombre = "Asunción de la Virgen", Dia = 15, Mes = 8, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 15, IdPais = IdPais, Nombre = "Día de la Raza", Dia = 12, Mes = 10, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 16, IdPais = IdPais, Nombre = "Todos los santos", Dia = 1, Mes = 11, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 17, IdPais = IdPais, Nombre = "Independencia de Cartagena", Dia = 11, Mes = 11, DiasPascua = null, IdTipo = 2 },
            new Festivo { Id = 18, IdPais = IdPais, Nombre = "Inmaculada Concepción", Dia = 8, Mes = 12, DiasPascua = null, IdTipo = 1 },
            new Festivo { Id = 19, IdPais = IdPais, Nombre = "Navidad", Dia = 25, Mes = 12, DiasPascua = null, IdTipo = 1 }
        };
    }
}
