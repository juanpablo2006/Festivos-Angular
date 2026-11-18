using FestivosApi.Aplicacion.Servicios;
using FestivosApi.Core.Interfaces.Servicios;
using Microsoft.Extensions.DependencyInjection;

namespace FestivosApi.Aplicacion
{
    public static class DependencyInjection
    {
        /// <summary>Registra los servicios de la capa de Aplicación.</summary>
        public static IServiceCollection AgregarAplicacion(this IServiceCollection services)
        {
            services.AddScoped<IPaisServicio, PaisServicio>();
            services.AddScoped<ITipoFestivoServicio, TipoFestivoServicio>();
            services.AddScoped<IFestivoServicio, FestivoServicio>();
            return services;
        }
    }
}
