using NUnit.Framework;

namespace Plasma.Core.Test.Unit
{
    [TestFixture]
    public class AspNetApplicationTests
    {
        [Test]
        public void Ctor_InvokedWithVirtualPathAndPhysicalPath_NotNull()
        {
            var app = new AspNetApplication("/", "c:\\temp");

            Assert.That(app, Is.Not.Null);
        }

        [Test]
        public void Ctor_InvokedWithPhysicalPathOnly_NotNull()
        {
            var app = new AspNetApplication("c:\\temp");

            Assert.That(app, Is.Not.Null);
        }
    }
}
