﻿using System;
using System.Linq;
using System.Net;
using NUnit.Framework;
using Plasma.Http;

namespace Plasma.Test.Functional.Http
{
    public class CookieTest
    {
        readonly Uri uri = new Uri("http://localhost");

        [Test]
        public void ShouldBeAbleToSendCookiesToHost()
        {
            var value = Guid.NewGuid().ToString();
            var client = new HttpPlasmaClient(TestFixture.Application);
            client.AddCookie(new Cookie("test", value));

            var response = client.Get("/Cookies/Show");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.GetBody(), Is.StringContaining(value));
        }

        [Test]
        public void ShouldAcceptCookiesSetByTheHost()
        {
            var client = new HttpPlasmaClient(TestFixture.Application);

            var response = client.Get("/Cookies/Set");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(client.GetAllCookies().Any(x=> x.Name == "Test"), Is.True);
            Assert.That(client.GetAllCookies().First(x => x.Name == "Test").Value, Is.StringContaining("Cookie Set By Host"));
        }

        [Test]
        public void ShouldAcceptManyCookiesSetByTheHost()
        {
            var client = new HttpPlasmaClient(TestFixture.Application);

            var response = client.Get("/Cookies/SetMany");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(client.GetAllCookies().Any(x => x.Name == "Test1"), Is.True);
            Assert.That(client.GetAllCookies().First(x => x.Name == "Test1").Value, Is.StringContaining("Cookie Set By Host"));
            Assert.That(client.GetAllCookies().Any(x => x.Name == "Test2"), Is.True);
            Assert.That(client.GetAllCookies().First(x => x.Name == "Test2").Value, Is.StringContaining("Cookie Set By Host"));
        }

        [Test]
        public void ShouldExpireCookiesThatAreSetByTheHostIntoThePast()
        {
            var value = Guid.NewGuid().ToString();
            var client = new HttpPlasmaClient(TestFixture.Application);
            client.AddCookie(new Cookie("test", value));

            var response = client.Get("/Cookies/Expire");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(client.GetAllCookies().Any(), Is.False);
        }
    }
}
