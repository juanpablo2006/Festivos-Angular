using System.Reflection;
using Microsoft.OpenApi.Models;

namespace FestivosApi.Presentacion.Configuracion
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AgregarSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opciones =>
            {
                opciones.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API de Festivos",
                    Version = "v1",
                    Description =
                        "Consulta y administración de festivos por país. Soporta festivos de fecha fija, " +
                        "con traslado al lunes (Ley de puente festivo) y basados en el Domingo de Pascua."
                });

                var xml = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(xml))
                    opciones.IncludeXmlComments(xml);
            });
            return services;
        }

        /// <summary>Publica Swagger UI en la raíz del sitio ("/").</summary>
        public static WebApplication UsarSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opciones =>
            {
                opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Festivos v1");
                opciones.RoutePrefix = string.Empty;
                opciones.DocumentTitle = "API de Festivos";
            });
            return app;
        }
    }
}
