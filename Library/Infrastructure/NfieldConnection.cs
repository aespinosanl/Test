using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;

namespace Nfield.Infrastructure
{

    internal class NfieldConnection : INfieldConnection, INfieldConnectionClient
    {
        #region Implementation of IServiceProvider

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        public object GetService(Type serviceType)
        {
            if (serviceType == null) {
                throw new ArgumentNullException("serviceType");
            }


            var serviceInstance = DependencyResolver.Current.Resolve(serviceType);

            var nfieldConnectionClientObject = serviceInstance as INfieldConnectionClientObject;
            if (nfieldConnectionClientObject != null)
            {
                nfieldConnectionClientObject.InitializeNfieldConnection(this);
            }
            return serviceInstance;
        }

        #endregion

        #region Implementation of INfieldConnection

        public Uri NfieldServerUri { get; internal set; }

        /// <summary>
        /// Sign into the specified domain, using the specified username and password
        /// </summary>
        /// <returns><c>true</c> if sign-in was successful, <c>false</c> otherwise.</returns>
        public async Task<bool> SignInAsync(string domainName, string username, string password)
        {
            if (Client == null)
            {
                Client = (IHttpClient)DependencyResolver.Current.Resolve(typeof(IHttpClient));
            }
            var data = new Dictionary<string, string>
                {
                    {"Domain", domainName},
                    {"Username", username},
                    {"Password", password}
                };
            var content = new FormUrlEncodedContent(data);

            var result = await Client.PostAsync(NfieldServerUri + @"/SignIn", content);

            return result.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Return the specified service <typeparamref name="TService"/> provided by Nfield.
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns>An implementation of the specified service.</returns>
        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }

        #endregion

        #region Implementation of IDisposable

        ~NfieldConnection()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || Client == null)
                return;

            Client.Dispose();
            Client = null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of INfieldConnectionClient

        public IHttpClient Client { get; private set; }

        #endregion
    }

}