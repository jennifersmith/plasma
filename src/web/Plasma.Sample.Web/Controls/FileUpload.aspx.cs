/* **********************************************************************************
 *
 * Copyright (c) Microsoft Corporation. All rights reserved.
 *
 * Portions Copyright 2010 ThoughtWorks, Inc.
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
using System.IO;
using System.Web.UI;

public partial class Controls_FileUpload : Page {
    protected void Button1_Click(object sender, EventArgs e) {
        if (FileUpload1.HasFile) {
            string destinationFilename = Path.GetTempFileName();
            FileUpload1.SaveAs(destinationFilename);
            FileName.Text = FileUpload1.PostedFile.FileName;
            ContentLength.Text = FileUpload1.PostedFile.ContentLength.ToString();
            ContentType.Text = FileUpload1.PostedFile.ContentType;
            SavedTo.Text = destinationFilename;
        } 
    }
}