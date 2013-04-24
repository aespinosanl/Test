using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{

    [ExcludeFromCodeCoverage] //It's just a wrapper class
    internal sealed class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        #region IHttpClient Members

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        public Task<string> GetStringAsync(string requestUri)
        {
            return _httpClient.GetStringAsync(requestUri);
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return _httpClient.PostAsJsonAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> PutAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return _httpClient.PutAsJsonAsync<TContent>(requestUri, content);
        }

        public HttpRequestHeaders DefaultRequestHeaders 
        { 
            get
            {
                return _httpClient.DefaultRequestHeaders;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        #endregion
    }
}
