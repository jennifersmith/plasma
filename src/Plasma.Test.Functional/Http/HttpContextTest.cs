using System.Net;
using NUnit.Framework;
using Plasma.Http;

namespace Plasma.Test.Functional.Http
{
    public class HttpContextTest
    {
        [Test]
        public void ShouldBeAbleToSendCookiesToHost()
        {
            var client = new HttpPlasmaClient(TestFixture.Application);

            var response = client.Get("/HttpContext/GetHttpContextCurrentUser");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.GetBody(), Is.StringContaining("HttpContext"));
        }
    }
}
