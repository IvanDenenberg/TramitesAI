using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Implementation;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Repository.Domain.Entidades;
using Xunit;

namespace TramitesAITest.Test.Unitarios
{
    public class AIHandlerTests
    {
        private readonly Mock<IAnalizadorAI> _analizadorMock;
        private readonly Mock<IExtractorInformacion> _extractorMock;
        private readonly AIHandler _aiHandler;

        public AIHandlerTests()
        {
            _analizadorMock = new Mock<IAnalizadorAI>();
            _extractorMock = new Mock<IExtractorInformacion>();
            _aiHandler = new AIHandler(_analizadorMock.Object, _extractorMock.Object);
        }

        [Fact]
        public async Task DeterminarTramiteAsync_DeberiaRetornarValorTramite()
        {
            // Arrange
            var requestDTO = "test request";
            var tramiteDTO = new TramiteDTO { valor = 1 };
            _analizadorMock.Setup(analizador => analizador.DeterminarTramite(requestDTO)).ReturnsAsync(tramiteDTO);

            // Act
            var result = await _aiHandler.DeterminarTramiteAsync(requestDTO);

            // Assert
            Assert.Equal(tramiteDTO.valor, result);
        }

        [Fact]
        public async Task ProcesarInformacion_ConArchivos_DeberiaRetornarInformacionAnalizada()
        {
            // Arrange
            var archivos = new List<MemoryStream> { new MemoryStream() };
            var solicitud = new SolicitudDTO();
            var tramite = new Tramite();
            var informacionArchivos = new List<InformacionExtraidaDTO>();
            var informacionAnalizada = new InformacionAnalizadaDTO();

            _extractorMock.Setup(extractor => extractor.ExtraerInformacionDeArchivos(archivos)).Returns(informacionArchivos);
            _analizadorMock.Setup(analizador => analizador.AnalizarInformacionAsync(informacionArchivos, solicitud, tramite)).ReturnsAsync(informacionAnalizada);

            // Act
            var result = await _aiHandler.ProcesarInformacion(archivos, solicitud, tramite);

            // Assert
            Assert.Equal(informacionAnalizada, result);
        }

        [Fact]
        public async Task ProcesarInformacion_SinArchivos_DeberiaRetornarInformacionAnalizada()
        {
            // Arrange
            var archivos = new List<MemoryStream>();
            var solicitud = new SolicitudDTO();
            var tramite = new Tramite();
            var informacionAnalizada = new InformacionAnalizadaDTO();

            _analizadorMock.Setup(analizador => analizador.AnalizarInformacionAsync(It.IsAny<List<InformacionExtraidaDTO>>(), solicitud, tramite)).ReturnsAsync(informacionAnalizada);

            // Act
            var result = await _aiHandler.ProcesarInformacion(archivos, solicitud, tramite);

            // Assert
            Assert.Equal(informacionAnalizada, result);
        }
    }
}
