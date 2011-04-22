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

namespace Plasma.Test.Functional.Controls {
    [TestFixture]
    public class FileUpload {

        [Test]
        public void File_Upload() {
            
            const int contentLength = 8192;
            string srcFile = Path.GetTempFileName();
            byte[] data = GenerateRandomData(contentLength);
            WriteDataToFile(srcFile, data);

            HtmlNavigator firstHtml = WebApplicationFixture.ProcessRequest("~/Controls/FileUpload.aspx").Html();

            AspNetForm form = firstHtml.GetForm();
            form["FileUpload1"] = srcFile;

            HtmlNavigator secondHtml = WebApplicationFixture.ProcessRequest(Button.Click(form, "Button1")).Html();

            Assert.AreEqual(Path.GetFileName(srcFile), secondHtml.FindElement(By.Id("FileName")).Text);
            Assert.AreEqual(contentLength.ToString(), secondHtml.FindElement(By.Id("ContentLength")).Text);
            Assert.AreEqual("application/octet-stream", secondHtml.FindElement(By.Id("ContentType")).Text);
            string destinationFile = secondHtml.FindElement(By.Id("SavedTo")).Text;

            byte[] copiedData = File.ReadAllBytes(destinationFile);
            
            Assert.That(copiedData, Is.EqualTo(data));

            File.Delete(srcFile);
            File.Delete(destinationFile);
        }

        
        private static byte[] GenerateRandomData(int contentLength) {
            var data = new byte[contentLength];
            var random = new Random();
            random.NextBytes(data);
            return data;
        } 

        private static void WriteDataToFile(string filename, byte[] data) {
            using (FileStream stream = File.OpenWrite(filename))
            {
                stream.Write(data, 0, data.Length);
            }
        }
    }
}