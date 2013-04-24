using System;

using Nfield.Infrastructure;

namespace Nfield.Extensions
{
    /// <summary>
    /// Extensions to NfieldConnection class to make the asynchronous methods synchronous
    /// </summary>
    public static class NfieldConnectionExtensions
    {
        /// <summary>
        /// A synchronous version of <see cref="INfieldConnection.SignInAsync"/>.
        /// </summary>
        /// <param name="nfieldConnection">The <see cref="INfieldConnection"/> to sign in</param>
        /// <param name="domainName">The name of the domain to sign in to</param>
        /// <param name="username">The username to sign in</param>
        /// <param name="password">The password to use for authentication</param>
        /// <returns><c>true</c> if sign in was successful, <c>false</c> otherwise.</returns>
        public static bool SignIn(this INfieldConnection nfieldConnection, string domainName, string username, string password)
        {
            var result = false;

            nfieldConnection.SignInAsync(domainName, username, password)
                            .ContinueWith(task => result = task.Result)
                            .Wait();

            return result;
        }

    }
}
