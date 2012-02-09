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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.WebDriver
{
    public class WebBrowser : ISearchContext
    {
        private IList<HttpCookie> httpCookies;
        private readonly AspNetApplication aspNetApplication;
        private AspNetResponse response;
        private IWebElement htmlElement;

        public WebBrowser(AspNetApplication aspNetApplication)
        {
            this.aspNetApplication = aspNetApplication;
            httpCookies = new List<HttpCookie>();
        }

        public string PageSource()
        {
            return response.ToEntireResponseString();
        }

        public void AddCookie(Cookie cookie)
        {
            httpCookies.Add(new HttpCookie(cookie.Name, cookie.Value));
        }

        public void DeleteAllCookies()
        {
            httpCookies = new List<HttpCookie>();
        }

        public void Get(string url)
        {
            ProcessRequest(url);
            ExtractCookies();
            FollowAnyRedirect();
            ParseHtmlBody();
        }

        public void Post(AspNetForm form)
        {
            ProcessRequest(form.GenerateFormPostRequest());
            ExtractCookies();
            FollowAnyRedirect();
            ParseHtmlBody();
        }

        public ReadOnlyCollection<Cookie> GetAllCookies()
        {
            return httpCookies.Select(x => new Cookie(x.Name, x.Value, x.Path, x.Expires)).ToList().AsReadOnly();
        }

        public String Title
        {
            get { return FindElement(By.TagName("title")).Text; }
        }

        public string RequestVirtualPath
        {
            get { return response.RequestVirtualPath; }
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

        public HtmlElement GetParentFormElement(XElement node)
        {
            if (node.Parent == null)
            {
                throw new ArgumentException("Tried to click on an input submit button that is not contained within a form. ");
            }
            if (node.Name.ToString().ToLower() == "form")
            {
                return new HtmlElement(node, this);
            }
            if (node.Parent.Name.ToString().ToLower() == "form")
            {
                return new HtmlElement(node.Parent, this);
            }

            return GetParentFormElement(node.Parent);
        }

        private void CheckWebBrowserHasADocument()
        {
            if (htmlElement == null)
            {
                throw new ApplicationException(
                    string.Format("Page {0}?{1} contained no body and you attempted to find an element in it.",
                                  response.RequestVirtualPath, response.QueryString));
            }
        }

        private void ExtractCookies()
        {
            if (response.Cookies == null) return;

            foreach (var cookie in response.Cookies)
            {
                if (!httpCookies.Any(x => x.Name == cookie.Name))
                {
                    httpCookies.Add(cookie);
                }
                else
                {
                    httpCookies = httpCookies.Where(x => x.Name != cookie.Name).ToList();

                    if (cookie.Expires > DateTime.Now)
                    {
                        httpCookies.Add(cookie);
                    }
                }
            }
        }

        private void FollowAnyRedirect()
        {
            while (response.Status == 302 || response.Status == 301)
            {
                FollowRedirectInResponse();
                ExtractCookies();
            }
        }

        private void FollowRedirectInResponse()
        {
            if (response.Status != 302 && response.Status != 301) 
            {
                throw new Exception("Expected Redirect status (302), Got " + response.Status);
            }

            var location = response.Headers.Where(x => x.Key == "Location").First().Value;
            var locationUri = new Uri(location, UriKind.RelativeOrAbsolute);
            var relativeLocation = locationUri.IsAbsoluteUri ? locationUri.PathAndQuery : location;
            ProcessRequest(relativeLocation);
        }

        private void ProcessRequest(string url)
        {
            var request = new AspNetRequest(url);
            ProcessRequest(request);
        }

        private void ProcessRequest(AspNetRequest request)
        {
            AddHostHeaderToRequest(request);
            AddCookiesToRequest(request);
            response = aspNetApplication.ProcessRequest(request);
        }

        private void AddCookiesToRequest(AspNetRequest request)
        {
            if (httpCookies != null) request.AddCookies(httpCookies);
        }

        private static void AddHostHeaderToRequest(AspNetRequest request)
        {
            request.Headers.Add(new KeyValuePair<string, string>("Host", "localhost"));
        }

        private void ParseHtmlBody()
        {
            htmlElement = IsHtml() ? new HtmlElement(Parse(response.BodyAsString), this) : null;
        }

        private bool IsHtml()
        {
            return response.BodyAsString.Contains("<html");
        }

        private static XElement Parse(string html)
        {
            var xmlReaderSettings = new XmlReaderSettings
            {
                XmlResolver = new LocalEntityResolver(),
                ProhibitDtd = false
            };

            XDocument xmlDocument;
            try
            {
                xmlDocument = XDocument.Load(XmlReader.Create(new StringReader(html), xmlReaderSettings));
                return RemoveNamespaces(xmlDocument).Root;
            }
            catch (XmlException)
            {
                Console.Out.WriteLine("Failed to parse response as html:\n{0}", html);
            }
            return new XElement("empty");
        }

        private static XDocument RemoveNamespaces(XDocument xDocument)
        {
            IEnumerable<XElement> elementsWithNamespaces = xDocument.Descendants().Where(x => x.Name.Namespace != XNamespace.None);
            foreach (var element in elementsWithNamespaces)
            {
                element.Name = RemoveNamespace(element.Name);
                element.ReplaceAttributes(element.Attributes().Select(RemoveNamespace));
            }
            xDocument.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();
            return xDocument;
        }

        private static XName RemoveNamespace(XName xName)
        {
            return XNamespace.None.GetName(xName.LocalName);
        }

        private static XAttribute RemoveNamespace(XAttribute xAttribute)
        {
            if (xAttribute.Name.Namespace != XNamespace.None)
            {
                return new XAttribute(RemoveNamespace(xAttribute.Name), xAttribute.Value);
            }
            return xAttribute;
        }
    }
}
