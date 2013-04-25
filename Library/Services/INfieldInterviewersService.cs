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
using Nfield.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Nfield.Services
{
    /// <summary>
    /// Represents a set of methods to read and update the interviewer data.
    /// </summary>
    public interface INfieldInterviewersService
    {
        /// <summary>
        /// Adds a new interviewer.
        /// </summary>>
        /// <param name="interviewer">The interviewer to add.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldConflictException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldBadRequestException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldNotFoundException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldServerErrorException"></exception>
        Task<Interviewer> AddAsync(Interviewer interviewer);

        /// <summary>
        /// Removes the interviewer.
        /// </summary>
        /// <param name="interviewer">The interviewer to remove.</param>
        /// <exception cref="T:System.AggregateException"></exception>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldConflictException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldBadRequestException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldNotFoundException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldServerErrorException"></exception>        
        Task RemoveAsync(Interviewer interviewer);

        /// <summary>
        /// Updates interviewers data.
        /// </summary>
        /// <exception cref="T:System.AggregateException"></exception>
        /// <param name="interviewer">The interviewer to update.</param>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldConflictException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldBadRequestException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldNotFoundException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldServerErrorException"></exception>        
        Task<Interviewer> UpdateAsync(Interviewer interviewer);

        /// <summary>
        /// Gets interviewer queryable object.
        /// <exception cref="T:System.AggregateException"></exception>
        /// </summary>
        /// The aggregate exception can contain:
        /// <exception cref="Nfield.Exceptions.NfieldConflictException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldBadRequestException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldNotFoundException"></exception>
        /// <exception cref="Nfield.Exceptions.NfieldServerErrorException"></exception>        
        Task<IQueryable<Interviewer>> QueryAsync();
    }
}