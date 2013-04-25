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
using System.Threading.Tasks;

namespace Nfield.Infrastructure
{

    /// <summary>
    /// Represents a connection to an Nfield server. use the <see cref="INfieldConnection"/> to gain access to the 
    /// various services that Nfield provides.
    /// Before you can access the 
    /// </summary>
    public interface INfieldConnection : IServiceProvider, IDisposable
    {
        /// <summary>
        /// the server Uri for Nfield.
        /// </summary>
        Uri NfieldServerUri { get; }

        /// <summary>
        /// Sign into the specified domain, using the specified username and password
        /// </summary>
        /// <returns><c>true</c> if sign-in was successful, <c>false</c> otherwise.</returns>
        Task<bool> SignInAsync(string domainName, string username, string password);

        /// <summary>
        /// Return the specified service <typeparamref name="TService"/> provided by Nfield.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns>An implementation of the specified service.</returns>
        TService GetService<TService>();

    }

}