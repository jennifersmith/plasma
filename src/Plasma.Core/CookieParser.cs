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