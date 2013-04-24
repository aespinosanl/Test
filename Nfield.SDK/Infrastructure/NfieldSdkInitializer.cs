using System;
using Nfield.Services;
using Nfield.Services.Implementation;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Used to register the types into the user defined IoC container.
    /// This method must be called to initialize the SDK.   
    /// </summary>
    public static class NfieldSdkInitializer
    {
        /// <summary>
        /// Method that registers all known types by calling the delegates provided.
        /// </summary>
        /// <param name="registerTransient">Method that registers a Transient type.</param>
        /// <param name="registerSingleton">Method that registers a Singleton.</param>
        /// <param name="registerInstance">Method that registers an instance.</param>
        public static void Initialize(Action<Type, Type> registerTransient, 
                                      Action<Type, Type> registerSingleton,
                                      Action<Type, Object> registerInstance)
        {
            // TODO register all types.
            registerTransient(typeof(NfieldConnection), typeof(NfieldConnection));
            registerTransient(typeof(INfieldInterviewersService), typeof(NfieldInterviewersService));
            registerTransient(typeof(IHttpClient), typeof(HttpClientWrapper));
        }

    }
}