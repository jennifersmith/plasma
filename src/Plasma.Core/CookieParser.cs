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
using System.Linq;
using System.Web;

namespace Plasma.Core
{
    public class CookieParser
    {
        public HttpCookie ParseCookie(string rawCookie)
        {
            string[] cookieParts = rawCookie.Split(';');
            string valuePart = cookieParts[0];
            int idx = valuePart.IndexOf("=");
            string cookieName = valuePart.Substring(0, idx);
            string cookieValue = valuePart.Substring(idx + 1, valuePart.Length - idx - 1);


            string expiresDateString = null;
            foreach (string cookiePart in cookieParts.Skip(1))
            {
                if (cookiePart.Trim().StartsWith("expires="))
                {
                    expiresDateString = cookiePart.Split('=')[1];
                }
            }

            var cookie = new HttpCookie(cookieName, cookieValue);
            if (expiresDateString != null)
            {
                cookie.Expires = DateTime.Parse(expiresDateString);
            }
            return cookie;
        }
    }
}