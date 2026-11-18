using FestivosApi.Aplicacion;
using FestivosApi.Infraestructura;
using FestivosApi.Presentacion.Configuracion;
using FestivosApi.Presentacion.Middleware;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("FestivosDb")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'FestivosDb' en la configuración.");

// Capas
builder.Services.AgregarAplicacion();
builder.Services.AgregarInfraestructura(cadenaConexion);

// Web API
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy("Angular", policy =>
    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();
builder.Services.AddHealthChecks();
builder.Services.AgregarSwagger();

var app = builder.Build();

// Crea la base de datos SQLite con los datos iniciales de Colombia si no existe.
await app.Services.AsegurarBaseDeDatosAsync();

app.UseExceptionHandler();
app.UseCors("Angular");
app.UsarSwagger();           // Swagger UI en "/"
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Necesario para poder usar WebApplicationFactory en pruebas de integración.
public partial class Program { }
