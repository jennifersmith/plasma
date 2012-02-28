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
using Plasma.WebDriver;

namespace Plasma.Test.Functional.WebDriver.Forms
{
    [TestFixture]
    public class RadioButtonTests
    {
        private PlasmaDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = TestFixture.Driver;
            driver.Navigate().GoToUrl("/FormWithRadioButton/");
        }

        [Test]
        public void ShouldBeAbleToCheckARadioButton()
        {
            driver.FindElement(By.Id("radioButton1")).Click();
            driver.FindElement(By.TagName("form")).Submit();
            Assert.That(driver.FindElement(By.Id("radioButtonValue")).Text, Is.StringContaining("One"), driver.PageSource);
        }

        [Test]
        public void ShouldBeAbletoCheckASecondRadioButtonWhenOneIsAlreadyChecked()
        {
            driver.FindElement(By.Id("radioButton1")).Click();
            driver.FindElement(By.Id("radioButton2")).Click();
            driver.FindElement(By.TagName("form")).Submit();

            Assert.That(driver.FindElement(By.Id("radioButton1")).Selected, Is.False);
            Assert.That(driver.FindElement(By.Id("radioButtonValue")).Text, Is.StringContaining("Two"), driver.PageSource);
        }
    }
}
