using System;
using Newtonsoft.Json;

namespace Nfield.Models
{
    /// <summary>
    /// Holds all properties of an interviewer
    /// </summary>
    public class Interviewer
    {
        /// <summary>
        /// Unique id of the interviewer
        /// </summary>
        [JsonProperty]
        public string InterviewerId { get; internal set; }

        /// <summary>
        /// Public id of the interviewer, is unique within a domain
        /// </summary>
        public string ClientInterviewerId { get; set; }

        /// <summary>
        /// User name interviewer uses to sign in
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email address of interviewer
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Telephone number of interviewer
        /// </summary>
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Password of interviewer
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Total number of successful interviews the interviewer has done
        /// </summary>
        [JsonProperty]
        public int SuccessfulCount { get; internal set; }

        /// <summary>
        /// Total number of unsuccessful interviews the interviewer has done
        /// </summary>
        [JsonProperty]
        public int UnsuccessfulCount { get; internal set; }

        /// <summary>
        /// Total number of broken of interviews the interviewer has done
        /// </summary>
        [JsonProperty]
        public int DroppedOutCount { get; internal set; }

        /// <summary>
        /// Last time the device of the interviewer was synchronized with the server
        /// </summary>
        [JsonProperty]
        public DateTime? LastSyncTime { get; internal set; }

        /// <summary>
        /// Last time the password of the interviewer was changed
        /// </summary>
        [JsonProperty]
        public DateTime? LastPasswordChangeTime { get; internal set; }
    }
}