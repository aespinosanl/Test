using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nfield.Exceptions;
using Nfield.Infrastructure;
using Nfield.Models;

namespace Nfield.Services.Implementation
{
    /// <summary>
    /// Implementation of <see cref="INfieldInterviewersService"/>
    /// </summary>
    internal class NfieldInterviewersService : INfieldInterviewersService, INfieldConnectionClientObject
    {

        #region implementation of INfieldInterviewersService

        /// <summary>
        /// See <see cref="INfieldInterviewersService.AddAsync"/>
        /// </summary>
        public async Task AddAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var result = await Client.PostAsJsonAsync(InterviewersApi.AbsoluteUri, interviewer);

            ValidateStatusCode(result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        public async Task RemoveAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var content = new StringContent(JsonConvert.SerializeObject(new[] {interviewer.InterviewerId}));
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/json");
            var request = new HttpRequestMessage(HttpMethod.Delete, InterviewersApi)
            {
                Content = content
            };
            
            var result = await Client.SendAsync(request);

            ValidateStatusCode(result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public async Task UpdateAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var result = await Client.PutAsJsonAsync(InterviewersApi.AbsoluteUri + @"/all/edit", interviewer);

            ValidateStatusCode(result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public async Task<IQueryable<Interviewer>> QueryAsync()
        {
            var result = await Client.GetAsync(InterviewersApi.AbsoluteUri);
            ValidateStatusCode(result);
            
            string content = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Interviewer>>(content).AsQueryable();
        }

        #endregion

        #region implementation of INfieldConnectionClientObject

        public INfieldConnectionClient ConnectionClient { get; internal set; }

        public void InitializeNfieldConnection(INfieldConnectionClient connection)
        {
            ConnectionClient = connection;
            Client = ConnectionClient.Client;
            InterviewersApi = new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + @"/api/interviewers"); 
        }

        #endregion

        IHttpClient Client { get; set; }

        Uri InterviewersApi { get; set; }

        /// <summary>
        /// Helper method that checks the <paramref name="result"/> and throws the appropriate exceptions 
        /// based on the status code.
        /// </summary>
        private static void ValidateStatusCode(HttpResponseMessage result)
        {
            switch(result.StatusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new NfieldConflictException(result.ReasonPhrase);

                case HttpStatusCode.BadRequest:
                    throw new NfieldBadRequestException(result.ReasonPhrase);

                case HttpStatusCode.NotFound:
                    throw new NfieldNotFoundException(result.ReasonPhrase);
            }

            int code = (int) result.StatusCode;
            if(code >= 500 && code < 600)
                throw new NfieldServerErrorException(result.ReasonPhrase);
        }
    }
}
