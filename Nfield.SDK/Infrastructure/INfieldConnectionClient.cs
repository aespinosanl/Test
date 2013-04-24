using System;
using System.Net.Http;

namespace Nfield.Infrastructure
{
    internal interface INfieldConnectionClient
    {
        IHttpClient Client { get; }

        Uri NfieldServerUri { get; }
    }

}
