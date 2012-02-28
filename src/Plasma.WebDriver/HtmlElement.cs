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
using System.Net;
using HtmlAgilityPack;
using OpenQA.Selenium;
using Plasma.WebDriver.Finders;

namespace Plasma.WebDriver
{
    public class HtmlElement : IWebElement
    {
        private readonly HtmlNode currentNode;
        private readonly WebBrowser webBrowser;

        public HtmlElement(HtmlNode currentNode, WebBrowser webBrowser)
        {
            this.currentNode = currentNode;
            this.webBrowser = webBrowser;
        }

        public string InnerHtml
        {
            get { return currentNode.InnerHtml; }
        }

        public IWebElement FindElement(By mechanism)
        {
            return mechanism.FindElement(new ElementFinderContext(currentNode, webBrowser));
        }

        public ReadOnlyCollection<IWebElement> FindElements(By mechanism)
        {
            return mechanism.FindElements(new ElementFinderContext(currentNode, webBrowser));
        }

        public void Clear()
        {
            if ((TagName == "textarea"))
            {
                currentNode.InnerHtml = string.Empty;
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
                currentNode.InnerHtml = text;
            }
            else
            {
                SetAttribute("value", text);
            }
        }

        public void Submit()
        {
            HandleFormSubmit();
        }

        public void Click()
        {
            HandleSelectingElements();
            HandleClickingElements();
        }

        private bool ElementIsNotSelected()
        {
            return currentNode.GetAttributeValue("selected", null) == null;
        }

        public string GetAttribute(string attributeName)
        {
            return WebUtility.HtmlDecode(currentNode.GetAttributeValue(attributeName, string.Empty));
        }

        public string GetCssValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public string TagName
        {
            get { return currentNode.Name; }
        }

        public string Text
        {
            get { return WebUtility.HtmlDecode(currentNode.InnerText.Trim(Environment.NewLine.ToCharArray()).Trim()); }
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

        public Point Location
        {
            get { throw new NotImplementedException(); }
        }

        public Size Size
        {
            get { throw new NotImplementedException(); }
        }

        public bool Displayed
        {
            get
            {
                return true;
            }
        }

        private void HandleClickingElements()
        {
            switch (TagName)
            {
                case "a":
                    HandleClickOnAnchorElement();
                    break;
                case "button":
                    HandleClickOnInputButtonElements();
                    break;
                case "input":
                    HandleClickOnInputButtonElements();
                    break;
            }
        }

        private void HandleClickOnAnchorElement()
        {
            webBrowser.Get(GetAttribute("href"));
        }

        private void HandleSelectingElements()
        {
            switch (TagName)
            {
                case "option":
                    HandleSelectingOptionElement();
                    break;
                case "input":
                    HandleSelecingInputElements();
                    break;
            }
        }

        private void HandleClickOnInputButtonElements()
        {
            var inputType = GetAttribute("type");
            switch (inputType)
            {
                case "submit":
                    HandleFormSubmit();
                    break;
                case "button":
                    HandleFormSubmit();
                    break;
            }
        }

        private void HandleSelecingInputElements()
        {
            var inputType = GetAttribute("type");
            switch (inputType)
            {
                case "checkbox":
                    HandleSelectingCheckBoxElement();
                    break;
                case "radio":
                    SelectRadioButton();
                    break;
            }
        }

        private void HandleFormSubmit()
        {
            webBrowser.Post(GetParentForm());
        }

        private void HandleSelectingOptionElement()
        {
            if (ElementIsNotSelected())
            {
                var selectElement = new HtmlElement(currentNode.ParentNode, webBrowser);
                var allOptionElements = selectElement.FindElements(By.TagName("option")).Cast<HtmlElement>();
                foreach (var element in allOptionElements)
                {
                    element.DeleteAttribute("selected");
                }
                SetAttribute("selected", "selected");
            }
        }

        private void SelectRadioButton()
        {
            var checkedState = currentNode.GetAttributeValue("checked", null);
            if (string.IsNullOrEmpty(checkedState))
            {
                var documentNode = new HtmlElement(currentNode.OwnerDocument.DocumentNode, webBrowser);
                var allRadioButtons = documentNode.FindElements(By.Name(GetAttribute("name")));
                foreach (HtmlElement element in allRadioButtons)
                {
                    element.DeleteAttribute("checked");
                }

                SetAttribute("checked", "checked");
                return;
            }
            DeleteAttribute("checked");
        }

        private void HandleSelectingCheckBoxElement()
        {
            if (currentNode.GetAttributeValue("checked", null) == null)
            {
                SetAttribute("checked", "checked");
            }
        }

        private AspNetForm GetParentForm()
        {
            var parentFormElement = webBrowser.GetParentFormElement(currentNode);
            return new AspNetForm(webBrowser.RequestVirtualPath, webBrowser.QueryString, parentFormElement, currentNode);
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
            currentNode.Attributes.Remove(attributeName);
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return RemoveXhtmlNamespaces(currentNode.ToString());
        }

        private void SetAttribute(string attributeName, string attributeValue)
        {
            currentNode.SetAttributeValue(attributeName, attributeValue);
        }

    }
}