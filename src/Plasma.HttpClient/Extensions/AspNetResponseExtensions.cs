using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using Plasma.Core;

namespace Plasma.HttpClient.Extensions
{
    public static class AspNetResponseExtensions
    {
        public static HttpResponseMessage ToHttpResponseMessage(this AspNetResponse response)
        {
            var responseMessage = new HttpResponseMessage((HttpStatusCode)response.Status)
            {
                Content = new ByteArrayContent(response.Body),
                ReasonPhrase = response.Status.ToString()
            };


            foreach (var item in response.Headers)
            {
                try
                {
                    responseMessage.Headers.Add(item.Key, item.Value);
                    responseMessage.Content.Headers.Add(item.Key, item.Value);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Could not add header: '{0}', Value: '{1}', Message: '{2}'", item.Key, item.Value, ex);
                }
            }
            return responseMessage;
        }
    }
}