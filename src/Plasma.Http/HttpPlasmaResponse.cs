using System;
using System.Net;
using Plasma.Core;

namespace Plasma.Http
{
    public class HttpPlasmaResponse
    {
        private readonly AspNetResponse response;

        public HttpPlasmaResponse(AspNetResponse response)
        {
            this.response = response;
        }

        public string VirtualPath
        {
            get { return response.RequestVirtualPath; }
        }

        public string QueryString
        {
            get { return response.QueryString; }
        }

        public HttpStatusCode StatusCode
        {
            get { return (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Status.ToString()); }
        }

        public string GetBody()
        {
            return response.BodyAsString;
        }
    }
}
