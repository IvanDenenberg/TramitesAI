namespace TramitesAI.src.Comun.Servicios.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsync<T>(string requestUri, T content);
    }


}
