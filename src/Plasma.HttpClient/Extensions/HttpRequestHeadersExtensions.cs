using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace Plasma.HttpClient.Extensions
{
    public static class HttpRequestHeadersExtensions
    {
        public static List<KeyValuePair<string, string>> ToKvp(this HttpRequestHeaders headers)
        {
            return headers.Select(header => new KeyValuePair<string, string>(header.Key, string.Join(";", header.Value))).ToList();
        } 

        public static List<KeyValuePair<string, string>> ToKvp(this HttpContentHeaders headers)
        {
            return headers.Select(header => new KeyValuePair<string, string>(header.Key, string.Join(";", header.Value))).ToList();
        } 
    }
}