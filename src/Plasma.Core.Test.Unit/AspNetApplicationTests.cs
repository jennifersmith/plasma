using NUnit.Framework;
using Plasma.Sample.Web.Mvc;

namespace Plasma.Core.Test.Unit
{
    [TestFixture]
    public class AspNetApplicationTests
    {
        [Test]
        public void CanBeConfigured()
        {
            var app = new AspNetApplication("/", "c:\\temp");

            Assert.That(app, Is.Not.Null);
        }

        [Test]
        public void DefaultsVirtualPathToRootWhenOnlyProvidedPhysicalPath()
        {
            var app = new AspNetApplication("c:\\temp");

            Assert.That(app, Is.Not.Null);
        }

        [Test]
        public void CanWireUpPathsFromProvidedType()
        {
            var appInstance = new AspNetApplication(typeof(MvcApplication));

            Assert.That(appInstance, Is.Not.Null);
        }
    }
}
