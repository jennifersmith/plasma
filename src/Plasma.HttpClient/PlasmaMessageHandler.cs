using System;
using System.Diagnostics;
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

            var responseMessage = new HttpResponseMessage((HttpStatusCode) response.Status)
            {
                Content = new ByteArrayContent(response.Body),
                ReasonPhrase = response.Status.ToString(),
                RequestMessage = request,
                Version = request.Version
            };

            foreach (var item in response.Headers)
            {
                try
                {
                    responseMessage.Headers.Add(item.Key, item.Value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }


            return Task.Run(() => responseMessage, cancellationToken);
        }
    }
}