using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TramitesAI.src.AI.Domain.Dto;
using TramitesAI.src.AI.Services.Implementation;
using TramitesAI.src.Business.Domain.Dto;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Domain.Entidades;
using Xunit;

namespace TramitesAI.Tests.AI.Services.Implementation
{
    public class ProcesadorPythonTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly ProcesadorPython _procesadorPython;

        public ProcesadorPythonTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://127.0.0.1:5000")
            };
            _procesadorPython = new ProcesadorPython(_httpClient);
        }

        [Fact]
        public async Task AnalizarInformacionAsync_DeberiaRetornarInformacionAnalizadaDTO_CuandoRespuestaEsExitosa()
        {
            // Arrange
            var textoArchivos = new List<InformacionExtraidaDTO> { new InformacionExtraidaDTO { Texto = "Texto de prueba", Confianza = 0.9f } };
            var solicitud = new SolicitudDTO { Message = "Mensaje de prueba" };
            var tramite = new Tramite { Nombre = "Denuncia Siniestro" };
            var expectedResponse = new { resultados = new List<InformacionAnalizadaDTO> { new InformacionAnalizadaDTO { Texto = "Resultado" } } };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
                });

            // Act
            var result = await _procesadorPython.AnalizarInformacionAsync(textoArchivos, solicitud, tramite);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Resultado", result.Texto);
        }

        [Fact]
        public async Task AnalizarInformacionAsync_DeberiaLanzarApiException_CuandoModeloNoImplementado()
        {
            // Arrange
            var textoArchivos = new List<InformacionExtraidaDTO> { new InformacionExtraidaDTO { Texto = "Texto de prueba", Confianza = 0.9f } };
            var solicitud = new SolicitudDTO { Message = "Mensaje de prueba" };
            var tramite = new Tramite { Nombre = "Denuncia Siniestro" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotImplemented
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _procesadorPython.AnalizarInformacionAsync(textoArchivos, solicitud, tramite));
            Assert.Equal(ErrorCode.MODELO_NO_IMPLEMENTADO.ToString(), exception.Codigo);
        }

        [Fact]
        public async Task AnalizarInformacionAsync_DeberiaLanzarApiException_CuandoRespuestaEsInvalida()
        {
            // Arrange
            var textoArchivos = new List<InformacionExtraidaDTO> { new InformacionExtraidaDTO { Texto = "Texto de prueba", Confianza = 0.9f } };
            var solicitud = new SolicitudDTO { Message = "Mensaje de prueba" };
            var tramite = new Tramite { Nombre = "Denuncia Siniestro" };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"invalid\": \"response\" }")
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _procesadorPython.AnalizarInformacionAsync(textoArchivos, solicitud, tramite));
            Assert.Equal(ErrorCode.JSON_INVALIDO.ToString(), exception.Codigo);
        }

        [Fact]
        public async Task DeterminarTramite_DeberiaRetornarTramiteDTO_CuandoRespuestaEsExitosa()
        {
            // Arrange
            var asunto = "Consulta de poliza";
            var expectedResponse = new { resultados = new List<int> { 1 } };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
                });

            // Act
            var result = await _procesadorPython.DeterminarTramite(asunto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.valor);
        }

        [Fact]
        public async Task DeterminarTramite_DeberiaLanzarApiException_CuandoRespuestaEsInvalida()
        {
            // Arrange
            var asunto = "Consulta de poliza";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"invalid\": \"response\" }")
                });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _procesadorPython.DeterminarTramite(asunto));
            Assert.Equal(ErrorCode.JSON_INVALIDO.ToString(), exception.Codigo);
        }

        [Fact]
        public async Task DeterminarTramite_DeberiaLanzarApiException_CuandoHayErrorDeRed()
        {
            // Arrange
            var asunto = "Consulta de poliza";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws(new HttpRequestException("Error de red"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _procesadorPython.DeterminarTramite(asunto));
            Assert.Equal(ErrorCode.HTTP_REQUEST_ERROR.ToString(), exception.Codigo);
        }
    }
}
