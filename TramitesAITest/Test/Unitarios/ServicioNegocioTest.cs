using Moq;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Business.Services.Implementation;
using TramitesAI.src.Business.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repositorio.Servicios.Interfaces;
using TramitesAI.src.Repository.Domain.Entidades;
using Xunit;

namespace TramitesAITest.Test.Unitarios
{
    public class ServiceNegocioTests
    {
        private readonly Mock<IRepositorio<SolicitudProcesada>> _solicitudProcesadaRepositorioMock;
        private readonly Mock<IAIHandler> _AIHandlerMock;
        private readonly Mock<IBuscadorArchivos> _fileSearcherMock;
        private readonly Mock<IRepositorio<Solicitud>> _solicitudRepositorioMock;
        private readonly Mock<IRepositorio<Tramite>> _tramiteRepositorioMock;
        private readonly Mock<IRepositorio<Respuesta>> _respuestaRepositorioMock;
        private readonly ServiceNegocio _serviceNegocio;

        public ServiceNegocioTests()
        {
            _solicitudProcesadaRepositorioMock = new Mock<IRepositorio<SolicitudProcesada>>();
            _AIHandlerMock = new Mock<IAIHandler>();
            _fileSearcherMock = new Mock<IBuscadorArchivos>();
            _solicitudRepositorioMock = new Mock<IRepositorio<Solicitud>>();
            _tramiteRepositorioMock = new Mock<IRepositorio<Tramite>>();
            _respuestaRepositorioMock = new Mock<IRepositorio<Respuesta>>();

            _serviceNegocio = new ServiceNegocio(
                _solicitudProcesadaRepositorioMock.Object,
                _AIHandlerMock.Object,
                _fileSearcherMock.Object,
                _solicitudRepositorioMock.Object,
                _tramiteRepositorioMock.Object,
                _respuestaRepositorioMock.Object
            );
        }

        [Fact]
        public async Task LeerPorId_IdValido_DeberiaRetornarSolicitudProcesada()
        {
            // Arrange
            int id = 1;
            var expectedSolicitud = new SolicitudProcesada { Id = id };
            _solicitudProcesadaRepositorioMock.Setup(repo => repo.LeerPorId(id)).ReturnsAsync(expectedSolicitud);

            // Act
            var result = await _serviceNegocio.LeerPorId(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSolicitud, result);
        }

        [Fact]
        public async Task LeerPorId_IdInvalido_DeberiaLanzarApiException()
        {
            // Arrange
            int id = -1;

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _serviceNegocio.LeerPorId(id));
        }

        [Fact]
        public async Task ProcesarAsync_ValidSolicitudDTO_DeberiaRetornarRespuestaDTO()
        {
            // Arrange
            var solicitudDTO = new SolicitudDTO
            {
                Asunto = "test",
                Adjuntos = new List<string> { "file1", "file2" },
                MsgId = "msg123",
                Canal = "email",
                Origen = "test@example.com",
                Recibido = DateTime.Now
            };

            var solicitud = new Solicitud { Id = 1 };
            var tramite = new Tramite { Id = 1, TramiteArchivos = new List<TramiteArchivo>() };
            var solProcesada = new SolicitudProcesada { Id = 1 };
            var informacionAnalizada = new InformacionAnalizadaDTO
            {
                Campos = new Dictionary<string, object>
                {
                    { "campo1", "valor1" },
                    { "campo2", null }
                }
            };

            _solicitudProcesadaRepositorioMock.Setup(repo => repo.Crear(It.IsAny<SolicitudProcesada>())).ReturnsAsync(1);
            _solicitudRepositorioMock.Setup(repo => repo.Crear(It.IsAny<Solicitud>())).ReturnsAsync(1);
            _solicitudProcesadaRepositorioMock.Setup(repo => repo.LeerPorId(It.IsAny<int>())).ReturnsAsync(solProcesada);
            _tramiteRepositorioMock.Setup(repo => repo.LeerPorId(It.IsAny<int>())).ReturnsAsync(tramite);
            _AIHandlerMock.Setup(handler => handler.DeterminarTramiteAsync(It.IsAny<string>())).ReturnsAsync(1);
            _AIHandlerMock.Setup(handler => handler.ProcesarInformacion(It.IsAny<List<MemoryStream>>(), It.IsAny<SolicitudDTO>(), It.IsAny<Tramite>())).ReturnsAsync(informacionAnalizada);

            // Act
            var result = await _serviceNegocio.ProcesarAsync(solicitudDTO);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Valido);
            Assert.Contains("campo1", result.DatosEncontrados.Keys);
            Assert.Contains("campo2", result.DatosFaltantes);
        }

        [Fact]
        public async Task ProcesarAsync_IdInvalido_DeberiaLanzarApiException()
        {
            // Arrange
            var solicitudDTO = new SolicitudDTO { Asunto = "test" };
            _solicitudRepositorioMock.Setup(repo => repo.Crear(It.IsAny<Solicitud>())).ReturnsAsync(1); // Simulamos un id válido
            _tramiteRepositorioMock.Setup(repo => repo.LeerPorId(It.IsAny<int>())).ReturnsAsync(new Tramite()); // Simulamos un tramite existente

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _serviceNegocio.ProcesarAsync(solicitudDTO));
        }

        [Fact]
        public async Task ProcesarAsync_TramiteInvalido_DeberiaLanzarApiException()
        {
            // Arrange
            var solicitudDTO = new SolicitudDTO { Asunto = "test" };
            _solicitudRepositorioMock.Setup(repo => repo.Crear(It.IsAny<Solicitud>())).ReturnsAsync(1); // Simulamos un id válido
            _tramiteRepositorioMock.Setup(repo => repo.LeerPorId(It.IsAny<int>())).ThrowsAsync(new ApiException(ErrorCode.NO_ENCONTRADO)); // Simulamos un tramite no encontrado

            // Act
            var result = await _serviceNegocio.ProcesarAsync(solicitudDTO);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Valido);
            Assert.Equal("El tramite no es valido", result.Mensaje);
        }

        [Fact]
        public async Task ProcesarAsync_ArchivosInvalidos_DeberiaLanzarApiException()
        {
            // Arrange
            var solicitudDTO = new SolicitudDTO { Asunto = "test", Adjuntos = new List<string> { "file1" }, MsgId = "msg123" };
            _solicitudRepositorioMock.Setup(repo => repo.Crear(It.IsAny<Solicitud>())).ReturnsAsync(1); // Simulamos un id válido
            _tramiteRepositorioMock.Setup(repo => repo.LeerPorId(It.IsAny<int>())).ReturnsAsync(new Tramite { TramiteArchivos = new List<TramiteArchivo>() }); // Simulamos un tramite con archivos
            _fileSearcherMock.Setup(fs => fs.ObtenerArchivo(It.IsAny<string>(), It.IsAny<string>())).Throws(new ApiException(ErrorCode.NO_ENCONTRADO)); // Simulamos una excepción al obtener archivos

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _serviceNegocio.ProcesarAsync(solicitudDTO));
        }

        [Fact]
        public async Task ProcesarAsync_ErrorInterno_DeberiaLanzarApiException()
        {
            // Arrange
            var solicitudDTO = new SolicitudDTO { Asunto = "test" };
            _solicitudRepositorioMock.Setup(repo => repo.Crear(It.IsAny<Solicitud>())).ThrowsAsync(new Exception()); // Simulamos un error interno en la creación de solicitud

            // Act & Assert
            await Assert.ThrowsAsync<ApiException>(() => _serviceNegocio.ProcesarAsync(solicitudDTO));
        }
    }
}
