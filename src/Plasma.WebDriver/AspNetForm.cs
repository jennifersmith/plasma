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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using OpenQA.Selenium;
using Plasma.Core;
using Plasma.Http;

namespace Plasma.WebDriver
{
    public class AspNetForm : NameValueCollection
    {
        private readonly IWebElement _formWebElement;
        private readonly HashSet<string> _fileControls = new HashSet<string>();
        private string _action;
        private string _method;
        private HtmlNode clickedElement;

        internal AspNetForm(string requestVirtualPath, string queryString, IWebElement formWebElement, HtmlNode clickedElement)
        {
            _formWebElement = formWebElement;
            this.clickedElement = clickedElement;
            // form's method
            string formMethod = formWebElement.GetAttribute("method");
            _method = string.IsNullOrEmpty(formMethod) ? "POST" : formMethod;

            // form's action
            string formAction = formWebElement.GetAttribute("action");
            string requestUrl = string.IsNullOrEmpty(queryString) ? requestVirtualPath : requestVirtualPath + "?" + queryString;
            _action = string.IsNullOrEmpty(formAction)
                         ? requestUrl
                         : VirtualPathUtility.Combine(requestVirtualPath, formAction);

            // populate the dictionary with form fields
            RetrieveFormFields(formWebElement);
        }

        public IWebElement FormWebElement
        {
            get { return _formWebElement; }
        }

        public string Method
        {
            get { return _method; }
            set { _method = value; }
        }

        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }

        public HttpPlasmaResponse GenerateFormPostRequest(HttpPlasmaClient client)
        {
            // path and query string
            var headers = new WebHeaderCollection();
            string path;
            string query;

            var iQuery = _action.IndexOf('?');

            if (iQuery >= 0)
            {
                path = _action.Substring(0, iQuery);
                query = _action.Substring(iQuery + 1);
            }
            else
            {
                path = _action;
                query = null;
            }

            if (_fileControls.Count > 0)
            {
                var multipartFormBody = new MultipartFormBody(this, _fileControls);

                headers.Add(HttpRequestHeader.ContentLength, multipartFormBody.ContentLength);
                headers.Add(HttpRequestHeader.ContentType, multipartFormBody.ContentType);
                var body = multipartFormBody.FormBodyData();
                return client.Post(path, query, body, headers);
            }

            var formData = GenerateFormDataAsString();

            if (string.Compare(_method, "GET", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return client.Get(path, formData);
            }

            var formBody = Encoding.UTF8.GetBytes(formData);
            headers.Add(HttpRequestHeader.ContentLength, formBody.Length.ToString());
            headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            return client.Post(path, query, formBody, headers);
        }

        private string GenerateFormDataAsString()
        {
            int n = Count;

            if (n == 0)
            {
                return string.Empty;
            }

            var s = new StringBuilder();

            for (int i = 0; i < n; i++)
            {
                string key = HttpUtility.UrlEncodeUnicode(GetKey(i));

                if (!string.IsNullOrEmpty(key))
                {
                    key += "=";
                }

                if (i > 0)
                {
                    s.Append('&');
                }

                var values = (ArrayList)BaseGet(i);

                if (values == null || values.Count == 0)
                {
                    s.Append(key);
                }
                else if (values.Count == 1)
                {
                    s.Append(key);
                    s.Append(HttpUtility.UrlEncodeUnicode((string)values[0]));
                }
                else
                {
                    for (int j = 0; j < values.Count; j++)
                    {
                        if (j > 0)
                        {
                            s.Append('&');
                        }

                        s.Append(key);
                        s.Append(HttpUtility.UrlEncodeUnicode((string)values[j]));
                    }
                }
            }

            return s.ToString();
        }

        private void RetrieveFormFields(IWebElement node)
        {
            foreach (IWebElement childNode in node.FindElements(By.XPath("./*")))
            {
                if (AddFormField(childNode))
                {
                    // if this a form field already no need to recurse
                    continue;
                }

                // recurse down
                RetrieveFormFields(childNode);
            }
        }

         private bool AddFormField(IWebElement node)
        {
            if (NodeNameIs(node, "input"))
            {
                string type = node.GetAttribute("type");

                if (IsSupportedInput(type))
                {
                    AddFieldValue(node, node.GetAttribute("value"));
                }
                else if (StringsEqual(type, "checkbox"))
                {
                    if (NodeHasAttributeWithValue(node, "checked", "checked"))
                    {
                        AddFieldValue(node, node.GetAttribute("value"));
                    }
                }
                else if (StringsEqual(type, "radio"))
                {
                    if (NodeHasAttributeWithValue(node, "checked", "checked"))
                    {
                        AddFieldValue(node, node.GetAttribute("value"));
                    }
                }
                else if (StringsEqual(type, "file"))
                {
                    _fileControls.Add(node.GetAttribute("name"));
                    AddFieldValue(node, node.GetAttribute("value"));
                }
                else if (StringsEqual(type, "submit") && node.GetAttribute("name") == GetClickedElementAttribute("name"))
                {
                    AddFieldValue(node, node.GetAttribute("value"));
                }
            }
            else if (NodeNameIs(node, "textarea"))
            {
                AddFieldValue(node, node.Text);
            }
            else if (NodeNameIs(node, "select"))
            {
                foreach (IWebElement optionNode in node.FindElements(By.TagName("option")))
                {
                    if (NodeHasAttributeWithValue(optionNode, "selected", "selected"))
                    {
                        AddFieldValue(node, optionNode.GetAttribute("value"));
                    }
                }
            }
            else if (NodeNameIs(node, "button"))
            {
                var type = node.GetAttribute("type");
                if (StringsEqual(type, "submit") && node.GetAttribute("name") == GetClickedElementAttribute("name"))
                {
                    AddFieldValue(node, node.GetAttribute("value"));
                }
            }
            else
            {
                // not a field
                return false;
            }

            // field processed
            return true;
        }

        private static bool IsSupportedInput(string type)
        {
            var inputTypes = new List<string> { "text", "password", "hidden", "email", "number", "week", "month", "date", "time", "datetime", "range", "tel","url" };
            return inputTypes.Contains(type.ToLower());
            //return StringsEqual(type, "text") || StringsEqual(type, "password") || StringsEqual(type, "hidden");
        }

        private string GetClickedElementAttribute(string name)
        {
            return clickedElement.GetAttributeValue(name, string.Empty);
        }

        private void AddFieldValue(IWebElement node, string fieldValue)
        {
            string fieldName = node.GetAttribute("name");

            if (!string.IsNullOrEmpty(fieldName))
            {
                Add(fieldName, fieldValue);
            }
        }

        private static bool NodeNameIs(IWebElement node, string name)
        {
            return StringsEqual(node.TagName, name);
        }

        private static bool NodeHasAttributeWithValue(IWebElement node, string attributeName, string attributeValue)
        {
            return StringsEqual(node.GetAttribute(attributeName), attributeValue);
        }

        private static bool StringsEqual(string s1, string s2)
        {
            return (string.Compare(s1, s2, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}