using System.IO;
using System.Net;
using NUnit.Framework;
using Plasma.Core;
using Plasma.Sample.Web.Mvc;

namespace Plasma.HttpClient.Test.Unit
{
    [TestFixture]
    public class PlasmaClientTests
    {
        [Test]
        public void CanCreateHttpClientForType()
        {
            var client = PlasmaClient.For(new AspNetApplication(typeof(MvcApplication)));

            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void CanCreateHttpClientForPath()
        {
            var client = PlasmaClient.For(Path.GetFullPath(@".\..\..\..\web\Plasma.Sample.Web.Mvc"));

            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void CanCreateHttpClientForGenericType()
        {
            var client = PlasmaClient.For<MvcApplication>();

            Assert.That(client, Is.Not.Null);
        }

        [Test]
        public void GetCallGet()
        {
            var client = PlasmaClient.For<MvcApplication>();

            var response = client.GetAsync("/").Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}