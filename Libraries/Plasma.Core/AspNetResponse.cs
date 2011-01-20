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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace Plasma.Core
{

    public class AspNetResponse {
        private string _requestVirtualPath;
        private int _status;
        private List<KeyValuePair<string, string>> _headers;
        private byte[] _body;
        private string _bodyAsString;
        private HtmlDocument _bodyAsHtmlDoc;

        internal AspNetResponse(string requestVirtualPath,
            int status, List<KeyValuePair<string, string>> headers, byte[] body) {

            _requestVirtualPath = requestVirtualPath;

            _status = status;
            _headers = headers;
            _body = body;
        }

        public int Status {
            get { return _status; }
        }

        public List<KeyValuePair<string, string>> Headers {
            get {
                if (_headers == null) {
                    _headers = new List<KeyValuePair<string, string>>();
                }

                return _headers; 
            }
        }

        public byte[] Body {
            get { return _body; }
        }

        public string BodyAsString {
            get {
                if (_bodyAsString == null) {
                    if (_body != null && _body.Length > 0) {
                        _bodyAsString = Encoding.UTF8.GetString(_body);
                    }
                    else {
                        _bodyAsString = string.Empty;
                    }
                }

                return _bodyAsString;
            }
        }

        public HtmlNode FindHtmlElementById(string id) {
            HtmlDocument doc = BodyAsHtmlDocument;
            HtmlNode node = FindChildNode(doc.DocumentNode, null, id);

            return node;
        }

        public string FindInnerHtmlById(string id)
        {
            HtmlDocument doc = BodyAsHtmlDocument;
            HtmlNode node = FindChildNode(doc.DocumentNode, null, id);

            if (node != null)
            {
                return node.InnerHtml;
            }
            else
            {
                return null;
            }
        }

        public AspNetForm GetForm() {
            HtmlDocument doc = BodyAsHtmlDocument;
            HtmlNode formNode = FindChildNode(doc.DocumentNode, "form", null);

            if (formNode == null) {
                return null;
            }

            return new AspNetForm(_requestVirtualPath, formNode);
        }

        public string ToEntireResponseString() {
            TextWriter output = new StringWriter();

            output.WriteLine("{0} {1}", Status, HttpWorkerRequest.GetStatusDescription(200));

            foreach (KeyValuePair<string, string> header in Headers) {
                output.WriteLine("{0}: {1}", header.Key, header.Value);
            }

            output.WriteLine();

            output.Write(BodyAsString);

            return output.ToString();
        }

        private HtmlDocument BodyAsHtmlDocument {
            get {
                if (_bodyAsHtmlDoc == null) {
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(BodyAsString);
                    _bodyAsHtmlDoc = doc;
                }

                return _bodyAsHtmlDoc;
            }
        }

        private HtmlNode FindChildNode(HtmlNode node, string name, string id) {
            foreach (HtmlNode childNode in node.ChildNodes) {
                if (childNode.NodeType == HtmlNodeType.Element) {
                    if (!string.IsNullOrEmpty(name)) {
                        if (string.Compare(childNode.Name, name, StringComparison.OrdinalIgnoreCase) == 0) {
                            return childNode;
                        }
                    }

                    if (!string.IsNullOrEmpty(id)) {
                        HtmlAttribute idAttribute = childNode.Attributes["id"];

                        if (idAttribute != null &&
                            string.Compare(idAttribute.Value, id, StringComparison.OrdinalIgnoreCase) == 0) {
                            return childNode;
                        }
                    }

                    HtmlNode childNodeWithId = FindChildNode(childNode, name, id);

                    if (childNodeWithId != null) {
                        return childNodeWithId;
                    }
                }
            }

            return null;
        }

    }
}
