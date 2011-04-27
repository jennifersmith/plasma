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
using System.Xml;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using Plasma.WebDriver.Finders;

namespace Plasma.WebDriver
{
    public class HtmlElement : IWebElement
    {
        private readonly XmlElement _xmlElement;

        public HtmlElement(XmlElement xmlElement)
        {
            _xmlElement = xmlElement;
        }

        public string InnerHtml
        {
            get { return RemoveXhtmlNamespaces(_xmlElement.InnerXml); }
        }



        public IWebElement FindElement(By mechanism)
        {
            return mechanism.FindElement(new ElementFinderContext(_xmlElement));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return mechanism.FindElements(new ElementFinderContext(_xmlElement));
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
            if (_xmlElement.Name == "option")
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
            if (!_xmlElement.HasAttribute("checked"))
            {
                XmlElement documentElement = _xmlElement.OwnerDocument.DocumentElement;
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
            if (!_xmlElement.HasAttribute("selected"))
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
            return _xmlElement.GetAttribute(attributeName, "");
        }

        public bool Toggle()
        {
            throw new NotImplementedException();
        }

        public string TagName
        {
            get { return _xmlElement.Name; }
        }

        public string Text
        {
            get { return _xmlElement.InnerText.Trim(); }
        }

        public string Value
        {
            get { return _xmlElement.GetAttribute("value", ""); }
        }

        public bool Enabled
        {
            get { return string.IsNullOrEmpty(_xmlElement.GetAttribute("disabled")); }
        }

        public bool Selected
        {
            get { return GetAttribute("checked") != String.Empty || GetAttribute("selected") != String.Empty; }
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
            if (_xmlElement.HasAttribute(attributeName))
            {
                _xmlElement.RemoveAttribute(attributeName);
            }
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return RemoveXhtmlNamespaces(_xmlElement.OuterXml);
        }

        private void SetAttribute(string attributeName, string attributeValue)
        {
            _xmlElement.SetAttribute(attributeName, attributeValue);
        }
        
    }



    public static class WebElementExtensions
    {
          public static string InnerHtml(this IWebElement webElement)
          {
              return ((HtmlElement) webElement).InnerHtml;
          }

        public static ReadOnlyCollection<IWebElement> AsReadonlyCollection(this IEnumerable<XmlElement> xmlElements)
        {
            return new ReadOnlyCollection<IWebElement>(xmlElements.Select(x=>new HtmlElement(x)).Cast<IWebElement>().ToList());
        }
    }
}