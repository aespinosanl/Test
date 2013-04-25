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
        private const string ServiceAddress = @"http://localhost/nfieldapi";

        #region AddAsync tests

        [Fact]
        public void TestAddAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws(typeof(ArgumentNullException), () => UnwrapAggregateException(target.AddAsync(null)));
        }

        [Fact]
        public void TestAddAsync_ServerAcceptsInterviewer_ReturnsInterviewer()
        {
            var interviewer = new Interviewer { UserName = "User X" };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            var content = new StringContent(JsonConvert.SerializeObject(interviewer));
            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync<Interviewer>(ServiceAddress + @"/interviewers", interviewer))
                .Returns(CreateTask(content));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.AddAsync(interviewer).Result;

            Assert.Equal(interviewer.UserName, actual.UserName);
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
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer { InterviewerId = InterviewerId };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.DeleteAsync(ServiceAddress + @"/interviewers/" + InterviewerId))
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
        public void TestUpdateAsync_ServerUpdatesInterviewer_ReturnsInterviewer()
        {
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer { 
                InterviewerId = InterviewerId,  
                FirstName = "XXX" 
            };
            var mockedNfieldConnection = new Mock<INfieldConnectionClient>();
            var mockedHttpClient = CreateHttpClientMock(HttpStatusCode.BadRequest);
            mockedNfieldConnection
                .SetupGet(connection => connection.Client)
                .Returns(mockedHttpClient.Object);
            mockedNfieldConnection
                .SetupGet(connection => connection.NfieldServerUri)
                .Returns(new Uri(ServiceAddress));
            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync<UpdateInterviewer>(ServiceAddress + @"/interviewers/" + InterviewerId, It.IsAny<UpdateInterviewer>()))
                .Returns(CreateTask(new StringContent(JsonConvert.SerializeObject(interviewer))));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.UpdateAsync(interviewer).Result;

            Assert.Equal(interviewer.FirstName, actual.FirstName);
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

            Assert.Throws(typeof(NfieldNotFoundException), () => UnwrapAggregateException(target.UpdateAsync(interviewer)));
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
                .Setup(client => client.GetAsync(ServiceAddress + @"/interviewers"))
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
            return Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(HttpStatusCode.OK){Content = content});
        }

        private Task<HttpResponseMessage> CreateTask(HttpStatusCode httpStatusCode)
        {
            return Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode));
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

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync<object>(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode){Content = new StringContent("")}));

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync(It.IsAny<string>(), It.IsAny<object>()))
                .Returns(Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode) { Content = new StringContent("Hello") }));

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync<UpdateInterviewer>(It.IsAny<string>(), It.IsAny<UpdateInterviewer>()))
                .Returns(Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode) { Content = new StringContent("World") }));

            return mockedHttpClient;
        }
    }
}
