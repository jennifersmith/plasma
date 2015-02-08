using System.Net.Http;
using System.Threading;
using Plasma.Core;

namespace Plasma.HttpClient.Test.Unit.TestDoubles
{
    /// <summary>
    /// Extended to invoke internal method.
    /// </summary>
    public class TestPlasmaHandler : PlasmaMessageHandler 
    {
        public TestPlasmaHandler(IRequestProcessor application) : base(application) { }
        public void InvokeInternalSendAsync(HttpRequestMessage message) { SendAsync(message, new CancellationToken()); }
    }
}