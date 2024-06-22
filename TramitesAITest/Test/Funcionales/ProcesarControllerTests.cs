using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using System.Net;
using TramitesAI.src.Common.Exceptions;
using TramitesAI.src.Repository.Configuration;
using TramitesAI.src.Repository.Domain.Entidades;
using TramitesAI.src.Comun.Servicios.Interfaces;
using System.Text;
using TramitesAI.src.Business.Domain.Dto;

namespace TramitesAITest.Test.Funcionales
{
    public class ProcesarControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly Mock<IHttpClientWrapper> _mockHttpClientWrapper;

        public ProcesarControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
            _mockHttpClientWrapper = Mock.Get(factory.Services.GetRequiredService<IHttpClientWrapper>());
        }

        [Fact]
        public async Task LeerSolicitudProcesadaPorId_Devuelve_Ok()
        {
            // Arrange
            int solicitudId = 2;
            var url = $"/api/procesar/leer-por-id/{solicitudId}";

            // Sembrar datos en la base de datos en memoria
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ConfigDBContext>();
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(2).Canal("123").Email("123").MsgId("123").Solicitud(new Solicitud { MensajeSolicitud = "123" }).
                    Build();
                solicitudProcesada.Respuesta = new Respuesta { MensajeRespuesta = "123" };
                context.SolicitudesProcesadas.Add(solicitudProcesada);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<SolicitudProcesada>(responseContent);

            Assert.NotNull(responseData);
            Assert.Equal(solicitudId, responseData.Id);
        }


        [Fact]
        public async Task LeerSolicitudProcesadaPorId_Devuelve_ApiException_ParametrosInvalidos()
        {
            // Arrange
            int solicitudId = -1; // Id que provocará una excepción
            var url = $"/api/procesar/leer-por-id/{solicitudId}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            // Verificar el contenido del mensaje de error
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains(ErrorCode.PARAMETROS_INVALIDOS.ToString(), responseContent);
        }

        [Fact]
        public async Task LeerSolicitudProcesadaPorId_Devuelve_ApiException_NoEncontrado()
        {
            // Arrange
            int solicitudId = 20; // Id que provocará una excepción
            var url = $"/api/procesar/leer-por-id/{solicitudId}";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            // Verificar el contenido del mensaje de error
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains(ErrorCode.NO_ENCONTRADO.ToString(), responseContent);
        }

        [Fact]
        public async Task LeerTodasLasSolicitudesProcesadas_Devuelve_Ok()
        {
            // Arrange
            var url = $"/api/procesar/leer-todas";

            // Sembrar datos en la base de datos en memoria
            using (var scope = _factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ConfigDBContext>();
                var solicitudProcesada = new SolicitudProcesada.SolicitudProcesadaBuilder()
                    .Id(1).Canal("123").Email("123").MsgId("123").Solicitud(new Solicitud { MensajeSolicitud = "123" }).
                    Build();
                solicitudProcesada.Respuesta = new Respuesta { MensajeRespuesta = "123" };
                context.SolicitudesProcesadas.Add(solicitudProcesada);
                await context.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<List<SolicitudProcesada>>(responseContent);

            Assert.NotNull(responseData);
        }

        [Fact]
        public async Task ProcesarAsync_Devuelve_Ok()
        {
            // Arrange
            var url = "/api/procesar";

            var solicitudDto = new SolicitudDTO
            {
                MsgId = "Id de Mensaje",
                Canal = "Canal",
                Origen = "Correo electrónico",
                Recibido = DateTime.Parse("2024-04-26T12:00:00"),
                Asunto = "Cotizar Poliza Auto",
                Mensaje = "Hola quiero consultar por la póliza para un ford fiesta 2018, mi código postal es 5000",
                Adjuntos = new List<string> { "Adjunto1", "Adjunto2", "Adjunto3" }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(solicitudDto), Encoding.UTF8, "application/json");


            // Configurar el comportamiento esperado del mock de HttpClientWrapper
            _mockHttpClientWrapper.Setup(wrapper =>
                    wrapper.PostAsync("/evaluar_asunto", It.IsAny<object>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"resultados\": [2]}", Encoding.UTF8, "application/json")
                });

            var jsonString = "{\"resultados\":[{\"campos\":{\"anio\":\"2018\",\"cod_postal\":\"5000\",\"marca\":\"ford\",\"modelo\":\"fiesta\"},\"texto\":\"Hola quiero consultar por la p\\u00f3liza para un ford fiesta 2018, mi c\\u00f3digo postal es 5000\"}]}\n";

            _mockHttpClientWrapper.Setup(wrapper =>
                    wrapper.PostAsync("/poliza_auto", It.IsAny<object>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonString, Encoding.UTF8, "application/json")
                });

            // Act
            var response = await _client.PostAsync(url, jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<RespuestaDTO>(responseContent);

            Assert.NotNull(responseData);
            // Aquí puedes realizar más aserciones según la respuesta esperada del método ProcesarAsync
        }


        [Fact]
        public async Task ProcesarAsync_Devuelve_ApiException_NoImplementado()
        {
            // Arrange
            var url = "/api/procesar";

            var solicitudDto = new SolicitudDTO
            {
                MsgId = "Id de Mensaje",
                Canal = "Canal",
                Origen = "Correo electrónico",
                Recibido = DateTime.Parse("2024-04-26T12:00:00"),
                Asunto = "Cotizar Poliza Hogar",
                Mensaje = "Hola quiero consultar por la póliza para un ford fiesta 2018, mi código postal es 5000",
                Adjuntos = new List<string> { "Adjunto1", "Adjunto2", "Adjunto3" }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(solicitudDto), Encoding.UTF8, "application/json");


            // Configurar el comportamiento esperado del mock de HttpClientWrapper
            _mockHttpClientWrapper.Setup(wrapper =>
                    wrapper.PostAsync("/evaluar_asunto", It.IsAny<object>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"resultados\": [4]}", Encoding.UTF8, "application/json")
                });

          
            _mockHttpClientWrapper.Setup(wrapper =>
                    wrapper.PostAsync("/poliza_hogar", It.IsAny<object>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotImplemented,
                    Content = new StringContent("El modelo no fue implementado", Encoding.UTF8, "application/json")
                });

            // Act
            var response = await _client.PostAsync(url, jsonContent);

            // Assert
            Assert.Equal(HttpStatusCode.NotImplemented, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<RespuestaDTO>(responseContent);

            Assert.NotNull(responseData);
            // Aquí puedes realizar más aserciones según la respuesta esperada del método ProcesarAsync
        }

        [Fact]
        public async Task ProcesarAsync_Devuelve_Ok_TramiteInvalido()
        {
            // Arrange
            var url = "/api/procesar";

            var solicitudDto = new SolicitudDTO
            {
                MsgId = "Id de Mensaje",
                Canal = "Canal",
                Origen = "Correo electrónico",
                Recibido = DateTime.Parse("2024-04-26T12:00:00"),
                Asunto = "Telequino vacante",
                Mensaje = "Hola quiero consultar por la póliza para un ford fiesta 2018, mi código postal es 5000",
                Adjuntos = new List<string> { "Adjunto1", "Adjunto2", "Adjunto3" }
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(solicitudDto), Encoding.UTF8, "application/json");


            // Configurar el comportamiento esperado del mock de HttpClientWrapper
            _mockHttpClientWrapper.Setup(wrapper =>
                    wrapper.PostAsync("/evaluar_asunto", It.IsAny<object>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"resultados\": [0]}", Encoding.UTF8, "application/json")
                });

            // Act
            var response = await _client.PostAsync(url, jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<RespuestaDTO>(responseContent);

            Assert.NotNull(responseData);
            // Aquí puedes realizar más aserciones según la respuesta esperada del método ProcesarAsync
        }

    }
}
