/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Plasma.Core
{
    public class AspNetRequest {
        public AspNetRequest(
            string requestFilePath,
            string requestPathInfo,
            string requestQueryString,
            string requestMethod,
            List<KeyValuePair<string, string>> requestHeaders,
            byte[] requestBody) {
            // BugBug: Start Hack

            string[] requestFilePathPieces = requestFilePath.Split('?');

            if (requestFilePathPieces.Length > 1) {
                requestFilePath = requestFilePathPieces[0];
                requestQueryString = requestFilePathPieces[1];
            }

            // BugBug: End Hack

            FilePath = requestFilePath;
            PathInfo = requestPathInfo;
            QueryString = requestQueryString;
            Method = requestMethod;
            Headers = requestHeaders ?? new List<KeyValuePair<string, string>>();
            Body = requestBody;
        }

        public AspNetRequest(string requestPath)
            : this(requestPath, null, null, "GET", null, null) {
        }

        public string FilePath { get; set; }

        public string PathInfo { get; set; }

        public string QueryString { get; set; }

        public string Method { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; private set; }

        public byte[] Body { get; set; }

        public void AddCookies(IEnumerable<HttpCookie> cookies) {
            var stringBuilder = new StringBuilder();

            foreach (HttpCookie cookie in cookies)
            {
                stringBuilder.Append(string.Format("{0}={1}; ", cookie.Name, cookie.Value));
            }

            Headers.Add(new KeyValuePair<string, string>("Cookie", stringBuilder.ToString()));
        }
    }
}