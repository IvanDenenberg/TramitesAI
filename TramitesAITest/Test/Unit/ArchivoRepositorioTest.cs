namespace TramitesAI.Test.Unit
{
    using global::TramitesAI.src.Common.Exceptions;
    using global::TramitesAI.src.Repository.Configuration;
    using global::TramitesAI.src.Repository.Domain.Entidades;
    using global::TramitesAI.src.Repository.Implementations;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    namespace TramitesAI.Test.Unit
    {
        public class ArchivoRepositorioTests : IDisposable
        {
            private readonly ConfigDBContext _context;
            private readonly DbContextOptions<ConfigDBContext> _dbContextOptions;

            public ArchivoRepositorioTests()
            {
                _dbContextOptions = new DbContextOptionsBuilder<ConfigDBContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabase")
                    .Options;

                _context = new ConfigDBContext(_dbContextOptions);
            }

            public void Dispose()
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }

            [Fact]
            public async Task Borrar_ArchivoExistente_DevuelveArchivo()
            {
                // Arrange
                var archivo = new Archivo { Id = 1, Nombre = "Archivo1", Obligatorio = true, TramiteArchivos = new List<TramiteArchivo>() };
                _context.Archivos.Add(archivo);
                await _context.SaveChangesAsync();

                var repositorio = new ArchivoRepositorio(_context);

                // Act
                var resultado = await repositorio.Borrar(1);

                // Assert
                Assert.Equal(archivo, resultado);
            }

            [Fact]
            public async Task Borrar_ArchivoNoExistente_LanzaApiException()
            {
                // Arrange
                var repositorio = new ArchivoRepositorio(_context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Borrar(999));
            }

            [Fact]
            public async Task Crear_ArchivoNuevo_GuardaArchivo()
            {
                // Arrange
                var repositorio = new ArchivoRepositorio(_context);
                var archivo = new Archivo { Id = 2, Nombre = "Archivo2", Obligatorio = false, TramiteArchivos = new List<TramiteArchivo>() };

                // Act
                var resultado = await repositorio.Crear(archivo);

                // Assert
                Assert.Equal(1, resultado);
                Assert.Equal(archivo, await _context.Archivos.FindAsync(archivo.Id));
            }

            [Fact]
            public async Task LeerPorId_ArchivoExistente_DevuelveArchivo()
            {
                // Arrange
                var archivo = new Archivo { Id = 3, Nombre = "Archivo3", Obligatorio = false, TramiteArchivos = new List<TramiteArchivo>() };
                _context.Archivos.Add(archivo);
                await _context.SaveChangesAsync();

                var repositorio = new ArchivoRepositorio(_context);

                // Act
                var resultado = await repositorio.LeerPorId(3);

                // Assert
                Assert.Equal(archivo, resultado);
            }

            [Fact]
            public async Task LeerPorId_ArchivoNoExistente_LanzaApiException()
            {
                // Arrange
                var repositorio = new ArchivoRepositorio(_context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.LeerPorId(999));
            }

            [Fact]
            public async Task LeerTodos_DevuelveListaDeArchivos()
            {
                // Arrange
                var archivo1 = new Archivo { Id = 4, Nombre = "Archivo4", Obligatorio = true, TramiteArchivos = new List<TramiteArchivo>() };
                var archivo2 = new Archivo { Id = 5, Nombre = "Archivo5", Obligatorio = false, TramiteArchivos = new List<TramiteArchivo>() };
                _context.Archivos.AddRange(archivo1, archivo2);
                await _context.SaveChangesAsync();

                var repositorio = new ArchivoRepositorio(_context);

                // Act
                var resultado = await repositorio.LeerTodos();

                // Assert
                var listaResultado = resultado.ToList();
                Assert.Equal(2, listaResultado.Count);
                Assert.Contains(listaResultado, a => a.Id == 4);
                Assert.Contains(listaResultado, a => a.Id == 5);
            }

            [Fact]
            public async Task Modificar_ArchivoExistente_ActualizaArchivo()
            {
                // Arrange
                var archivo = new Archivo { Id = 6, Nombre = "Original", Obligatorio = false, TramiteArchivos = new List<TramiteArchivo>() };
                _context.Archivos.Add(archivo);
                await _context.SaveChangesAsync();

                var repositorio = new ArchivoRepositorio(_context);
                archivo.Nombre = "Modificado";

                // Act
                var resultado = await repositorio.Modificar(archivo);

                // Assert
                Assert.Equal("Modificado", resultado.Nombre);
            }

            [Fact]
            public async Task Modificar_ArchivoNull_LanzaApiException()
            {
                // Arrange
                var repositorio = new ArchivoRepositorio(_context);

                // Act & Assert
                await Assert.ThrowsAsync<ApiException>(() => repositorio.Modificar(null));
            }
        }
    }

}
