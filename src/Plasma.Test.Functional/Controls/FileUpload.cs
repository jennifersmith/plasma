/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Permissive
 * License (MS-PL).
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using Plasma.WebDriver;

namespace Plasma.Test.Functional.Controls
{
    [TestFixture]
    public class FileUpload
    {
        [Test, Ignore]
        public void File_Upload()
        {
            const int contentLength = 8192;
            var srcFile = Path.GetTempFileName();
            var data = GenerateRandomData(contentLength);
            WriteDataToFile(srcFile, data);

            var plasmaDriver = new PlasmaDriver(WebApplicationFixture.AppInstance);
            plasmaDriver.Navigate().GoToUrl("~/Controls/FileUpload.aspx");
            
            plasmaDriver.FindElement(By.Id("FileUpload1")).SendKeys(srcFile);
            plasmaDriver.FindElement(By.Id("Button1")).Click();

            Assert.AreEqual(Path.GetFileName(srcFile), plasmaDriver.FindElement(By.Id("FileName")).Text);
            Assert.AreEqual(contentLength.ToString(), plasmaDriver.FindElement(By.Id("ContentLength")).Text);
            Assert.AreEqual("application/octet-stream", plasmaDriver.FindElement(By.Id("ContentType")).Text);
            string destinationFile = plasmaDriver.FindElement(By.Id("SavedTo")).Text;

            byte[] copiedData = File.ReadAllBytes(destinationFile);

            Assert.That(copiedData, Is.EqualTo(data));

            File.Delete(srcFile);
            File.Delete(destinationFile);
        }

        private static byte[] GenerateRandomData(int contentLength)
        {
            var data = new byte[contentLength];
            var random = new Random();
            random.NextBytes(data);
            return data;
        }

        private static void WriteDataToFile(string filename, byte[] data)
        {
            using (FileStream stream = File.OpenWrite(filename))
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}