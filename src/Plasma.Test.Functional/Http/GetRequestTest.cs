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
