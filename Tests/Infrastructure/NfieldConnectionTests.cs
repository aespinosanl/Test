﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using Nfield.Services;
using Xunit;

namespace Nfield.Infrastructure
{
    public class NfieldConnectionTests
    {
        #region SignInAsync Tests

        [Fact]
        public void TestSignInAsync_CredentialsAreIncorrect_ReturnsFalse()
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
        public void TestSignInAsync_CredentialsAreCorrect_ReturnsTrue()
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
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldConnection();
            target.NfieldServerUri = ServerUri;
            var result = target.SignInAsync(Domain, Username, Password).Result;

            Assert.True(result);
        }

        #endregion
        #region GetService Tests

        [Fact]
        public void TestGetService_RequestedServiceTypeIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldConnection();
            Assert.Throws(typeof(ArgumentNullException), () => target.GetService(null));
        }

        [Fact]
        public void TestGetService_ServiceDoesNotExist_ReturnsNull()
        {
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(null);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Null(result);
        }

        [Fact]
        public void TestGetService_ServiceExists_ReturnsService()
        {
            var stubbedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>().Object;
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(stubbedNfieldConnectionClientObject);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            Assert.Equal(result, stubbedNfieldConnectionClientObject);
        }

        [Fact]
        public void TestGetService_ServiceExistsAndImplementsINfieldConnectionClientObject_CallsInitializeConnectionOnService()
        {
            var mockedNfieldConnectionClientObject = new Mock<INfieldConnectionClientObject>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(INfieldConnectionClientObject)))
                .Returns(mockedNfieldConnectionClientObject.Object);

            var target = new NfieldConnection();
            var result = target.GetService<INfieldConnectionClientObject>();

            mockedNfieldConnectionClientObject.Verify(client => client.InitializeNfieldConnection(target));
        }

        #endregion

        #region Dispose Tests

        [Fact]
        public void TestDispose_HasClient_CallsDisposeOnClient()
        {
            var mockedHttpClient = new Mock<IHttpClient>();
            var mockedResolver = new Mock<IDependencyResolver>();
            DependencyResolver.Register(mockedResolver.Object);
            mockedResolver
                .Setup(resolver => resolver.Resolve(typeof(IHttpClient)))
                .Returns(mockedHttpClient.Object);
            mockedHttpClient
                .Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldConnection();
            var result = target.SignInAsync("", "", "").Result;
            target.Dispose();

            mockedHttpClient.Verify(client => client.Dispose());
        }

        #endregion

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode)
        {
            return Task.Factory.StartNew(() => new HttpResponseMessage(httpStatusCode));
        }

        private bool CheckContent(HttpContent actual, HttpContent expected)
        {
            var result = actual.Equals(expected);
            var actualAsString = actual.ReadAsStringAsync().Result;
            var expectedAsString = expected.ReadAsStringAsync().Result;
            return actualAsString == expectedAsString;
        }
    }
}
