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
using System;
using System.Collections.Generic;
using System.Text;

namespace Plasma.Core
{

    public class AspNetRequest {
        private string _filePath;
        private string _pathInfo;
        private string _queryString;
        private string _method;
        private List<KeyValuePair<string, string>> _headers;
        private byte[] _body;

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

            _filePath = requestFilePath;
            _pathInfo = requestPathInfo;
            _queryString = requestQueryString;
            _method = requestMethod;
            _headers = requestHeaders;
            _body = requestBody;
        }

        public AspNetRequest(string requestPath) 
            : this(requestPath, null, null, "GET", null, null) {
        }

        public string FilePath {
            get { return _filePath; }
            set { _filePath = value; }
        }

        public string PathInfo {
            get { return _pathInfo; }
            set { _pathInfo = value; }
        }

        public string QueryString {
            get { return _queryString; }
            set { _queryString = value; }
        }

        public string Method {
            get { return _method; }
            set { _method = value; }
        }

        public List<KeyValuePair<string, string>> Headers {
            get {
                if (_headers == null) {
                    _headers = new List<KeyValuePair<string, string>>();
                }

                return _headers; 
            }
        }

        public byte[] Body {
            get { return _body; }
            set { _body = value; }
        }
    }
}
