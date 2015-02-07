/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * Portions Copyright 2010 ThoughtWorks, Inc.
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
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Plasma.Core
{
    public class AspNetResponse 
    {
        private readonly IEnumerable<KeyValuePair<string, string>> _headers;
        private readonly string _hashUri;

        internal AspNetResponse(string requestVirtualPath, string queryString, string hashUri, int status, IEnumerable<KeyValuePair<string, string>> headers, byte[] body, string responseStatusDescrption) 
        {
            RequestVirtualPath = requestVirtualPath;
            QueryString = queryString;
            _hashUri = hashUri;

            Status = status;
            StatusDescription = responseStatusDescrption;
            _headers = headers ?? new Dictionary<string, string>();
            Body = body;
        }

        public int Status { get; private set; }
        public string StatusDescription { get; private set; }
        public string RequestVirtualPath { get; private set; }
        public string QueryString { get; private set; }
        public byte[] Body { get; private set; }

        public string BaseUrlWithHash { get { return RequestVirtualPath + (_hashUri ?? string.Empty); } }

        public string Url
        {
            get { return string.IsNullOrEmpty(QueryString) ? BaseUrlWithHash : BaseUrlWithHash + "?" + QueryString; }
        }

        public IEnumerable<KeyValuePair<string, string>> Headers 
        {
            get { return _headers; }
        }

        public IEnumerable<HttpCookie> Cookies 
        {
            get 
            {
                var cookieParser = new CookieParser();
                return _headers.Where(x => x.Key == "Set-Cookie").Select(x => cookieParser.ParseCookie(x.Value));
            }
        }

        public IEnumerable<string> CookieHeader
        {
            get { return _headers.Where(x => x.Key == "Set-Cookie").Select(x => x.Value); }
        }

        public string BodyAsString 
        {
            get
            {
                if (Body != null && Body.Length > 0)
                {
                    return Encoding.UTF8.GetString(Body);
                }

                return String.Empty;
            }
        }
        
        public string ToEntireResponseString() 
        {
            TextWriter output = new StringWriter();
            output.WriteLine("{0} {1}", Status, HttpWorkerRequest.GetStatusDescription(200));

            foreach (var header in Headers) 
            {
                output.WriteLine("{0}: {1}", header.Key, header.Value);
            }

            output.WriteLine();
            output.Write(BodyAsString);
            return output.ToString();
        }
    }
}