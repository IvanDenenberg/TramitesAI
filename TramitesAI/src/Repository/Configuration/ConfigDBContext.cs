using Microsoft.EntityFrameworkCore;
using TramitesAI.src.Repository.Domain.Entidades;

namespace TramitesAI.src.Repository.Configuration
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

            modelBuilder.Entity<Tramite>().HasData(
                new Tramite { Id = 1, Nombre = "Denuncia Siniestro" },
                new Tramite { Id = 2, Nombre = "Cotizar Poliza Auto" },
                new Tramite { Id = 3, Nombre = "Carga Presupuestos" },
                new Tramite { Id = 4, Nombre = "Cotizar Poliza Hogar" }
            );

            modelBuilder.Entity<Archivo>().HasData(
                new Archivo { Id = 1, Nombre = "Denuncia Policial" },
                new Archivo { Id = 2, Nombre = "Carga Presupuestos" }
            );

            modelBuilder.Entity<TramiteArchivo>().HasData(
                new TramiteArchivo { TramiteId = 1, ArchivoId = 1 },
                new TramiteArchivo { TramiteId = 3, ArchivoId = 2 }
            );

            modelBuilder.Entity<Dato>().HasData(
            new Dato { Id = 1, Nombre = "Marca" },
            new Dato { Id = 2, Nombre = "Modelo" },
            new Dato { Id = 3, Nombre = "Año" },
            new Dato { Id = 4, Nombre = "Cod_Post" },
            new Dato { Id = 5, Nombre = "Numero_siniestro" },
            new Dato { Id = 6, Nombre = "Tipo_inmueble" },
            new Dato { Id = 7, Nombre = "Direccion" },
            new Dato { Id = 8, Nombre = "cod_Post" },
            new Dato { Id = 9, Nombre = "Superficie" },
            new Dato { Id = 10, Nombre = "Rejas" },
            new Dato { Id = 11, Nombre = "fecha_siniestro" },
            new Dato { Id = 12, Nombre = "Denunciante" },
            new Dato { Id = 13, Nombre = "Motivo" },
            new Dato { Id = 14, Nombre = "Ubicación" },
            new Dato { Id = 15, Nombre = "Comentarios" },
            new Dato { Id = 16, Nombre = "fecha_presupuesto" },
            new Dato { Id = 17, Nombre = "Total_presupuesto" },
            new Dato { Id = 18, Nombre = "items_presupuesto" },
            new Dato { Id = 19, Nombre = "Comentarios" }
        );

            modelBuilder.Entity<TramiteDato>().HasData(
            new TramiteDato { DatoId = 1, TramiteId = 2 },
            new TramiteDato { DatoId = 2, TramiteId = 2 },
            new TramiteDato { DatoId = 3, TramiteId = 2 },
            new TramiteDato { DatoId = 4, TramiteId = 2 },
            new TramiteDato { DatoId = 6, TramiteId = 4 },
            new TramiteDato { DatoId = 7, TramiteId = 4 },
            new TramiteDato { DatoId = 8, TramiteId = 4 },
            new TramiteDato { DatoId = 9, TramiteId = 4 },
            new TramiteDato { DatoId = 10, TramiteId = 4 },
            new TramiteDato { DatoId = 11, TramiteId = 1 },
            new TramiteDato { DatoId = 12, TramiteId = 1 },
            new TramiteDato { DatoId = 13, TramiteId = 1 },
            new TramiteDato { DatoId = 14, TramiteId = 1 },
            new TramiteDato { DatoId = 15, TramiteId = 1 },
            new TramiteDato { DatoId = 16, TramiteId = 3 },
            new TramiteDato { DatoId = 17, TramiteId = 3 },
            new TramiteDato { DatoId = 18, TramiteId = 3 },
            new TramiteDato { DatoId = 19, TramiteId = 3 },
            new TramiteDato { DatoId = 5, TramiteId = 3 }
        );

            base.OnModelCreating(modelBuilder);
        }
    }
}
