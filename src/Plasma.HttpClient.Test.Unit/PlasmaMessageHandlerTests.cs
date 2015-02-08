using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using Plasma.Http;
using Plasma.HttpClient.Test.Unit.TestDoubles;

namespace Plasma.HttpClient.Test.Unit
{
    [TestFixture]
    public class PlasmaMessageHandlerTests
    {
        private TestPlasmaHandler _handler;
        private HttpRequestMessage _msg;
        private FakeAspNetApp _fakeAspNetApp;

        [SetUp]
        public void SetUp()
        {
            _fakeAspNetApp = new FakeAspNetApp();
            _handler = new TestPlasmaHandler(_fakeAspNetApp);
            _msg = new HttpRequestMessage(HttpMethod.Get, "http://www.tempuri.org/123");
        }

        [Test]
        public void SendAsync_WitHeaders_CallsAppHostWithHeaders()
        {
            _msg.Headers.Add("Test", "one");

            _handler.InvokeInternalSendAsync(_msg);

            Assert.That(_fakeAspNetApp.LastRequest.Headers[0].Key, Is.EqualTo("Test"));
            Assert.That(_fakeAspNetApp.LastRequest.Headers[0].Value, Is.EqualTo("one"));
        }

        [Test]
        public void SendAsync_WitContentHeaders_ContentHeadersInGeneratedRequest()
        {
            _msg.Content = new StringContent("Abc");
            _msg.Content.Headers.Add("ContentHeader1", "one");

            _handler.InvokeInternalSendAsync(_msg);

            Assert.That(_fakeAspNetApp.LastRequest.Headers.Any(x => x.Key == "ContentHeader1"));
            Assert.That(_fakeAspNetApp.LastRequest.Headers.Single(x => x.Key == "ContentHeader1").Value, Is.EqualTo("one"));
        }

        [Test]
        public void SendAsync_WithBody_CreatesRequestWithIdenticalByteArrayBody()
        {
            var msgBody = new byte[] {1, 2, 3, 4};
            _msg.Content = new ByteArrayContent(msgBody);

            _handler.InvokeInternalSendAsync(_msg);

            Assert.That(_fakeAspNetApp.LastRequest.Body, Is.EqualTo(msgBody));
        }

        [Test]
        public void SendAsync_ValidRequest_RequestIsMappedIntoReturnedResponse()
        {
            var response = _handler.InvokeInternalSendAsync(_msg);

            Assert.That(response.Result.RequestMessage, Is.EqualTo(_msg));
        }

        [Test]
        public void SendAsync_ValidRequest_VersionIsMappedFromRequest()
        {
            var response = _handler.InvokeInternalSendAsync(_msg);

            Assert.That(response.Result.Version, Is.EqualTo(_msg.Version));
        }

        [Test]
        public void SendAsync_ValidRequest_CookiesAreMappedToRequest()
        {
            _msg.Headers.Add("Cookie", "cookie1=value1; cookie2=value2");

            _handler.InvokeInternalSendAsync(_msg);

            Assert.That(_fakeAspNetApp.LastRequest.Headers.Any(x=>x.Key == "Cookie"));
        }
    }
}
