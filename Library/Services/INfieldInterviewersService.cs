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
        Task UpdateAsync(Interviewer interviewer);

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