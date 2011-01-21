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
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using OpenQA.Selenium;

namespace Plasma.Core
{
    public class AspNetResponse : ISearchContext
    {
        private readonly byte[] body;
        private readonly IEnumerable<KeyValuePair<string, string>> headers;
        private readonly string requestVirtualPath;
        private readonly int status;
        private string bodyAsString;
        private IWebElement htmlElement;

        internal AspNetResponse(string requestVirtualPath,
                                int status, IEnumerable<KeyValuePair<string, string>> headers, byte[] body)
        {
            this.requestVirtualPath = requestVirtualPath;

            this.status = status;
            this.headers = headers ?? new Dictionary<string, string>();
            this.body = body;
        }

        public string RequestVirtualPath
        {
            get { return requestVirtualPath; }
        }

        public int Status
        {
            get { return status; }
        }

        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get { return headers; }
        }

        public byte[] Body
        {
            get { return body; }
        }

        public string BodyAsString
        {
            get
            {
                if (bodyAsString == null)
                {
                    if (body != null && body.Length > 0)
                    {
                        bodyAsString = Encoding.UTF8.GetString(body);
                    }
                    else
                    {
                        bodyAsString = String.Empty;
                    }
                }

                return bodyAsString;
            }
        }

        private IWebElement HtmlElement
        {
            get
            {
                if (htmlElement == null)
                {
                    htmlElement = new HtmlElement(CreateXmlDocument(BodyAsString).DocumentElement);
                }
                return htmlElement;
            }
        }

        public String Title
        {
            get { return FindElement(By.TagName("title")).Text; }
        }

        public IWebElement FindElement(By mechanism)
        {
            return HtmlElement.FindElement(mechanism);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return HtmlElement.FindElements(mechanism);
        }


        public AspNetForm GetForm()
        {
            IWebElement formNode = HtmlElement.FindElement(By.TagName("form"));

            return new AspNetForm(requestVirtualPath, formNode);
        }
        public AspNetForm GetFormById(string formId)
        {
            IWebElement formNode =
                HtmlElement.FindElements(By.TagName("form")).Single(e => e.GetAttribute("id").Equals(formId));

            return new AspNetForm(requestVirtualPath, formNode);
        }


        public string ToEntireResponseString()
        {
            TextWriter output = new StringWriter();

            output.WriteLine("{0} {1}", Status, HttpWorkerRequest.GetStatusDescription(200));

            foreach (var header in Headers)
            {
                output.WriteLine("{0}: {1}", header.Key, header.Value);
            }

            output.WriteLine();

            output.Write(BodyAsString);

            return output.ToString();
        }

        public string InnerHtml(IWebElement element)
        {
            return ((HtmlElement)element).InnerHtml;
        }

        public AspNetForm GetFormByClass(string cssClass)
        {
            IWebElement formNode =
                HtmlElement.FindElements(By.TagName("form")).Single(e => e.GetAttribute("class").Equals(cssClass));

            return new AspNetForm(requestVirtualPath, formNode);
        }

        private static XmlDocument CreateXmlDocument(string html)
        {
            var doc = new XmlDocument();
            var xmlReaderSettings = new XmlReaderSettings
            {
                XmlResolver = new LocalEntityResolver(),
                ProhibitDtd = false
            };

            try
            {
                doc.Load(XmlReader.Create(new StringReader(html), xmlReaderSettings));
            }
            catch (XmlException)
            {
                Console.Out.WriteLine("Failed to parse response as html:\n{0}", html);
                throw;
            }
            return doc;
        }
    }
}