namespace FestivosApi.Core.Excepciones
{
    /// <summary>La operación choca con el estado actual de los datos, p. ej. un duplicado (se traduce a HTTP 409).</summary>
    public class ConflictoException : Exception
    {
        public ConflictoException(string mensaje) : base(mensaje) { }
    }
}
