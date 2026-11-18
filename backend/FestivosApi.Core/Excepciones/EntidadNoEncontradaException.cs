namespace FestivosApi.Core.Excepciones
{
    /// <summary>El recurso solicitado no existe (se traduce a HTTP 404).</summary>
    public class EntidadNoEncontradaException : Exception
    {
        public EntidadNoEncontradaException(string mensaje) : base(mensaje) { }
    }
}
