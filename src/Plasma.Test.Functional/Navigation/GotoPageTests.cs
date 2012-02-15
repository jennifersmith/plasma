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
using NUnit.Framework;
using OpenQA.Selenium;

namespace Plasma.Test.Functional.Navigation
{
    [TestFixture]
    public class GotoPageTests
    {
        [Test]
        public void ShouldNavigateToTheGivenUrl()
        {
            TestFixture.Driver.Navigate().GoToUrl("~/GotoPage");
            var titleElement = TestFixture.Driver.FindElement(By.TagName("title"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }

        [Test]
        public void ShouldFollow302RedirectsWhenNavigatingToAGivenUrl()
        {
            TestFixture.Driver.Navigate().GoToUrl("~/GotoPage/ThreeOhTwo");
            var titleElement = TestFixture.Driver.FindElement(By.TagName("title"));
            Assert.That(TestFixture.Driver.Url, Is.EqualTo("/GotoPage"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }

        [Test]
        public void ShouldFollow301RedirectsWhenNavigatingToAGivenUrl()
        {
            TestFixture.Driver.Navigate().GoToUrl("~/GotoPage/ThreeOhOne");
            var titleElement = TestFixture.Driver.FindElement(By.TagName("title"));
            Assert.That(TestFixture.Driver.Url, Is.EqualTo("/GotoPage"));
            Assert.That(titleElement.Text, Is.StringContaining("GotoPage"));
        }
    }
}
