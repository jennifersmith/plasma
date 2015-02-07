using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Plasma.Core;

namespace Plasma.HttpClient
{
    public class PlasmaMessageHandler : HttpMessageHandler
    {
        private readonly AspNetApplication _application;

        public PlasmaMessageHandler(AspNetApplication application)
        {
            _application = application;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = _application.ProcessRequest(request.RequestUri.AbsolutePath);

            var responseMessage = new HttpResponseMessage((HttpStatusCode)response.Status);


            return new Task<HttpResponseMessage>(() => responseMessage);
        }
    }
}