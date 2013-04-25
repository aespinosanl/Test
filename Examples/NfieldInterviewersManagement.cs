using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nfield.Models;
using Nfield.Services;
using Nfield.Extensions;
using System.Linq;

namespace Nfield.SDK.Samples
{
    /// <summary>
    /// Helper class handling <see cref="Interviewer"/>s management
    /// </summary>
    public class NfieldInterviewersManagement
    {
        private readonly INfieldInterviewersService _interviewersService;

        /// <summary>
        /// Ctor.
        /// </summary>
        public NfieldInterviewersManagement(INfieldInterviewersService interviewersService)
        {
            _interviewersService = interviewersService;
        }

        /// <summary>
        /// Adds and <see cref="Interviewer"/> to the system through an asynchronous operation.
        /// </summary>
        public async Task<Interviewer> AddInterviewerAsync()
        {
            Interviewer interviewer = new Interviewer
            {
                ClientInterviewerId = "ftropo5i",
                FirstName = "Bill",
                LastName = "Gates",
                EmailAddress = "bill@hotmail.com",
                TelephoneNumber = "0206598547",
                UserName = "bill",
                Password = "password12"
            };

            var addedInterviewer = await _interviewersService.AddAsync(interviewer);
            return addedInterviewer;
        }

        /// <summary>
        /// Adds and <see cref="Interviewer"/> to the system through a synchronous operation.
        /// </summary>
        public Interviewer AddInterviewer()
        {
            Interviewer interviewer = new Interviewer
            {
                ClientInterviewerId = "pomn45dr",
                FirstName = "Steve",
                LastName = "Balmer",
                EmailAddress = "steve@hotmail.com",
                TelephoneNumber = "0207821569",
                UserName = "steve",
                Password = "password12"
            };

            return _interviewersService.Add(interviewer);
        }

        /// <summary>
        /// Updates an <see cref="Interviewer"/> through an asynchronous operation.
        /// </summary>
        public async Task UpdateInterviewerAsync(Interviewer interviewer)
        {
            if(interviewer == null)
            {
                return;
            }
            await _interviewersService.UpdateAsync(interviewer);
        }

        /// <summary>
        /// Updates an <see cref="Interviewer"/> through a synchronous operation.
        /// </summary>
        public void UpdateInterviewer(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return;
            }


            _interviewersService.Update(interviewer);
        }

        /// <summary>
        /// Removes an existing <see cref="Interviewer"/> through an asynchronous operation.
        /// </summary>
        public async Task RemoveInterviewerAsync(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return;
            }

            await _interviewersService.RemoveAsync(interviewer);
        }

        /// <summary>
        /// Removes an existing <see cref="Interviewer"/> through a synchronous operation.
        /// </summary>
        public void RemoveInterviewer(Interviewer interviewer)
        {
            if (interviewer == null)
            {
                return;
            }

            _interviewersService.Remove(interviewer);
        }

        /// <summary>
        /// Performs query operation for available Interviewers. 
        /// </summary>
        public void QueryForInterviewers()
        {
            IEnumerable<Interviewer> allInterviewers = _interviewersService.Query().ToList();

            IEnumerable<Interviewer> interviewersWithValidTelephone =
                _interviewersService.Query().Where(interviewer => !string.IsNullOrEmpty(interviewer.TelephoneNumber)).ToList();
        }

        /// <summary>
        /// Performs query operation for available Interviewers asynchronously. 
        /// </summary>
        public void QueryForInterviewersAsync()
        {
            // execute async call
            Task<IQueryable<Interviewer>> task = _interviewersService.QueryAsync();

            // do some work here
            // ...

            // get async call result
            IEnumerable<Interviewer> allInterviewers = task.Result.ToList();

            IEnumerable<Interviewer> interviewersWithValidTelephone =
                allInterviewers.Where(interviewer => !string.IsNullOrEmpty(interviewer.TelephoneNumber)).ToList();
        }

    }
}