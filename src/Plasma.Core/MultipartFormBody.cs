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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace Plasma.Core {
    public class MultipartFormBody {

        private readonly string _boundary;
        private readonly byte[] _formBodyData;

        public MultipartFormBody(NameValueCollection fields, HashSet<string> fileControls) {
            _boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            _formBodyData = GenerateMultipartFormData(fields, fileControls);
        }

        public byte[] FormBodyData() {
            return _formBodyData;
        }

        private  byte[] GenerateMultipartFormData(NameValueCollection fields, HashSet<string> fileControls) {
            
            var rs = new MemoryStream();
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + _boundary + "\r\n");
            const string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in fields.Keys)
            {
                if (fileControls.Contains(key)) continue;

                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = String.Format(formdataTemplate, key, fields[key]);
                byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            const string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            foreach (var fileControl in fileControls) {
                var filePath = fields[fileControl];
                string header = String.Format(headerTemplate, fileControl, Path.GetFileName(filePath), ContentTypeMapper.MimeType(filePath));
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                if (string.IsNullOrEmpty(filePath)) continue;
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    var buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        rs.Write(buffer, 0, bytesRead);
                    }    
                }
            }
            
            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + _boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();
            return rs.ToArray();
        }

        public string ContentType {
            get { return "multipart/form-data; boundary=" + _boundary; }
        }

        public string ContentLength
        {
            get { return _formBodyData.ToString(); }
        }
    }
}