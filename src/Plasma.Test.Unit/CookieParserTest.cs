/* **********************************************************************************
 *
 * Copyright 2010 ThoughtWorks, Inc.  
 * ThoughtWorks provides the software "as is" without warranty of any kind, either express or implied, including but not limited to, 
 * the implied warranties of merchantability, satisfactory quality, non-infringement and fitness for a particular purpose.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).  
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/
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