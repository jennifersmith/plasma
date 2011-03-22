using System;
using System.Web;
using NUnit.Framework;
using Plasma.Core;

namespace Plasma.Test.Unit
{
    [TestFixture]
    public class CookieParserTest
    {
        [Test]
        public void ShouldParseCookieWithExpiryButNoValueCorrectly()
        {
            var testDate = new DateTime(2010, 8, 3, 10, 25, 3);
            const string rawCookie = "USER=; expires=Tue, 03-Aug-2010 09:25:03 GMT; path=/";

            var cookieParser = new CookieParser();
            HttpCookie cookie = cookieParser.ParseCookie(rawCookie);

            Assert.That(cookie.Name, Is.EqualTo("USER"));
            Assert.That(cookie.Value, Is.EqualTo(""));
            Assert.That(cookie.Expires, Is.EqualTo(testDate));
        }

        [Test]
        public void ShouldParseCookieWithExpiryAndAValueCorrectly()
        {
            var testDate = new DateTime(2010, 8, 3, 10, 25, 3);
            const string rawCookie = "USER=bob; expires=Tue, 03-Aug-2010 09:25:03 GMT; path=/";

            var cookieParser = new CookieParser();
            HttpCookie cookie = cookieParser.ParseCookie(rawCookie);

            Assert.That(cookie.Name, Is.EqualTo("USER"));
            Assert.That(cookie.Value, Is.EqualTo("bob"));
            Assert.That(cookie.Expires, Is.EqualTo(testDate));
        }

        [Test]
        public void ShouldParseCookieWithoutExpiryAndNoValueCorrectly()
        {
            const string rawCookie = "USER=; path=/";

            var cookieParser = new CookieParser();
            HttpCookie cookie = cookieParser.ParseCookie(rawCookie);

            Assert.That(cookie.Name, Is.EqualTo("USER"));
            Assert.That(cookie.Value, Is.EqualTo(""));
            Assert.That(cookie.Expires, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void ShouldParseCookieWithEqualsInTheValue()
        {
            const string rawCookie = "DATA=USER=bob&FullName=Bob The Builder; path=/";

            var cookieParser = new CookieParser();
            HttpCookie cookie = cookieParser.ParseCookie(rawCookie);

            Assert.That(cookie.Name, Is.EqualTo("DATA"));
            Assert.That(cookie.Value, Is.EqualTo("USER=bob&FullName=Bob The Builder"));
            Assert.That(cookie.Expires, Is.EqualTo(DateTime.MinValue));
        }
    }
}