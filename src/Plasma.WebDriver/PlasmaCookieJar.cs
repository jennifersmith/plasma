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
using System.Collections.ObjectModel;
using OpenQA.Selenium;

namespace Plasma.WebDriver
{
    public class PlasmaCookieJar : ICookieJar
    {
        private readonly WebBrowser browser;

        public PlasmaCookieJar(WebBrowser browser)
        {
            this.browser = browser;
        }

        public void AddCookie(Cookie cookie)
        {
            browser.AddCookie(cookie);
        }

        public Cookie GetCookieNamed(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteCookie(Cookie cookie)
        {
            throw new NotImplementedException();
        }

        public void DeleteCookieNamed(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllCookies()
        {
            browser.DeleteAllCookies();
        }

        public ReadOnlyCollection<Cookie> AllCookies
        {
            get { return browser.GetAllCookies(); }
        }
    }
}
