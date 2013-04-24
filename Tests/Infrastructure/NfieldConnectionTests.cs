using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Nfield.Infrastructure
{
    public class NfieldConnectionTests
    {
        [Fact]
        public void TestSignIn_CredentialsAreIncorrect_ReturnsFalse()
        {
            var mockedHttpClient = new Mock<IHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(IHttpClient)))
                .Returns(mockedHttpClient.Object);
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));

            var target = new NfieldConnection();
            var result = target.SignInAsync("", "", "").Result;

            Assert.False(result);
        }

        [Fact]
        public void TestSignIn_CredentialsAreCorrectButResponseDoesNotContainToken_ReturnsFalse()
        {
            var mockedHttpClient = new Mock<IHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(IHttpClient)))
                .Returns(mockedHttpClient.Object);
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldConnection();
            var result = target.SignInAsync("", "", "").Result;
            Assert.False(result);
        }

        [Fact]
        public void TestSignIn_CredentialsAreCorrectAndResponseContainsToken_ReturnsTrue()
        {
            Uri ServerUri = new Uri(@"http://localhost/");

            const string Domain = "Domain";
            const string Username = "UserName";
            const string Password = "Password";

            var mockedHttpClient = new Mock<IHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(IHttpClient)))
                .Returns(mockedHttpClient.Object);
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"Domain", Domain},
                    {"Username", Username},
                    {"Password", Password}
                });
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.BadRequest));
            mockedHttpClient
                .Setup(httpClient => httpClient.PostAsync(ServerUri + @"/SignIn", It.Is<HttpContent>(c => CheckContent(c, content))))
                .Returns(CreateTaskForResponseMessageWithToken());
//HK ToDo find a way to mock httpClient.DefaultRequestHeaders
//            mockedHttpClient
//                .Setup(httpClient => httpClient.DefaultRequestHeaders.Add(It.IsAny<string>(), It.IsAny<string>()));

            var target = new NfieldConnection();
            target.NfieldServerUri = ServerUri;
            var result = target.SignInAsync(Domain, Username, Password).Result;

            Assert.True(result);
        }

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode)
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(httpStatusCode));
        }

        private Task<HttpResponseMessage> CreateTaskForResponseMessageWithToken()
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Headers.Add("X-AuthenticationToken", "valid token");
            return Task.Factory.StartNew(() => response);
        }

        private bool CheckContent(HttpContent actual, HttpContent expected)
        {
            var result = actual.Equals(expected);
            var actualAsString = actual.ReadAsStringAsync().Result;
            var expectedAsString = expected.ReadAsStringAsync().Result;
            return actualAsString == expectedAsString;
        }
    }

    class TestHeaders : HttpHeaders
    {
        public TestHeaders(): base()
        {}
    }
}
