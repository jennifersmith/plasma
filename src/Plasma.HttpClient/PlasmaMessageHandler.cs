using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Plasma.Core;
using Plasma.HttpClient.Extensions;

namespace Plasma.HttpClient
{
    public class PlasmaMessageHandler : HttpMessageHandler
    {
        private readonly IRequestProcessor _application;

        public PlasmaMessageHandler(IRequestProcessor application)
        {
            _application = application;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var body = (byte[])null;
            var requestHeaders = request.Headers.ToKvp();

            if (request.Content != null)
            {
                body = request.Content.ReadAsByteArrayAsync().Result;
                requestHeaders.AddRange(request.Content.Headers.ToKvp());
            }

            var aspNetRequest = new AspNetRequest(request.RequestUri.AbsolutePath,
                                                  null,
                                                  request.RequestUri.Query,
                                                  request.Method.ToString(),
                                                  requestHeaders,
                                                  body);
            
            var response = _application.ProcessRequest(aspNetRequest).ToHttpResponseMessage();
            response.RequestMessage = request;
            response.Version = request.Version;

            return Task.Run(() => response, cancellationToken);
        }
    }
}