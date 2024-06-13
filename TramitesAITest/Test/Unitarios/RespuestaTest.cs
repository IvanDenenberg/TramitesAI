namespace TramitesAI.Test.Unit
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

    public class RespuestaRepositorioTests : IDisposable
    {
        private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

        public RespuestaRepositorioTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                .UseInMemoryDatabase(databaseName: "TestRespuestasDatabase")
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
        public async Task Borrar_RespuestaExistente_DevuelveRespuesta()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var respuesta = new Respuesta { Id = 1, MensajeRespuesta = "Respuesta1" };
                context.Respuestas.Add(respuesta);
                await context.SaveChangesAsync();

                var repositorio = new RespuestaRepositorio(context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(respuesta, resultado);
            }
        }

        [Fact]
        public async Task Borrar_RespuestaNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new RespuestaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }
        }

        [Fact]
        public async Task Crear_RespuestaNuevo_GuardaRespuesta()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new RespuestaRepositorio(context);
                var respuesta = new Respuesta { Id = 2, MensajeRespuesta = "Respuesta2" };

                // Act
                var resultado = await repositorio.Crear(respuesta);

                // Assert
                Assert.Equal(1, resultado);
                Assert.Equal(respuesta, await context.Respuestas.FindAsync(respuesta.Id));
            }
        }

        [Fact]
        public async Task LeerPorId_RespuestaExistente_DevuelveRespuesta()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var respuesta = new Respuesta { Id = 3, MensajeRespuesta = "Respuesta3" };
                context.Respuestas.Add(respuesta);
                await context.SaveChangesAsync();

                var repositorio = new RespuestaRepositorio(context);

                // Act
                var resultado = await repositorio.LeerPorId(3);

                // Assert
                Assert.Equal(respuesta, resultado);
            }
        }

        [Fact]
        public async Task LeerPorId_RespuestaNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new RespuestaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }
        }

        [Fact]
        public async Task LeerTodos_DevuelveListaDeRespuestas()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var respuesta1 = new Respuesta { Id = 4, MensajeRespuesta = "Respuesta4" };
                var respuesta2 = new Respuesta { Id = 5, MensajeRespuesta = "Respuesta5" };
                context.Respuestas.AddRange(respuesta1, respuesta2);
                await context.SaveChangesAsync();

                var repositorio = new RespuestaRepositorio(context);

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
        public async Task Modificar_RespuestaExistente_ActualizaRespuesta()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var respuesta = new Respuesta { Id = 6, MensajeRespuesta = "Original" };
                context.Respuestas.Add(respuesta);
                await context.SaveChangesAsync();

                var repositorio = new RespuestaRepositorio(context);
                respuesta.MensajeRespuesta = "Modificado";

                // Act
                var resultado = await repositorio.Modificar(respuesta);

                // Assert
                Assert.Equal("Modificado", resultado.MensajeRespuesta);
            }
        }

        [Fact]
        public async Task Modificar_RespuestaNull_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new RespuestaRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }
}