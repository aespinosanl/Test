using System;
using System.Linq;
using Nfield.Models;
using Nfield.Services;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to make the methods of NfieldInterviewersService synchronous
    /// </summary>
    public static class NfieldInterviewersServiceExtensions
    {
        /// <summary>
        /// a synchronous version of <see cref="INfieldInterviewersService.AddAsync"/>
        /// </summary>
        /// <param name="interviewersService">The <see cref="INfieldInterviewersService"/> to use</param>
        /// <param name="interviewer">interviewer to add</param>
        public static void Add(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            interviewersService.AddAsync(interviewer).Wait();
        }

        /// <summary>
        /// a synchronous version of <see cref="INfieldInterviewersService.RemoveAsync"/>
        /// </summary>
        /// <param name="interviewersService">The <see cref="INfieldInterviewersService"/> to use</param>
        /// <param name="interviewer">interviewer to remove</param>
        public static void Remove(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            interviewersService.RemoveAsync(interviewer).Wait();
        }

        /// <summary>
        /// a synchronous version of <see cref="INfieldInterviewersService.UpdateAsync"/>
        /// </summary>
        /// <param name="interviewersService">The <see cref="INfieldInterviewersService"/> to use</param>
        /// <param name="interviewer">interviewer to update</param>
        public static void Update(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            interviewersService.UpdateAsync(interviewer).Wait();
        }

        /// <summary>
        /// a synchronous version of <see cref="INfieldInterviewersService.QueryAsync"/>
        /// </summary>
        /// <returns>interviewers as queryable</returns>
        public static IQueryable<Interviewer> Query(this INfieldInterviewersService interviewersService)
        {
            return interviewersService.QueryAsync().Result;
        }
    }
}
