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
