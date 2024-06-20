using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TramitesAITest.Test.Funcionales
{
    public class PingControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PingControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Ping_Returns_Pong()
        {
            // Arrange
            var url = "/api/ping";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode, "Expected a successful response status code.");
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Equal("Pong", responseString);
        }
    }
}
