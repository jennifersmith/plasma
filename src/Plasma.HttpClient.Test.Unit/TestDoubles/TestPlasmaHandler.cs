using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Plasma.Core;

namespace Plasma.HttpClient.Test.Unit.TestDoubles
{
    /// <summary>
    /// Extended to invoke internal method.
    /// </summary>
    public class TestPlasmaHandler : PlasmaMessageHandler
    {
        public TestPlasmaHandler(IRequestProcessor application) : base(application)
        {
        }

        public Task<HttpResponseMessage> InvokeInternalSendAsync(HttpRequestMessage message)
        {
            return SendAsync(message, new CancellationToken());
        }
    }
}