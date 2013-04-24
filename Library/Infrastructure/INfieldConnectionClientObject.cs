using System;

namespace Nfield.Infrastructure
{

    internal interface INfieldConnectionClientObject
    {

        INfieldConnectionClient ConnectionClient { get; }

        void InitializeNfieldConnection(INfieldConnectionClient connection);

    }

}