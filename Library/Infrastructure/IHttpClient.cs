using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{
    internal interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<string> GetStringAsync(string requestUri);
        Task<HttpResponseMessage> PostAsJsonAsync<TContent>(string requestUri, TContent content);
        Task<HttpResponseMessage> PutAsJsonAsync<TContent>(string requestUri, TContent content);
        HttpRequestHeaders DefaultRequestHeaders { get; }
    }
}
