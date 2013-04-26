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
                public Task<Interviewer> AddAsync(Interviewer interviewer)
        {
            if(interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return Client.PostAsJsonAsync(InterviewersApi.AbsoluteUri, interviewer)
                         .ContinueWith(responseMessageTask => ValidateStatusCodeAsync(responseMessageTask.Result).Result)
                         .ContinueWith(task => task.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(task => JsonConvert.DeserializeObjectAsync<Interviewer>(task.Result).Result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        public Task RemoveAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return
                Client.DeleteAsync(InterviewersApi + @"/" + interviewer.InterviewerId)
                      .ContinueWith(responseTask => ValidateStatusCodeAsync(responseTask.Result));
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        public Task<Interviewer> UpdateAsync(Interviewer interviewer)
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

            return Client.PatchAsJsonAsync(InterviewersApi + @"/" + interviewer.InterviewerId, updatedInterviewer)
                         .ContinueWith(responseMessageTask => ValidateStatusCodeAsync(responseMessageTask.Result).Result)
                         .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(stringTask => JsonConvert.DeserializeObjectAsync<Interviewer>(stringTask.Result).Result);
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        public Task<IQueryable<Interviewer>> QueryAsync()
        {
            return Client.GetAsync(InterviewersApi.AbsoluteUri)
                        .ContinueWith(responoseMessageTask => ValidateStatusCodeAsync(responoseMessageTask.Result).Result)
                        .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                        .ContinueWith(stringTask => JsonConvert.DeserializeObject<List<Interviewer>>(stringTask.Result).AsQueryable());   
        }

        /// <summary>
        /// See <see cref="INfieldInterviewersService.ChangePasswordAsync"/>
        /// </summary>
        public Task<Interviewer> ChangePasswordAsync(Interviewer interviewer, string password)
        {
            if (interviewer == null)
            {
                throw new ArgumentNullException("interviewer");
            }

            return Client.PutAsJsonAsync(InterviewersApi + @"/" + interviewer.InterviewerId, password)
                         .ContinueWith(responseMessageTask => ValidateStatusCodeAsync(responseMessageTask.Result).Result)
                         .ContinueWith(responseMessageTask => responseMessageTask.Result.Content.ReadAsStringAsync().Result)
                         .ContinueWith(stringTask => JsonConvert.DeserializeObjectAsync<Interviewer>(stringTask.Result).Result);
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

        INfieldHttpClient Client { get; set; }

        Uri InterviewersApi { get; set; }

        /// <summary>
        /// Helper method that checks the <paramref name="result"/> and throws the appropriate exceptions 
        /// based on the status code.
        /// </summary>
        private static Task<HttpResponseMessage> ValidateStatusCodeAsync(HttpResponseMessage result)
        {
            return Task.Factory.StartNew(() =>
            {
                switch (result.StatusCode)
                {
                    case HttpStatusCode.Conflict:
                        throw new NfieldConflictException(result.ReasonPhrase);

                    case HttpStatusCode.BadRequest:
                        throw new NfieldBadRequestException(result.ReasonPhrase);

                    case HttpStatusCode.NotFound:
                        throw new NfieldNotFoundException(result.ReasonPhrase);
                }

                int code = (int)result.StatusCode;
                if (code >= 500 && code < 600)
                    throw new NfieldServerErrorException(result.ReasonPhrase);

                return result;
            });
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
