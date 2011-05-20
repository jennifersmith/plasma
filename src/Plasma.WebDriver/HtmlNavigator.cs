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
using System.Xml;
using System.Xml.Linq;
using OpenQA.Selenium;
using Plasma.Core;

namespace Plasma.WebDriver {
	public class HtmlNavigator : ISearchContext {
		private readonly AspNetResponse _response;
		private IWebElement _htmlElement;


		public HtmlNavigator(AspNetResponse response) {
			_response = response;
		}

		private IWebElement HtmlElement {
			get {
				return _htmlElement ??
					   (_htmlElement = new HtmlElement(Parse(_response.BodyAsString)));
			}
		}

        public AspNetResponse Response
        {
            get { return _response; }
        }

	    public String Title {
			get { return FindElement(By.TagName("title")).Text; }
		}

		public IWebElement FindElement(By mechanism) {
			return HtmlElement.FindElement(mechanism);
		}

		public ReadOnlyCollection<IWebElement> FindElements(By mechanism) {
			return HtmlElement.FindElements(mechanism);
		}

		public AspNetForm GetForm() {
			IWebElement formNode = HtmlElement.FindElement(By.TagName("form"));

			return new AspNetForm(_response.RequestVirtualPath, _response.QueryString, formNode);
		}

		public IEnumerable<AspNetForm> GetForms() {
			return
				HtmlElement.FindElements(By.TagName("form")).Select(
                    x => new AspNetForm(_response.RequestVirtualPath, _response.QueryString, x));
		}

        private static XElement Parse(string html)
        {
			var xmlReaderSettings = new XmlReaderSettings {
															  XmlResolver = new LocalEntityResolver(),
															  ProhibitDtd = false
														  };

            XDocument xmlDocument;
            try
			{
			    xmlDocument = XDocument.Load(XmlReader.Create(new StringReader(html), xmlReaderSettings));
			}
			catch (XmlException) {
				Console.Out.WriteLine("Failed to parse response as html:\n{0}", html);
				throw;
			}
			return RemoveNamespaces(xmlDocument).Root;
		}

	    private static XDocument RemoveNamespaces(XDocument xDocument)
	    {
	        IEnumerable<XElement> elementsWithNamespaces = xDocument.Descendants().Where(x => x.Name.Namespace != XNamespace.None);
	        foreach (var element in elementsWithNamespaces)
	        {
	            element.Name = RemoveNamespace(element.Name);
                element.ReplaceAttributes(element.Attributes().Select(RemoveNamespace));
	        }
	        xDocument.Descendants().Attributes().Where(x=>x.IsNamespaceDeclaration).Remove();
	        return xDocument;
	    }

	    private static XName RemoveNamespace(XName xName)
	    {
	        return XNamespace.None.GetName(xName.LocalName);
	    }

	    private static XAttribute RemoveNamespace(XAttribute xAttribute)
	    {
	        if(xAttribute.Name.Namespace!=XNamespace.None)
	        {
	            return new XAttribute(RemoveNamespace(xAttribute.Name), xAttribute.Value);
	        }
	        return xAttribute;
	    }
	}
}