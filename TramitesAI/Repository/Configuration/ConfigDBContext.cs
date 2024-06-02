using Microsoft.EntityFrameworkCore;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.Repository.Configuration
{
    public class ConfigDBContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public ConfigDBContext(DbContextOptions<ConfigDBContext> options) : base(options)
        {
        }

        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Dato> Datos { get; set; }
        public DbSet<Respuesta> Respuestas { get; set; }
        public DbSet<Solicitud> Solicitudes { get; set; }
        public DbSet<SolicitudProcesada> SolicitudesProcesadas { get; set; }
        public DbSet<Tramite> Tramites { get; set; }
        public DbSet<TramiteArchivo> TramiteArchivos { get; set; }
        public DbSet<TramiteDato> TramiteDatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TramiteArchivo>()
                .HasKey(ta => new { ta.TramiteId, ta.ArchivoId });

            modelBuilder.Entity<TramiteArchivo>()
                .HasOne(ta => ta.Tramite)
                .WithMany(t => t.TramiteArchivos)
                .HasForeignKey(ta => ta.TramiteId);

            modelBuilder.Entity<TramiteArchivo>()
                .HasOne(ta => ta.Archivo)
                .WithMany(a => a.TramiteArchivos)
                .HasForeignKey(ta => ta.ArchivoId);

            modelBuilder.Entity<TramiteDato>()
                .HasKey(td => new { td.TramiteId, td.DatoId });

            modelBuilder.Entity<TramiteDato>()
                .HasOne(td => td.Tramite)
                .WithMany(t => t.TramiteDatos)
                .HasForeignKey(td => td.TramiteId);

            modelBuilder.Entity<TramiteDato>()
                .HasOne(td => td.Dato)
                .WithMany(d => d.TramiteDatos)
                .HasForeignKey(td => td.DatoId);

            modelBuilder.Entity<SolicitudProcesada>()
                .HasOne(sp => sp.Solicitud)
                .WithMany()
                .HasForeignKey(sp => sp.SolicitudId);

            modelBuilder.Entity<SolicitudProcesada>()
                .HasOne(sp => sp.Respuesta)
                .WithMany()
                .HasForeignKey(sp => sp.RespuestaId);         

            base.OnModelCreating(modelBuilder);
        }
    }
}
