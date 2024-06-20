using Newtonsoft.Json;
using System.Text;
using TramitesAI.src.Comun.Servicios.Interfaces;

namespace TramitesAI.src.Comun.Servicios.Implementaciones
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly HttpClient _httpClient;
        private Uri URL_Python = new("http://127.0.0.1:5000");

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content)
        {
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");
            return await _httpClient.PostAsync(URL_Python + requestUri, jsonContent);
        }
    }
}
