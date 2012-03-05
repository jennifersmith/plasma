/* **********************************************************************************
 *
 * Copyright 2010 ThoughtWorks, Inc.  
 * ThoughtWorks provides the software "as is" without warranty of any kind, either express or implied, including but not limited to, 
 * the implied warranties of merchantability, satisfactory quality, non-infringement and fitness for a particular purpose.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).  
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Plasma.Core;

namespace Plasma.Http
{
    public class HttpPlasmaClient
    {
        private const int CookieCapacity = 100;

        private readonly AspNetApplication application;
        private CookieContainer cookieContainer;

        public HttpPlasmaClient(AspNetApplication application)
        {
            this.application = application;
            cookieContainer = new CookieContainer(CookieCapacity);
        }

        public HttpPlasmaResponse Get(string url, string queryString = "", WebHeaderCollection headers = null)
        {
            var request = new AspNetRequest(url, null, queryString, "GET", CreateRequestHeaders(headers), null);
            return new HttpPlasmaResponse(ProcessRequest(request));
        }

        public HttpPlasmaResponse Post(string url, string queryString = "", byte[] body = null, WebHeaderCollection headers = null)
        {
            var request = new AspNetRequest(url, null, queryString, "POST", CreateRequestHeaders(headers), body);
            return new HttpPlasmaResponse(ProcessRequest(request));
        }

        public void AddCookie(Cookie cookie)
        {
            cookieContainer.Add(new Uri("http://localhost"), cookie);
        }

        public void ClearCookies()
        {
            cookieContainer = new CookieContainer(CookieCapacity);
        }

        public IEnumerable<Cookie> GetAllCookies()
        {
            return cookieContainer
                .GetCookies(new Uri("http://localhost"))
                .OfType<Cookie>();
        }

        private List<KeyValuePair<string, string>> CreateRequestHeaders(WebHeaderCollection headers)
        {
            var requestHeaders = new List<KeyValuePair<string, string>>();
            requestHeaders.Add(new KeyValuePair<string, string>("Host", "localhost"));
            requestHeaders.Add(new KeyValuePair<string, string>("Cookie", cookieContainer.GetCookieHeader(new Uri("http://localhost"))));
            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    requestHeaders.Add(new KeyValuePair<string, string>((string)key, headers[(string)key]));
                }
            }
            return requestHeaders;
        }

        private AspNetResponse ProcessRequest(AspNetRequest request)
        {
            var response = application.ProcessRequest(request);
            ExtractCookies(response);
            return FollowAnyRedirect(response);
        }

        private AspNetResponse FollowAnyRedirect(AspNetResponse response)
        {
            if (response.Status == 302 || response.Status == 301)
            {
                var location = response.Headers.First(x => x.Key == "Location").Value;
                var locationUri = new Uri(location, UriKind.RelativeOrAbsolute);
                var url = locationUri.IsAbsoluteUri ? locationUri.PathAndQuery : location;
                var request = new AspNetRequest(url, null, null, "GET", CreateRequestHeaders(new WebHeaderCollection()), null);
                return ProcessRequest(request);
            }
            return response;
        }

        private void ExtractCookies(AspNetResponse response)
        {
            if (cookieContainer == null) return;

            foreach (var cookie in response.CookieHeader)
            {
                cookieContainer.SetCookies(new Uri("http://localhost"), cookie);
            }
        }
    }
}
