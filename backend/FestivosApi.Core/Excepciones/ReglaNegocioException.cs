namespace FestivosApi.Core.Excepciones
{
    /// <summary>Los datos recibidos incumplen una regla de negocio (se traduce a HTTP 400).</summary>
    public class ReglaNegocioException : Exception
    {
        public ReglaNegocioException(string mensaje) : base(mensaje) { }
    }
}
