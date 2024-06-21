namespace TramitesAITest.Test.Unitarios
{
    using global::TramitesAI.src.Common.Exceptions;
    using global::TramitesAI.src.Repository.Configuration;
    using global::TramitesAI.src.Repository.Domain.Entidades;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TramitesAI.src.Repositorio.Servicios.Implementaciones;
    using Xunit;

    public class SolicitudRepositorioTests : IDisposable
    {
        private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

        public SolicitudRepositorioTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                .UseInMemoryDatabase(databaseName: "TestSolicitudesDatabase")
                .Options;
        }

        private ConfigDBContext CreateContext()
        {
            return new ConfigDBContext(_dbContextOptions);
        }

        public void Dispose()
        {
            using (var context = CreateContext())
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task Borrar_SolicitudExistente_DevuelveSolicitud()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitud = new Solicitud { Id = 1, MensajeSolicitud = "Solicitud1" };
                context.Solicitudes.Add(solicitud);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudRepositorio(context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(solicitud, resultado);
            }
        }

        [Fact]
        public async Task Borrar_SolicitudNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }
        }

        [Fact]
        public async Task Crear_SolicitudNuevo_GuardaSolicitud()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudRepositorio(context);
                var solicitud = new Solicitud { Id = 2, MensajeSolicitud = "Solicitud2" };

                // Act
                var resultado = await repositorio.Crear(solicitud);

                // Assert
                Assert.Equal(2, resultado);
                Assert.Equal(solicitud, await context.Solicitudes.FindAsync(solicitud.Id));
            }
        }

        [Fact]
        public async Task LeerPorId_SolicitudExistente_DevuelveSolicitud()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitud = new Solicitud { Id = 3, MensajeSolicitud = "Solicitud3" };
                context.Solicitudes.Add(solicitud);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudRepositorio(context);

                // Act
                var resultado = await repositorio.LeerPorId(3);

                // Assert
                Assert.Equal(solicitud, resultado);
            }
        }

        [Fact]
        public async Task LeerPorId_SolicitudNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }
        }

        [Fact]
        public async Task LeerTodos_DevuelveListaDeSolicitudes()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitud1 = new Solicitud { Id = 4, MensajeSolicitud = "Solicitud4" };
                var solicitud2 = new Solicitud { Id = 5, MensajeSolicitud = "Solicitud5" };
                context.Solicitudes.AddRange(solicitud1, solicitud2);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudRepositorio(context);

                // Act
                var resultado = await repositorio.LeerTodos();

                // Assert
                var listaResultado = resultado.ToList();
                Assert.Equal(2, listaResultado.Count);
                Assert.Contains(listaResultado, a => a.Id == 4);
                Assert.Contains(listaResultado, a => a.Id == 5);
            }
        }

        [Fact]
        public async Task Modificar_SolicitudExistente_ActualizaSolicitud()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitud = new Solicitud { Id = 6, MensajeSolicitud = "Original" };
                context.Solicitudes.Add(solicitud);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudRepositorio(context);
                solicitud.MensajeSolicitud = "Modificado";

                // Act
                var resultado = await repositorio.Modificar(solicitud);

                // Assert
                Assert.Equal("Modificado", resultado.MensajeSolicitud);
            }
        }

        [Fact]
        public async Task Modificar_SolicitudNull_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }
}