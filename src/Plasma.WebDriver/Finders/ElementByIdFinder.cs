using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Plasma.WebDriver.Finders
{
    public class ElementByIdFinder : IElementFinder
    {
        private readonly string id;

        public ElementByIdFinder(string id)
        {
            this.id = id;
        }

        public IEnumerable<HtmlNode> FindWithin(HtmlNode htmlNodes)
        {
            return htmlNodes.Descendants().Where(x => x.GetAttributeValue("id", string.Empty).ToLower() == id.ToLower());
        }
    }
}