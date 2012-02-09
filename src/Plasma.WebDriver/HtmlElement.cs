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
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using OpenQA.Selenium;
using Plasma.WebDriver.Finders;

namespace Plasma.WebDriver
{
    public class HtmlElement : IWebElement
    {
        private readonly XElement xElement;
        private readonly WebBrowser webBrowser;

        public HtmlElement(XElement xElement, WebBrowser webBrowser)
        {
            this.xElement = xElement;
            this.webBrowser = webBrowser;
        }

        public string InnerHtml
        {
            get { return xElement.Nodes().Select(x => x.ToString()).Aggregate(String.Concat); }
        }

        public IWebElement FindElement(By mechanism)
        {
            return mechanism.FindElement(new ElementFinderContext(xElement, webBrowser));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return mechanism.FindElements(new ElementFinderContext(xElement, webBrowser));
        }

        public void Clear()
        {
            if ((TagName == "textarea"))
            {
                xElement.SetValue(string.Empty);
            }
            else
            {
                SetAttribute("value", string.Empty);
            }
        }

        public void SendKeys(string text)
        {
            if ((TagName == "textarea"))
            {
                xElement.SetValue(text);
            }
            else
            {
                SetAttribute("value", text);
            }
        }

        public void Submit()
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            switch (TagName)
            {
                case "a":
                    HandleClickOnAnchorElement();
                    break;

                case "option":
                    HandleSelectingOptionElement();
                    break;

                default: //handles input type = "button" and "checkbox" for now
                    HandleClickOnInputElement();
                    break;
            }
        }

        private void HandleClickOnAnchorElement()
        {
            webBrowser.Get(GetAttribute("href"));
        }

        private void HandleClickOnInputElement()
        {
            var inputType = GetAttribute("type");
            if ("checkbox".Equals(inputType))
            {
                SelectCheckBox();
                return;
            }
            if ("radio".Equals(inputType))
            {
                Toggle();
                return;
            }

            webBrowser.Post(GetParentForm());
        }

        private AspNetForm GetParentForm()
        {
            var parentFormElement = webBrowser.GetParentFormElement(xElement);
            return new AspNetForm(webBrowser.RequestVirtualPath, webBrowser.QueryString, parentFormElement, xElement);
        }

        private void HandleSelectingOptionElement()
        {
            if (ElementIsNotSelected())
            {
                var selectElement = new HtmlElement(xElement.Parent, webBrowser);
                var allOptionElements = selectElement.FindElements(By.TagName("option")).Cast<HtmlElement>();
                foreach (var element in allOptionElements)
                {
                    element.DeleteAttribute("selected");
                }
                SetAttribute("selected", "selected");
            }
        }

        private bool ElementIsNotSelected()
        {
            return !xElement.Attributes().Any(x => x.Name == "selected");
        }

        public void Select()
        {
            if (xElement.Name == "option")
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
            if (xElement.Attribute("checked") == null)
            {
                SetAttribute("checked", "checked");
            }
        }

        private void SelectOption()
        {
            if (!xElement.Attributes().Any(x=>x.Name=="selected"))
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
            var xAttribute = xElement.Attribute(attributeName);
            return xAttribute == null ? string.Empty : xAttribute.Value;
        }

        public void Toggle()
        {
            var checkedState = xElement.Attributes("checked").FirstOrDefault();
            if (checkedState==null || string.IsNullOrEmpty(checkedState.Value))
            {
                SetAttribute("checked", "checked");
                return;
            }
            SetAttribute("checked", null);
        }

        public string GetCssValue(string propertyName) {
            throw new NotImplementedException();
        }

        public string TagName
        {
            get { return xElement.Name.ToString(); }
        }

        public string Text
        {
            get { return xElement.Value.Trim(); }
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

        public Point Location {
            get { throw new NotImplementedException(); }
        }

        public Size Size {
            get { throw new NotImplementedException(); }
        }

        public bool Displayed
        {
            get
            {
                //assuming you are just waiting for it so return true
                return true;
            }   
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
            xElement.Attributes(attributeName).Remove();
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return RemoveXhtmlNamespaces(xElement.ToString());
        }

        private void SetAttribute(string attributeName, string attributeValue)
        {
            xElement.SetAttributeValue(attributeName, attributeValue);
        }
        
    }
}