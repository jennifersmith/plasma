using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using NUnit.Framework;
using Plasma.Core;
using Plasma.Sample.Web.Mvc;

namespace Plasma.HttpClient.Test.Unit
{
    [TestFixture]
    public class PlasmaClientTests
    {
        private string _randomData;
        private FormUrlEncodedContent _formContent;
        private System.Net.Http.HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _randomData = Guid.NewGuid().ToString();
            _formContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("value", _randomData)
            });
            _client = PlasmaClient.For<MvcApplication>();
        }

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
        public async void GetCallGet()
        {
            var client = PlasmaClient.For<MvcApplication>();

            var response = await client.GetAsync("/AllTheVerbs/Get");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void CanPostData()
        {
            var response =  _client.PostAsync("/AllTheVerbs/Post", _formContent).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining(_randomData));
        }

        [Test]
        public void CanPutData()
        {
            var response = _client.PutAsync("/AllTheVerbs/Put", _formContent).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining(_randomData));
        }

        [Test]
        public void CanPatchData()
        {
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), "/AllTheVerbs/Patch") {Content = _formContent};

            var response = _client.SendAsync(req).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining(_randomData));
        }

        [Test]
        public void CanCallOptions()
        {
            var req = new HttpRequestMessage(HttpMethod.Options, "/AllTheVerbs/Options");

            var response = _client.SendAsync(req).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining("Options page"));
        }

        [Test]
        public void CanCallTrace()
        {
            var req = new HttpRequestMessage(HttpMethod.Trace, "/AllTheVerbs/Trace");

            var response = _client.SendAsync(req).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining("Trace page"));
        }

        [Test]
        public void CanCallHead()
        {
            var req = new HttpRequestMessage(HttpMethod.Head, "/AllTheVerbs/Head");

            var response = _client.SendAsync(req).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.Not.StringContaining("Head page")); // Because HEAD doesn't return a body.
        }

        [Test]
        public void CanDeleteData()
        {
            var response = _client.DeleteAsync("/AllTheVerbs/Delete").Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining("Delete page"));
        }

        [Test]
        public void ShouldBeAbleToSendCookiesToHost()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/Cookies/Show");
            req.Headers.Add("Cookie", "Test=" + _randomData + ";");

            var response = _client.SendAsync(req).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.ReadAsStringAsync().Result, Is.StringContaining(_randomData));
        }

        [Test]
        public void ShouldAcceptCookiesSetByTheHost()
        {
            var response = _client.GetAsync("/Cookies/Set").Result;
            var cookies = response.Headers.SingleOrDefault(x => x.Key == "Set-Cookie");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(cookies.Value.First().Contains("Test=Cookie Set By Host"));
        }

        [Test]
        public void ShouldAcceptManyCookiesSetByTheHost()
        {
            var response = _client.GetAsync("/Cookies/SetMany").Result;
            var cookies = response.Headers.SingleOrDefault(x => x.Key == "Set-Cookie");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(cookies.Value.First().Contains("Test1=Cookie Set By Host"));
            Assert.That(cookies.Value.Skip(1).First().Contains("Test2=Cookie Set By Host"));
        }

        [Test]
        public void ShouldExpireCookiesThatAreSetByTheHostIntoThePast()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "/Cookies/Expire");
            req.Headers.Add("Cookie", "test=" + _randomData + ";");

            var response = _client.SendAsync(req).Result;
            var cookies = response.Headers.SingleOrDefault(x => x.Key == "Set-Cookie");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(cookies.Value.First().Contains("Test=;"));
        }
    }
}