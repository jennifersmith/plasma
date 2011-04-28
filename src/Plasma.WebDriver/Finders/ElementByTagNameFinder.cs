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
using System.Linq;
using System.Xml.Linq;

namespace Plasma.WebDriver.Finders
{
    public class ElementByTagNameFinder : IElementFinder
    {
        private readonly string _name;

        public ElementByTagNameFinder(string name)
        {
            _name = name;
        }

        public IEnumerable<XElement> FindWithin(XElement xmlElement)
        {
            return xmlElement.Descendants().Where(x => x.Name == _name);
        }
    }
}