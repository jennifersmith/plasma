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
using System.Linq;
using System.Xml.Linq;
using OpenQA.Selenium;
using Plasma.WebDriver.Finders;

namespace Plasma.WebDriver
{
    public class HtmlElement : IWebElement
    {
        private readonly XElement _xElement;

        public HtmlElement(XElement xElement)
        {
            _xElement = xElement;
        }

        public string InnerHtml
        {
            get { return _xElement.Nodes().Select(x => x.ToString()).Aggregate(String.Concat); }
        }



        public IWebElement FindElement(By mechanism)
        {
            return mechanism.FindElement(new ElementFinderContext(_xElement));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return mechanism.FindElements(new ElementFinderContext(_xElement));
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }


        public void SendKeys(string text)
        {
            SetAttribute("value", text);
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            throw new NotImplementedException();
        }

        public void Select()
        {
            if (_xElement.Name == "option")
            {
                SelectOption();
            }
            else
            {
                SelectCheckBox();
            }
        }

        private void SelectCheckBox()
        {
            if (_xElement.Attribute("checked")!=null)
            {
                XElement documentElement = _xElement.Document.Root;
                IEnumerable<IWebElement> allElementsWithName =
                    new HtmlElement(documentElement).FindElements(By.Name(GetAttribute("name")));
                foreach (HtmlElement element in allElementsWithName)
                {
                    element.DeleteAttribute("checked");
                }
                SetAttribute("checked", "checked");
            }
        }

        private void SelectOption()
        {
            if (!_xElement.Attributes().Any(x=>x.Name=="selected"))
            {
                var selectElement = FindElement(By.XPath(string.Format("ancestor::{0}", "select")));
                var allOptionElements = selectElement.FindElements(By.TagName("option"));
                foreach (HtmlElement element in allOptionElements)
                {
                    element.DeleteAttribute("selected");
                }
                SetAttribute("selected", "selected");
            }
        }

        public string GetAttribute(string attributeName)
        {
            return _xElement.Attributes(attributeName).Select(x => x.Value).FirstOrDefault();
        }

        public bool Toggle()
        {
            throw new NotImplementedException();
        }

        public string TagName
        {
            get { return _xElement.Name.ToString(); }
        }

        public string Text
        {
            get { return _xElement.Value.Trim(); }
        }

        public string Value
        {
            get { return GetAttribute("value"); }
        }

        public bool Enabled
        {
            get { return string.IsNullOrEmpty(GetAttribute("disabled")); }
        }

        public bool Selected
        {
            get { return !string.IsNullOrEmpty(GetAttribute("checked")) || !string.IsNullOrEmpty(GetAttribute("selected")); }
        }

        private static string RemoveXhtmlNamespaces(string html)
        {
            return html
                .Replace(" xmlns=\"http://www.w3.org/1999/xhtml\"", "")
                .Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "")
                .Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">", "")
                ;
        }

        private void DeleteAttribute(string attributeName)
        {
            _xElement.Attributes(attributeName).Remove();
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return RemoveXhtmlNamespaces(_xElement.ToString());
        }

        private void SetAttribute(string attributeName, string attributeValue)
        {
            _xElement.SetAttributeValue(attributeName, attributeValue);
        }
        
    }
}