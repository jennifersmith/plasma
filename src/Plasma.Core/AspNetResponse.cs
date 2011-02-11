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
using System.Text;
using System.Web;

namespace Plasma.Core
{
    public class AspNetResponse {
        private readonly byte[] _body;
        private readonly IEnumerable<KeyValuePair<string, string>> _headers;
        private readonly string _requestVirtualPath;
        private readonly int _status;
        private string _bodyAsString;

        internal AspNetResponse(string requestVirtualPath,
                                int status, IEnumerable<KeyValuePair<string, string>> headers, byte[] body) {
            _requestVirtualPath = requestVirtualPath;

            _status = status;
            _headers = headers ?? new Dictionary<string, string>();
            _body = body;
        }

        public string RequestVirtualPath {
            get { return _requestVirtualPath; }
        }

        public int Status {
            get { return _status; }
        }

        public IEnumerable<KeyValuePair<string, string>> Headers {
            get { return _headers; }
        }

        public byte[] Body {
            get { return _body; }
        }

        public string BodyAsString {
            get {
                if (_bodyAsString == null) {
                    if (_body != null && _body.Length > 0) {
                        _bodyAsString = Encoding.UTF8.GetString(_body);
                    } else {
                        _bodyAsString = String.Empty;
                    }
                }

                return _bodyAsString;
            }
        }


        public string ToEntireResponseString() {
            TextWriter output = new StringWriter();

            output.WriteLine("{0} {1}", Status, HttpWorkerRequest.GetStatusDescription(200));

            foreach (var header in Headers) {
                output.WriteLine("{0}: {1}", header.Key, header.Value);
            }

            output.WriteLine();

            output.Write(BodyAsString);

            return output.ToString();
        }
                
    }
}