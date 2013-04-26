//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.
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

        #region AddAsync

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

        #region RemoveAsync

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

        #region UpdateAsync

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

        #region QueryAsync

        [Fact]
        public async Task TestQueryAsync_ServerReturnsQuery_ReturnsListWithInterviewers()
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
        public void TestQueryAsync_ServerReturnsError_ThrowsNfieldServerException()
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

        #region ChangePasswordAsync

        [Fact]
        public void TestChangePasswordAsync_InterviewerIsNull_ThrowsArgumentNullException()
        {
            var target = new NfieldInterviewersService();
            Assert.Throws(typeof(ArgumentNullException), () => UnwrapAggregateException(target.ChangePasswordAsync(null, string.Empty)));
        }

        [Fact]
        public void TestChangePasswordAsync_ServerChangesPassword_ReturnsInterviewer()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer {InterviewerId = InterviewerId };
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
                .Setup(client => client.PutAsJsonAsync<string>(ServiceAddress + @"/interviewers/" + InterviewerId, Password))
                .Returns(CreateTask(content));

            var target = new NfieldInterviewersService();
            target.InitializeNfieldConnection(mockedNfieldConnection.Object);

            var actual = target.ChangePasswordAsync(interviewer, Password).Result;

            Assert.Equal(interviewer.InterviewerId, actual.InterviewerId);
        }

        [Fact]
        public void TestChangePasswordAsync_ServerReturnsNotFound_ThrowsNfieldNotFoundException()
        {
            const string Password = "Password";
            const string InterviewerId = "Interviewer X";
            var interviewer = new Interviewer { InterviewerId = InterviewerId };
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

            Assert.Throws(typeof(NfieldNotFoundException), () => UnwrapAggregateException(target.ChangePasswordAsync(interviewer, Password)));
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
                var innerException = ex.InnerException;
                while (innerException is AggregateException)
                {
                    innerException = innerException.InnerException;
                }
                throw innerException;
            }
        }
                
        private Mock<INfieldHttpClient> CreateHttpClientMock(HttpStatusCode httpStatusCode)
        {
            var mockedHttpClient = new Mock<INfieldHttpClient>();

            //setup the mocked HttpClient to return httpStatusCode for all methods that send a request to the server

            mockedHttpClient
                .Setup(client => client.PostAsJsonAsync<Interviewer>(It.IsAny<string>(), It.IsAny<Interviewer>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.PutAsJsonAsync<string>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode) { Content = new StringContent("") }));

            mockedHttpClient
                .Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.GetAsync(It.IsAny<string>()))
                .Returns(CreateTask(httpStatusCode));

            mockedHttpClient
                .Setup(client => client.PatchAsJsonAsync<UpdateInterviewer>(It.IsAny<string>(), It.IsAny<UpdateInterviewer>()))
                .Returns(Task.Factory.StartNew<HttpResponseMessage>(() => new HttpResponseMessage(httpStatusCode) { Content = new StringContent("") }));

            return mockedHttpClient;
        }
    }
}
