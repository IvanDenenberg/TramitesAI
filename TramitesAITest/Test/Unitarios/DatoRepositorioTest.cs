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

    public class DatoRepositorioTests : IDisposable
    {
        private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

        public DatoRepositorioTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatosDatabase")
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
        public async Task Borrar_DatoExistente_DevuelveDato()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var dato = new Dato { Id = 1, Nombre = "Dato1" };
                context.Datos.Add(dato);
                await context.SaveChangesAsync();

                var repositorio = new DatoRepositorio(context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(dato, resultado);
            }
        }

        [Fact]
        public async Task Borrar_DatoNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new DatoRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }
        }

        [Fact]
        public async Task Crear_DatoNuevo_GuardaDato()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new DatoRepositorio(context);
                var dato = new Dato { Id = 2, Nombre = "Dato2" };

                // Act
                var resultado = await repositorio.Crear(dato);

                // Assert
                Assert.Equal(1, resultado);
                Assert.Equal(dato, await context.Datos.FindAsync(dato.Id));
            }
        }

        [Fact]
        public async Task LeerPorId_DatoExistente_DevuelveDato()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var dato = new Dato { Id = 3, Nombre = "Dato3" };
                context.Datos.Add(dato);
                await context.SaveChangesAsync();

                var repositorio = new DatoRepositorio(context);

                // Act
                var resultado = await repositorio.LeerPorId(3);

                // Assert
                Assert.Equal(dato, resultado);
            }
        }

        [Fact]
        public async Task LeerPorId_DatoNoExistente_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new DatoRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }
        }

        [Fact]
        public async Task LeerTodos_DevuelveListaDeDatos()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var dato1 = new Dato { Id = 4, Nombre = "Dato4" };
                var dato2 = new Dato { Id = 5, Nombre = "Dato5" };
                context.Datos.AddRange(dato1, dato2);
                await context.SaveChangesAsync();

                var repositorio = new DatoRepositorio(context);

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
        public async Task Modificar_DatoExistente_ActualizaDato()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var dato = new Dato { Id = 6, Nombre = "Original" };
                context.Datos.Add(dato);
                await context.SaveChangesAsync();

                var repositorio = new DatoRepositorio(context);
                dato.Nombre = "Modificado";

                // Act
                var resultado = await repositorio.Modificar(dato);

                // Assert
                Assert.Equal("Modificado", resultado.Nombre);
            }
        }

        [Fact]
        public async Task Modificar_DatoNull_LanzaApiException()
        {
            using (var context = CreateContext())
            {
                // Arrange
                var repositorio = new DatoRepositorio(context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }
}