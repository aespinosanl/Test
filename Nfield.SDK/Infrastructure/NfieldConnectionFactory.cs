using System;
using System.Net;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Creates connections to an Nfield server. An Nfield server is identified by a <see cref="Uri"/>.
    /// </summary>
    public static class NfieldConnectionFactory
    {

        /// <summary>
        /// Create a connection to the Nfield server on the specified <paramref name="nfieldServerUri"/>.
        /// </summary>
        /// <param name="nfieldServerUri"></param>
        /// <returns></returns>
        public static INfieldConnection Create(Uri nfieldServerUri)
        {
            var connection = DependencyResolver.Current.Resolve<NfieldConnection>();
            connection.NfieldServerUri = nfieldServerUri;

            return connection;
        }

    }

}
