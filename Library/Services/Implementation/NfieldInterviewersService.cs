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
        public async Task<Interviewer> AddAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var result = await Client.PostAsJsonAsync(InterviewersApi.AbsoluteUri, interviewer);

            ValidateStatusCode(result);

            return await JsonConvert.DeserializeObjectAsync<Interviewer>(await result.Content.ReadAsStringAsync());
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

            var result = await Client.DeleteAsync(InterviewersApi + @"/" + interviewer.InterviewerId);

            ValidateStatusCode(result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public async Task<Interviewer> UpdateAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            var updatedInterviewer = new UpdateInterviewer{
                EmailAddress = interviewer.EmailAddress,
                FirstName = interviewer.FirstName,
                LastName = interviewer.LastName,
                TelephoneNumber = interviewer.TelephoneNumber
            };

            var result = await Client.PatchAsJsonAsync(InterviewersApi + @"/" + interviewer.InterviewerId, updatedInterviewer);

            ValidateStatusCode(result);

            return await JsonConvert.DeserializeObjectAsync<Interviewer>(await result.Content.ReadAsStringAsync());
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
            InterviewersApi = new Uri(ConnectionClient.NfieldServerUri.AbsoluteUri + @"/interviewers"); 
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

    internal class UpdateInterviewer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
