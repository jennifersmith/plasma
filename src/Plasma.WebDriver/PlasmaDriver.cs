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
using Plasma.Core;

namespace Plasma.WebDriver
{
    public class PlasmaDriver : IWebDriver
    {
        private readonly WebBrowser browser;
        private readonly PlasmaNavigation navigation;

        public PlasmaDriver(AspNetApplication aspNetApplication)
        {
            browser = new WebBrowser(aspNetApplication);
            navigation = new PlasmaNavigation(browser);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IWebElement FindElement(By mechanism)
        {
            return browser.FindElement(mechanism);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return browser.FindElements(mechanism);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Quit()
        {
            throw new NotImplementedException();
        }

        public IOptions Manage()
        {
            return new PlasmaOptions(new PlasmaCookieJar(browser));
        }

        public INavigation Navigate()
        {
            return navigation;
        }

        public ITargetLocator SwitchTo()
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<string> GetWindowHandles()
        {
            throw new NotImplementedException();
        }

        public string GetWindowHandle()
        {
            throw new NotImplementedException();
        }

        public string Url
        {
            get { return browser.RequestVirtualPath; }
            set { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }

        public string PageSource
        {
            get { return browser.PageSource(); }
        }

        public string CurrentWindowHandle
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<string> WindowHandles
        {
            get { throw new NotImplementedException(); }
        }
    }
}
