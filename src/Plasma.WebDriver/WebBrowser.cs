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
using System.Linq;
using HtmlAgilityPack;
using OpenQA.Selenium;
using Plasma.Core;
using Plasma.Http;
using Cookie = OpenQA.Selenium.Cookie;

namespace Plasma.WebDriver
{
    public class WebBrowser : ISearchContext
    {
        private readonly HttpPlasmaClient httpClient;
        private HttpPlasmaResponse response;
        private IWebElement htmlElement;
        

        public WebBrowser(AspNetApplication aspNetApplication)
        {
            httpClient = new HttpPlasmaClient(aspNetApplication);
        }

        public string PageSource()
        {
            return response.GetBody();
        }

        public void AddCookie(Cookie cookie)
        {
            httpClient.AddCookie(new System.Net.Cookie(cookie.Name, cookie.Value));
        }

        public void DeleteAllCookies()
        {
            httpClient.ClearCookies();
        }

        public void Get(string url)
        {
            response = httpClient.Get(url);
            ParseHtmlBody();
        }

        public void Post(AspNetForm form)
        {
            response = form.GenerateFormPostRequest(httpClient);
            ParseHtmlBody();
        }

        public ReadOnlyCollection<Cookie> GetAllCookies()
        {
            return httpClient
                .GetAllCookies()
                .Select(x => new Cookie(x.Name, x.Value, x.Path, x.Expires))
                .ToList()
                .AsReadOnly();
        }

        public String Title
        {
            get { return FindElement(By.TagName("title")).Text; }
        }

        public string RequestVirtualPath
        {
            get { return response.VirtualPath; }
        }

        public string QueryString
        {
            get { return response.QueryString; }
        }

        public IWebElement FindElement(By mechanism)
        {
            CheckWebBrowserHasADocument();
            return htmlElement.FindElement(mechanism);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            CheckWebBrowserHasADocument();
            return htmlElement.FindElements(mechanism);
        }

        public HtmlElement GetParentFormElement(HtmlNode node)
        {
            if (node.ParentNode == null)
            {
                throw new ArgumentException("Tried to click on an input submit button that is not contained within a form. ");
            }
            if (node.Name.ToLower() == "form")
            {
                return new HtmlElement(node, this);
            }
            if (node.ParentNode.Name.ToLower() == "form")
            {
                return new HtmlElement(node.ParentNode, this);
            }

            return GetParentFormElement(node.ParentNode);
        }

        private void CheckWebBrowserHasADocument()
        {
            if (htmlElement == null)
            {
                throw new ApplicationException(
                    string.Format("Page {0}?{1} contained no body and you attempted to find an element in it.",
                                  response.VirtualPath, response.QueryString));
            }
        }

        private void ParseHtmlBody()
        {
            HtmlNode.ElementsFlags.Remove("form");
            HtmlNode.ElementsFlags.Remove("option");
            var document = new HtmlDocument();
            try
            {
                document.LoadHtml(response.GetBody());
                htmlElement = new HtmlElement(document.DocumentNode, this);
            }
            catch (Exception)
            {
                htmlElement = null;
            }
        }
    }
}
