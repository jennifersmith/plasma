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
