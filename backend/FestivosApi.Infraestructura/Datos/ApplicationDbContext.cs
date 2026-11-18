using FestivosApi.Dominio.Entidades;
using FestivosApi.Dominio.Semillas;
using Microsoft.EntityFrameworkCore;

namespace FestivosApi.Infraestructura.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pais> Paises => Set<Pais>();
        public DbSet<TipoFestivo> TiposFestivo => Set<TipoFestivo>();
        public DbSet<Festivo> Festivos => Set<Festivo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Restricciones de columnas ----------
            modelBuilder.Entity<Pais>(e =>
            {
                e.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                e.HasIndex(p => p.Nombre).IsUnique();
            });

            modelBuilder.Entity<TipoFestivo>(e =>
            {
                e.Property(t => t.Tipo).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Festivo>(e =>
            {
                e.Property(f => f.Nombre).IsRequired().HasMaxLength(150);
            });

            // ---------- Relaciones (FKs) ----------
            // Restrict: no se permite borrar un país o un tipo que todavía tenga festivos.
            modelBuilder.Entity<Festivo>()
                .HasOne(f => f.Pais)
                .WithMany(p => p.Festivos)
                .HasForeignKey(f => f.IdPais)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Festivo>()
                .HasOne(f => f.Tipo)
                .WithMany(t => t.Festivos)
                .HasForeignKey(f => f.IdTipo)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Seed: tipos, país y festivos de Colombia ----------
            modelBuilder.Entity<TipoFestivo>().HasData(DatosColombia.CrearTipos());
            modelBuilder.Entity<Pais>().HasData(DatosColombia.CrearPaises());
            modelBuilder.Entity<Festivo>().HasData(DatosColombia.CrearFestivos());
        }
    }
}
