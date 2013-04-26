//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Nfield.Infrastructure
{

    [ExcludeFromCodeCoverage]
    internal sealed class NfieldHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public NfieldHttpClient()
        {
            _httpClient = new HttpClient();
        }

        #region IHttpClient Members

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return SendRequest(_httpClient.SendAsync(request));
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return SendRequest(_httpClient.PostAsync(requestUri, content));
        }

        public Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return SendRequest(_httpClient.GetAsync(requestUri));
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return SendRequest(_httpClient.PostAsJsonAsync(requestUri, content));
        }

        public Task<HttpResponseMessage> PutAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            return SendRequest(_httpClient.PutAsJsonAsync<TContent>(requestUri, content));
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return SendRequest(_httpClient.DeleteAsync(requestUri));
        }

        public Task<HttpResponseMessage> PatchAsJsonAsync<TContent>(string requestUri, TContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);
            request.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
            return SendRequest(_httpClient.SendAsync(request));
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        #endregion

        private Task<HttpResponseMessage> SendRequest(Task<HttpResponseMessage> sendTask)
        {
            return sendTask.ContinueWith<HttpResponseMessage>
                (response =>
                    {
                        var result = response.Result;
                        IEnumerable<string> headerValues;
                        if (result.Headers.TryGetValues("X-AuthenticationToken", out headerValues))
                        {
                            var token = headerValues.First();
                            _httpClient.DefaultRequestHeaders.Remove("Authorization");
                            _httpClient.DefaultRequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Basic", token).ToString());
                        }
                        return result;
                    }
                );
        }

    }
}
