using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Implementation;
using TramitesAI.src.AI.Services.Interfaces;
using TramitesAI.src.Common.Exceptions;
using Xunit;

namespace TramitesAITest.Test.Unitarios.AI
{
    public class ServicioTesseractTests
    {
        private readonly Mock<ITesseractEngineWrapper> _mockTesseractWrapper;
        private readonly ServicioTesseract _servicioTesseract;

        public ServicioTesseractTests()
        {
            _mockTesseractWrapper = new Mock<ITesseractEngineWrapper>();
            _servicioTesseract = new ServicioTesseract(_mockTesseractWrapper.Object);
        }

        [Fact]
        public void ExtraerInformacionDeArchivos_DeberiaRetornarInformacionExtraida_CuandoArchivosValidos()
        {
            // Arrange
            var memoryStreams = new List<MemoryStream> { new MemoryStream(new byte[] { 1, 2, 3 }) };
            var expectedDto = new InformacionExtraidaDTO
            {
                Texto = "Texto de prueba",
                Confianza = 0.9f
            };

            _mockTesseractWrapper
                .Setup(wrapper => wrapper.Procesar(It.IsAny<byte[]>()))
                .Returns(expectedDto);

            // Act
            var result = _servicioTesseract.ExtraerInformacionDeArchivos(memoryStreams);

            // Assert
            Assert.Single(result);
            Assert.Equal(expectedDto.Texto, result[0].Texto);
            Assert.Equal(expectedDto.Confianza, result[0].Confianza);
        }

        [Fact]
        public void ExtraerInformacionDeArchivos_DeberiaLanzarApiException_CuandoTesseractFalla()
        {
            // Arrange
            var memoryStreams = new List<MemoryStream> { new MemoryStream(new byte[] { 1, 2, 3 }) };

            _mockTesseractWrapper
                .Setup(wrapper => wrapper.Procesar(It.IsAny<byte[]>()))
                .Throws(new Exception("Simulando error de Tesseract"));

            // Act & Assert
            var exception = Assert.Throws<ApiException>(() => _servicioTesseract.ExtraerInformacionDeArchivos(memoryStreams));
            Assert.Equal(ErrorCode.ERROR_EXTRAYENDO_TEXTOS.ToString(), exception.Codigo);
        }
    }
}
