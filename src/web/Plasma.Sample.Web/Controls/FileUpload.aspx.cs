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