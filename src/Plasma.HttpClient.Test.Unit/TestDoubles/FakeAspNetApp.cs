using System.Collections.Generic;
using Plasma.Core;

namespace Plasma.HttpClient.Test.Unit.TestDoubles
{
    /// <summary>
    /// Captures request sent to hosted app domain
    /// </summary>
    public class FakeAspNetApp : IRequestProcessor
    {
        public AspNetRequest LastRequest { get; set; }
        public string LastRequestPath { get; set; }

        public AspNetResponse ProcessRequest(AspNetRequest request)
        {
            LastRequest = request;
            LastRequestPath = request.FilePath;
            return new AspNetResponse("/", "", "", 200, new List<KeyValuePair<string, string>>(), new byte[0], "OK");
        }

        public AspNetResponse ProcessRequest(string requestPath)
        {
            LastRequestPath = requestPath;
            return new AspNetResponse("/", "", "", 200, new List<KeyValuePair<string, string>>(), new byte[0], "OK");
        }
    }
}