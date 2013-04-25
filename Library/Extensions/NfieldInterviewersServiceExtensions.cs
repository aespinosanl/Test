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
        public static Interviewer Add(this INfieldInterviewersService interviewersService, Interviewer interviewer)
        {
            return interviewersService.AddAsync(interviewer).Result;
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
