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

    public class TramiteRepositorioTests : IDisposable
    {
        private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

        public TramiteRepositorioTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                .UseInMemoryDatabase(databaseName: "TestTramitesDatabase")
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
        public async Task Borrar_TramiteExistente_DevuelveTramite()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var tramite = new Tramite { Id = 1, Nombre = "Tramite1" };
                context.Tramites.Add(tramite);
                await context.SaveChangesAsync();

                var repositorio = new TramiteRepositorio(context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(tramite, resultado);
            }
        }

        [Fact]
        public async Task Borrar_TramiteNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new TramiteRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }
        }

        [Fact]
        public async Task Crear_TramiteNuevo_GuardaTramite()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new TramiteRepositorio(context);
                var tramite = new Tramite { Id = 2, Nombre = "Tramite2" };

                // Act
                var resultado = await repositorio.Crear(tramite);

                // Assert
                Assert.Equal(1, resultado);
                Assert.Equal(tramite, await context.Tramites.FindAsync(tramite.Id));
            }
        }

        [Fact]
        public async Task LeerPorId_TramiteExistente_DevuelveTramite()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var tramite = new Tramite { Id = 3, Nombre = "Tramite3" };
                context.Tramites.Add(tramite);
                await context.SaveChangesAsync();

                var repositorio = new TramiteRepositorio(context);

                // Act
                var resultado = await repositorio.LeerPorId(3);

                // Assert
                Assert.Equal(tramite, resultado);
            }
        }

        [Fact]
        public async Task LeerPorId_TramiteNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new TramiteRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }
        }

        [Fact]
        public async Task LeerTodos_DevuelveListaDeTramites()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var tramite1 = new Tramite { Id = 4, Nombre = "Tramite4" };
                var tramite2 = new Tramite { Id = 5, Nombre = "Tramite5" };
                context.Tramites.AddRange(tramite1, tramite2);
                await context.SaveChangesAsync();

                var repositorio = new TramiteRepositorio(context);

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
        public async Task Modificar_TramiteExistente_ActualizaTramite()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var tramite = new Tramite { Id = 6, Nombre = "Original" };
                context.Tramites.Add(tramite);
                await context.SaveChangesAsync();

                var repositorio = new TramiteRepositorio(context);
                tramite.Nombre = "Modificado";

                // Act
                var resultado = await repositorio.Modificar(tramite);

                // Assert
                Assert.Equal("Modificado", resultado.Nombre);
            }
        }

        [Fact]
        public async Task Modificar_TramiteNull_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new TramiteRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }
}