using System;
using Nfield.Infrastructure;
using Nfield.Services.Implementation;
using Nfield.Extensions;
using Xunit;
using Nfield.Models;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Nfield.Exceptions;
using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Nfield.Services
{
    public class NfieldInterviewersServiceTests
    {
        private const string ServiceAddress = @"http://localhost";

        #region AddAsync tests

        [Fact]
        public void TestAddAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws(typeof(ArgumentNullException), () => UnwrapAggregateException(target.AddAsync(null)));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsInterviewer_DoesNotThrow()
        {
            var interviewer = new Interviewer();
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync<Interviewer>(ServiceAddress + @"//api/interviewers", interviewer))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.AddAsync(interviewer).Wait());
        }

        [Fact]
        public void TestAddAsync_ServerReturnsBadRequest_ThrowsNfieldBadRequestException()
        {
            var interviewer = new Interviewer();
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.Throws(typeof(NfieldBadRequestException), () => UnwrapAggregateException(target.AddAsync(interviewer)));
        }

        #endregion

        #region RemoveAsync tests

        [Fact]
        public void TestRemoveAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.RemoveAsync(null)));
        }

        [Fact]
        public void TestRemoveAsync_ServerRemovedInterviewer_DoesNotThrow()
        {
            var interviewer = new Interviewer { InterviewerId = "TestInterviewer" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.SendAsync(It.Is<HttpRequestMessage>(message => IsExpectedDeleteMessage(message))))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.RemoveAsync(interviewer).Wait());
        }

        [Fact]
        public void TestRemoveAsync_ServerReturnsConflict_ThrowsNfieldConflictException()
        {
            var interviewer = new Interviewer { InterviewerId = "TestInterviewer" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.Conflict);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.Throws(typeof(NfieldConflictException), () => UnwrapAggregateException(target.AddAsync(interviewer)));
        }

        #endregion

        #region UpdateAsync tests

        [Fact]
        public void TestUpdateAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws<ArgumentNullException>(() => UnwrapAggregateException(target.UpdateAsync(null)));
        }

        [Fact]
        public void TestUpdateAsync_ServerUpdatesInterviewer_DoesNotThrow()
        {
            var interviewer = new Interviewer { InterviewerId = "TestInterviewer" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync<Interviewer>(ServiceAddress + @"//api/interviewers/all/edit", interviewer))
                .Returns(CreateTask(HttpStatusCode.OK));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.DoesNotThrow(() => target.UpdateAsync(interviewer).Wait());
        }

        [Fact]
        public void TestUpdateAsync_ServerReturnsNotFound_ThrowsNfieldNotFoundException()
        {
            var interviewer = new Interviewer { InterviewerId = "TestInterviewer" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.NotFound);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.Throws(typeof(NfieldNotFoundException), () => UnwrapAggregateException(target.AddAsync(interviewer)));
        }

        #endregion

        #region QueryGet

        [Fact]
        public async Task TestQueryGet_ServerReturnsQuery_ReturnsListWithInterviewers()
        {
            var expectedInterviewers = new Interviewer[]
            { new Interviewer{InterviewerId = "TestInterviewer"},
              new Interviewer{InterviewerId = "AnotherTestInterviewer"}
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.NotFound);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.GetAsync(ServiceAddress + @"//api/interviewers"))
                .Returns(CreateTask(new StringContent(JsonConvert.SerializeObject(expectedInterviewers))));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actualInterviewers = await target.QueryAsync();
            Assert.Equal(expectedInterviewers[0].InterviewerId, actualInterviewers.ToArray()[0].InterviewerId);
            Assert.Equal(expectedInterviewers[1].InterviewerId, actualInterviewers.ToArray()[1].InterviewerId);
            Assert.Equal(2, actualInterviewers.Count());
        }

        [Fact]
        public void TestQueryGet_ServerReturnsError_ThrowsNfieldServerException()
        {
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.InternalServerError);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            Assert.Throws<NfieldServerErrorException>(() => UnwrapAggregateException(target.QueryAsync()));
        }

        #endregion

        private Task<HttpResponseMessage> CreateTask(HttpContent content)
        {
            var task = new Task<HttpResponseMessage>(() => { var msg = new HttpResponseMessage(HttpStatusCode.OK); msg.Content = content; return msg; });
            task.Start();
            return task;
        }

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode)
        {
            var task = new Task<HttpResponseMessage>(() => { return new HttpResponseMessage(httpStatusCode); });
            task.Start();
            return task;
        }   

        private void UnwrapAggregateException(Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }

        private bool IsExpectedDeleteMessage(HttpRequestMessage message)
        {
            var methodOk = message.Method == HttpMethod.Delete;
            var uriOk = message.RequestUri.AbsoluteUri == ServiceAddress + @"//api/interviewers";
            var interviewerIdOk = message.Content.ReadAsStringAsync().Result == "[\"TestInterviewer\"]";
            return methodOk && uriOk && interviewerIdOk;
        }

        private Mock<IHttpClient> CreateHttpClientMock(HttpStatusCode httpStatusCode)
        {
            var mockedHttpClient = new Mock<IHttpClient>();

            //setup the mocked HttpClient to return httpStatusCode for all methods that send a request to the server

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync<Interviewer>(It.IsAny<string>(), It.IsAny<Interviewer>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync<Interviewer>(It.IsAny<string>(), It.IsAny<Interviewer>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .Returns(CreateTask(httpStatusCode));

            return mockedHttpClient;
        }
    }
}
