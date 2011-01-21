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
        private readonly byte[] _body;
        private readonly IEnumerable<KeyValuePair<string, string>> _headers;
        private readonly string _requestVirtualPath;
        private readonly int _status;
        private string _bodyAsString;
        private IWebElement _htmlElement;

        internal AspNetResponse(string requestVirtualPath,
                                int status, IEnumerable<KeyValuePair<string, string>> headers, byte[] body)
        {
            _requestVirtualPath = requestVirtualPath;

            _status = status;
            _headers = headers ?? new Dictionary<string, string>();
            _body = body;
        }

        public string RequestVirtualPath
        {
            get { return _requestVirtualPath; }
        }

        public int Status
        {
            get { return _status; }
        }

        public IEnumerable<KeyValuePair<string, string>> Headers
        {
            get { return _headers; }
        }

        public byte[] Body
        {
            get { return _body; }
        }

        public string BodyAsString
        {
            get
            {
                if (_bodyAsString == null)
                {
                    if (_body != null && _body.Length > 0)
                    {
                        _bodyAsString = Encoding.UTF8.GetString(_body);
                    }
                    else
                    {
                        _bodyAsString = String.Empty;
                    }
                }

                return _bodyAsString;
            }
        }

        private IWebElement HtmlElement
        {
            get
            {
                if (_htmlElement == null)
                {
                    _htmlElement = new HtmlElement(CreateXmlDocument(BodyAsString).DocumentElement);
                }
                return _htmlElement;
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

            return new AspNetForm(_requestVirtualPath, formNode);
        }
        public AspNetForm GetFormById(string formId)
        {
            IWebElement formNode =
                HtmlElement.FindElements(By.TagName("form")).Single(e => e.GetAttribute("id").Equals(formId));

            return new AspNetForm(_requestVirtualPath, formNode);
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

        public AspNetForm GetFormByClass(string cssClass)
        {
            IWebElement formNode =
                HtmlElement.FindElements(By.TagName("form")).Single(e => e.GetAttribute("class").Equals(cssClass));

            return new AspNetForm(_requestVirtualPath, formNode);
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