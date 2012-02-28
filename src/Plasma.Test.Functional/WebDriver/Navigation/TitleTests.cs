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

namespace Plasma.Test.Functional.WebDriver.Navigation
{
    [TestFixture]
    public class TitleTests
    {
        [Test]
        public void ShouldBeAbleToGetTheTitleOfThePage()
        {
            TestFixture.Driver.Navigate().GoToUrl("/GotoPage");
            Assert.That(TestFixture.Driver.Title, Is.StringContaining("GotoPage"));
        }
    }
}
