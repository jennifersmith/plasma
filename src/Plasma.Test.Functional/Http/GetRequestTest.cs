using System.Net;
using NUnit.Framework;
using Plasma.Http;

namespace Plasma.Test.Functional.Http
{
    [TestFixture]
    public class GetRequestTest
    {
        [Test]
        public void ShouldBeAbleToGetTheContentOfAResource()
        {
            var client = new HttpPlasmaClient(TestFixture.Application);

            var httpPlasmaResponse = client.Get("/GotoPage");

            Assert.That(httpPlasmaResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void ShouldFollow302RedirectsWhenNavigatingToAGivenUrl()
        {
            var request = new HttpPlasmaClient(TestFixture.Application);
            
            var httpPlasmaResponse = request.Get("/GotoPage/ThreeOhTwo");

            Assert.That(httpPlasmaResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(httpPlasmaResponse.VirtualPath, Is.EqualTo("/GotoPage"));
        }

        [Test]
        public void ShouldFollow301RedirectsWhenNavigatingToAGivenUrl()
        {
            var request = new HttpPlasmaClient(TestFixture.Application);

            var httpPlasmaResponse = request.Get("/GotoPage/ThreeOhOne");

            Assert.That(httpPlasmaResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(httpPlasmaResponse.VirtualPath, Is.EqualTo("/GotoPage"));
        }
    }
}
