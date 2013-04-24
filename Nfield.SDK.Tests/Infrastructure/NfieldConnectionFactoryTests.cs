using System;

using Xunit;

namespace Nfield.Infrastructure
{
    public class NfieldConnectionFactoryTests
    {

        [Fact]
        public void CreateCreatesAnNfieldConnection()
        {
            var expectedConnection = new NfieldConnection();
            var expectedUri = new Uri("http://fake/");
            DependencyResolver.Register(type => expectedConnection, type => null);

            var actualConnection = NfieldConnectionFactory.Create(expectedUri);

            Assert.Same(expectedConnection, actualConnection);
            Assert.Equal(expectedUri, actualConnection.NfieldServerUri);
        }
    }
}
