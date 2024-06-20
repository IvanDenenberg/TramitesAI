namespace TramitesAITest.Test.Unitarios
{
    using global::TramitesAI.src.Common.Exceptions;
    using global::TramitesAI.src.Repository.Configuration;
    using global::TramitesAI.src.Repository.Domain.Entidades;
    using global::TramitesAI.src.Repository.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class SolicitudProcesadaRepositorioTests : IDisposable
    {
        private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

        public SolicitudProcesadaRepositorioTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                .UseInMemoryDatabase(databaseName: "TestSolicitudProcesadaDatabase")
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
        public async Task Borrar_SolicitudProcesadaExistente_DevuelveSolicitudProcesada()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(1).Canal("123").Email("123").MsgId("123").Build();

                context.SolicitudesProcesadas.Add(solicitudProcesada);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudProcesadaRepositorio(context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(solicitudProcesada, resultado);
            }
        }

        [Fact]
        public async Task Borrar_SolicitudProcesadaNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudProcesadaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }
        }

        [Fact]
        public async Task Crear_SolicitudProcesadaNuevo_GuardaSolicitudProcesada()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudProcesadaRepositorio(context);
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(2).Canal("123").Email("123").MsgId("123").Build();

                // Act
                var resultado = await repositorio.Crear(solicitudProcesada);

                // Assert
                Assert.Equal(2, resultado);
                Assert.Equal(solicitudProcesada, await context.SolicitudesProcesadas.FindAsync(solicitudProcesada.Id));
            }
        }

        [Fact]
        public async Task LeerPorId_SolicitudProcesadaExistente_DevuelveSolicitudProcesada()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(3).Canal("123").Email("123").MsgId("123").Solicitud(new Solicitud { MensajeSolicitud = "123" }).
                    Build();
                solicitudProcesada.Respuesta = new Respuesta { MensajeRespuesta = "123"};
                context.SolicitudesProcesadas.Add(solicitudProcesada);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudProcesadaRepositorio(context);

                // Act
                var resultado = await repositorio.LeerPorId(solicitudProcesada.Id);

                // Assert
                Assert.Equal(solicitudProcesada, resultado);
            }
        }

        [Fact]
        public async Task LeerPorId_SolicitudProcesadaNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudProcesadaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }
        }

        [Fact]
        public async Task LeerTodos_DevuelveListaDeSolicitudProcesadaes()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitudProcesada1 = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(4).Canal("123").Email("123").MsgId("123").Solicitud(new Solicitud { MensajeSolicitud = "123" }).Build();
                solicitudProcesada1.Respuesta = new Respuesta { MensajeRespuesta = "123" };
                var solicitudProcesada2 = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(5).Canal("123").Email("123").MsgId("123").Solicitud(new Solicitud { MensajeSolicitud = "123" }).Build();
                solicitudProcesada2.Respuesta = new Respuesta { MensajeRespuesta = "123" };
                context.SolicitudesProcesadas.AddRange(solicitudProcesada1, solicitudProcesada2);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudProcesadaRepositorio(context);

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
        public async Task Modificar_SolicitudProcesadaExistente_ActualizaSolicitudProcesada()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(6).Canal("123").Email("123").MsgId("123").Build();
                context.SolicitudesProcesadas.Add(solicitudProcesada);
                await context.SaveChangesAsync();

                var repositorio = new SolicitudProcesadaRepositorio(context);
                solicitudProcesada.TramiteId = 2;

                // Act
                var resultado = await repositorio.Modificar(solicitudProcesada);

                // Assert
                Assert.Equal(2, resultado.TramiteId);
            }
        }

        [Fact]
        public async Task Modificar_SolicitudProcesadaNull_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new SolicitudProcesadaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }
}