﻿using Microsoft.EntityFrameworkCore;
using TramitesAI.Repository.Domain.Dto;

namespace TramitesAI.Repository.Configuration
{
    public class ConfigDBContext : DbContext
    {
        public IConfiguration _config { get; set; }

        public ConfigDBContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection").Replace("SERVERNAME", "LAPTOP-C56DTAB8\\MSSQLSERVER01)"));
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
                .WithOne(s => s.SolicitudProcesada)
                .HasForeignKey<Solicitud>(s => s.Id);

            modelBuilder.Entity<SolicitudProcesada>()
                .HasOne(sp => sp.Respuesta)
                .WithOne(s => s.SolicitudProcesada)
                .HasForeignKey<Respuesta>(r => r.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
