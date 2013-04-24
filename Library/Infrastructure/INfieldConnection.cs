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