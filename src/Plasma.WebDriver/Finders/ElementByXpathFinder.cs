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
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Plasma.WebDriver.Finders
{
    public class ElementByXpathFinder : IElementFinder
    {
        private readonly string _xpath;

        public ElementByXpathFinder(string xpath)
        {
            _xpath = xpath;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
            return xmlElement.XPathSelectElements(_xpath);
        }
    }
}