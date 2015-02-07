using System;
using System.Collections.Generic;
using System.IO;
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

            var response = await client.GetAsync("/");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        //[Test]
        //public void CanPostData()
        //{
        //    var client = PlasmaClient.For<MvcApplication>();
        //    var randomData = Guid.NewGuid().ToString();

        //    var response =
        //        client.PostAsync("/Post/Data", new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        //        {
        //            new KeyValuePair<string, string>("value", randomData)
        //        })).Result;

        //    var responseBody = response.Content.ReadAsStringAsync().Result;

        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //    Assert.That(responseBody, Is.StringContaining(randomData));
        //}
    }
}