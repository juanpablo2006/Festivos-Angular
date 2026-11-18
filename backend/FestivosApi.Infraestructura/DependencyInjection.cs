using FestivosApi.Core.Interfaces.Repositorios;
using FestivosApi.Infraestructura.Datos;
using FestivosApi.Infraestructura.Repositorios;
using FestivosApi.Dominio.Entidades;
using FestivosApi.Dominio.Semillas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FestivosApi.Infraestructura
{
    public static class DependencyInjection
    {
        /// <summary>Registra el contexto de EF Core (SQLite) y los repositorios.</summary>
        public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, string cadenaConexion)
        {
            services.AddDbContext<ApplicationDbContext>(opciones => opciones.UseSqlite(cadenaConexion));

            services.AddScoped<IPaisRepositorio, PaisRepositorio>();
            services.AddScoped<ITipoFestivoRepositorio, TipoFestivoRepositorio>();
            services.AddScoped<IFestivoRepositorio, FestivoRepositorio>();
            return services;
        }

        /// <summary>
        /// Crea la base de datos (con el seed de Colombia) si todavía no existe.
        /// Para un entorno productivo se recomienda sustituirlo por migraciones de EF Core (ver README).
        /// </summary>
        public static async Task AsegurarBaseDeDatosAsync(this IServiceProvider servicios)
        {
            using var scope = servicios.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await contexto.Database.EnsureCreatedAsync();

            // EnsureCreated no ejecuta HasData de nuevo cuando festivos.db ya existe.
            // Este complemento es idempotente: conserva los datos ingresados por el usuario,
            // y añade únicamente los festivos de ejemplo que todavía falten.
            var paisesExistentes = await contexto.Paises.ToListAsync();
            var datos = DatosPaisesAdicionales.CrearFestivosPorPais();
            foreach (var (nombrePais, festivosIniciales) in datos)
            {
                var pais = paisesExistentes.FirstOrDefault(p =>
                    string.Equals(p.Nombre.Trim(), nombrePais, StringComparison.OrdinalIgnoreCase));
                if (pais is null)
                {
                    pais = new Pais { Nombre = nombrePais };
                    contexto.Paises.Add(pais);
                    await contexto.SaveChangesAsync();
                    paisesExistentes.Add(pais);
                }

                var existentes = await contexto.Festivos
                    .Where(f => f.IdPais == pais.Id)
                    .ToListAsync();

                foreach (var festivo in festivosIniciales)
                {
                    // No duplicar un registro existente aunque la aplicación se reinicie.
                    if (existentes.Any(f => f.Nombre == festivo.Nombre))
                        continue;
                    festivo.IdPais = pais.Id;
                    contexto.Festivos.Add(festivo);
                }
                await contexto.SaveChangesAsync();
            }
        }
    }
}
