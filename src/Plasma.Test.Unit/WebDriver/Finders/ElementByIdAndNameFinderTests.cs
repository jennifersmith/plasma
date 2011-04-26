using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Plasma.WebDriver.Finders;

namespace Plasma.Test.Unit.WebDriver.Finders
{
    [TestFixture]
    public class ElementByIdAndNameFinderTests
    {
        [Test]
        public void ShouldMatchIfThereIsAnElementWithTheGivenId()
        {
            const string xmlSource = @"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                <div id='elementToFind'>Find this</div>
                                                            </div>
                                                         </div>
                                                    </body>N
                                                </html>";

            var document = new XmlDocument();
            document.LoadXml(xmlSource);

            IEnumerable<XmlElement> xmlElements = new ElementByIdFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1));
            Assert.That(xmlElements.Single().InnerText, Is.EqualTo("Find this"));
        }

        [Test]
        public void ShouldNotMatchIfThereIsAnElementWithTheGivenId()
        {

            const string xmlSource = @"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                <div id='dontFindThis'>Don't find this</div>
                                                            </div>
                                                         </div>
                                                    </body>N
                                                </html>";

            var document = new XmlDocument();
            document.LoadXml(xmlSource);

            IEnumerable<XmlElement> xmlElements = new ElementByIdFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0));
        }


        [Test]
        public void ShouldMatchIfThereIsAnElementWithTheGivenName()
        {
            const string xmlSource = @"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                <div name='elementToFind'>Find this</div>
                                                            </div>
                                                         </div>
                                                    </body>N
                                                </html>";

            var document = new XmlDocument();
            document.LoadXml(xmlSource);

            IEnumerable<XmlElement> xmlElements = new ElementByNameFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(1));
            Assert.That(xmlElements.Single().InnerText, Is.EqualTo("Find this"));
        }

        [Test]
        public void ShouldNotMatchIfThereIsAnElementWithTheGivenName()
        {

            const string xmlSource = @"<html>
                                                    <body>
                                                        <div>
                                                            <div>
                                                                <div name='dontFindThis'>Don't find this</div>
                                                            </div>
                                                         </div>
                                                    </body>N
                                                </html>";

            var document = new XmlDocument();
            document.LoadXml(xmlSource);

            IEnumerable<XmlElement> xmlElements = new ElementByNameFinder("elementToFind").FindWithin(document.DocumentElement);

            Assert.That(xmlElements.Count(), Is.EqualTo(0));
        }
    }
}