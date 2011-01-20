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
using System.Text;
using System.Diagnostics;
using System.Collections;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

// Derived from HtmlAgilityPack V1.0 - Simon Mourier <simon_mourier@hotmail.com>

namespace Plasma.Core
{

    public class HtmlAttribute : IComparable {
        internal int _line = 0;
        internal int _lineposition = 0;
        internal int _streamposition = 0;
        internal int _namestartindex = 0;
        internal int _namelength = 0;
        internal int _valuestartindex = 0;
        internal int _valuelength = 0;
        internal HtmlDocument _ownerdocument; // attribute can exists without a node
        internal HtmlNode _ownernode;
        internal string _name;
        internal string _value;

        internal HtmlAttribute(HtmlDocument ownerdocument) {
            _ownerdocument = ownerdocument;
        }

        public HtmlAttribute Clone() {
            HtmlAttribute att = new HtmlAttribute(_ownerdocument);
            att.Name = Name;
            att.Value = Value;
            return att;
        }

        public int CompareTo(object obj) {
            HtmlAttribute att = obj as HtmlAttribute;
            if (att == null) {
                throw new ArgumentException("obj");
            }
            return Name.CompareTo(att.Name);
        }

        internal string XmlName {
            get {
                return HtmlDocument.GetXmlName(Name);
            }
        }

        internal string XmlValue {
            get {
                return Value;
            }
        }

        public string Name {
            get {
                if (_name == null) {
                    _name = _ownerdocument._text.Substring(_namestartindex, _namelength).ToLower();
                }
                return _name;
            }
            set {
                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                _name = value.ToLower();
                if (_ownernode != null) {
                    _ownernode._innerchanged = true;
                    _ownernode._outerchanged = true;
                }
            }
        }

        public string Value {
            get {
                if (_value == null) {
                    _value = _ownerdocument._text.Substring(_valuestartindex, _valuelength);
                }
                return _value;
            }
            set {
                _value = value;
                if (_ownernode != null) {
                    _ownernode._innerchanged = true;
                    _ownernode._outerchanged = true;
                }
            }
        }

        public int Line {
            get {
                return _line;
            }
        }

        public int LinePosition {
            get {
                return _lineposition;
            }
        }

        public int StreamPosition {
            get {
                return _streamposition;
            }
        }

        public HtmlNode OwnerNode {
            get {
                return _ownernode;
            }
        }

        public HtmlDocument OwnerDocument {
            get {
                return _ownerdocument;
            }
        }
    }

    public class HtmlAttributeCollection : IEnumerable {
        internal Hashtable _hashitems = new Hashtable();
        private ArrayList _items = new ArrayList();
        private HtmlNode _ownernode;

        internal HtmlAttributeCollection(HtmlNode ownernode) {
            _ownernode = ownernode;
        }

        public HtmlAttribute Append(HtmlAttribute newAttribute) {
            if (newAttribute == null) {
                throw new ArgumentNullException("newAttribute");
            }

            _hashitems[newAttribute.Name] = newAttribute;
            newAttribute._ownernode = _ownernode;
            _items.Add(newAttribute);

            _ownernode._innerchanged = true;
            _ownernode._outerchanged = true;
            return newAttribute;
        }

        public HtmlAttribute Append(string name) {
            HtmlAttribute att = _ownernode._ownerdocument.CreateAttribute(name);
            return Append(att);
        }

        public HtmlAttribute Append(string name, string value) {
            HtmlAttribute att = _ownernode._ownerdocument.CreateAttribute(name, value);
            return Append(att);
        }

        public HtmlAttribute Prepend(HtmlAttribute newAttribute) {
            if (newAttribute == null) {
                throw new ArgumentNullException("newAttribute");
            }

            _hashitems[newAttribute.Name] = newAttribute;
            newAttribute._ownernode = _ownernode;
            _items.Insert(0, newAttribute);

            _ownernode._innerchanged = true;
            _ownernode._outerchanged = true;
            return newAttribute;
        }

        public void RemoveAt(int index) {
            HtmlAttribute att = (HtmlAttribute)_items[index];
            _hashitems.Remove(att.Name);
            _items.RemoveAt(index);

            _ownernode._innerchanged = true;
            _ownernode._outerchanged = true;
        }

        public void Remove(HtmlAttribute attribute) {
            if (attribute == null) {
                throw new ArgumentNullException("attribute");
            }
            int index = GetAttributeIndex(attribute);
            if (index == -1) {
                throw new IndexOutOfRangeException();
            }
            RemoveAt(index);
        }

        public void Remove(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            string lname = name.ToLower();
            for (int i = 0; i < _items.Count; i++) {
                HtmlAttribute att = (HtmlAttribute)_items[i];
                if (att.Name == lname) {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveAll() {
            _hashitems.Clear();
            _items.Clear();

            _ownernode._innerchanged = true;
            _ownernode._outerchanged = true;
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        internal int GetAttributeIndex(HtmlAttribute attribute) {
            if (attribute == null) {
                throw new ArgumentNullException("attribute");
            }
            for (int i = 0; i < _items.Count; i++) {
                if (((HtmlAttribute)_items[i]) == attribute)
                    return i;
            }
            return -1;
        }

        internal int GetAttributeIndex(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            string lname = name.ToLower();
            for (int i = 0; i < _items.Count; i++) {
                if (((HtmlAttribute)_items[i]).Name == lname)
                    return i;
            }
            return -1;
        }

        public HtmlAttribute this[string name] {
            get {
                if (name == null) {
                    throw new ArgumentNullException("name");
                }
                return _hashitems[name.ToLower()] as HtmlAttribute;
            }
        }

        public HtmlAttribute this[int index] {
            get {
                return _items[index] as HtmlAttribute;
            }
        }

        internal void Clear() {
            _hashitems.Clear();
            _items.Clear();
        }

        public HtmlAttributeEnumerator GetEnumerator() {
            return new HtmlAttributeEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public class HtmlAttributeEnumerator : IEnumerator {
            int _index;
            ArrayList _items;

            internal HtmlAttributeEnumerator(ArrayList items) {
                _items = items;
                _index = -1;
            }

            public void Reset() {
                _index = -1;
            }

            public bool MoveNext() {
                _index++;
                return (_index < _items.Count);
            }

            public HtmlAttribute Current {
                get {
                    return (HtmlAttribute)(_items[_index]);
                }
            }

            object IEnumerator.Current {
                get {
                    return (Current);
                }
            }
        }
    }

    public class HtmlCommentNode : HtmlNode {
        private string _comment;

        internal HtmlCommentNode(HtmlDocument ownerdocument, int index)
            :
            base(HtmlNodeType.Comment, ownerdocument, index) {
        }

        public override string InnerHtml {
            get {
                if (_comment == null) {
                    return base.InnerHtml;
                }
                return _comment;
            }
            set {
                _comment = value;
            }
        }

        public override string OuterHtml {
            get {
                if (_comment == null) {
                    return base.OuterHtml;
                }
                return "<!--" + _comment + "-->";
            }
        }

        public string Comment {
            get {
                if (_comment == null) {
                    return base.InnerHtml;
                }
                return _comment;
            }
            set {
                _comment = value;
            }
        }
    }

    public class HtmlDocument {
        internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";
        internal static readonly string HtmlExceptionUseIdAttributeFalse = "You need to set UseIdAttribute property to true to enable this feature";

        internal Hashtable _openednodes;
        internal Hashtable _lastnodes = new Hashtable();
        internal Hashtable _nodesid;
        private HtmlNode _documentnode;
        internal string _text;
        private string _remainder;
        private int _remainderOffset;
        private HtmlNode _currentnode;
        private HtmlNode _lastparentnode;
        private HtmlAttribute _currentattribute;
        private int _index;
        private int _line;
        private int _lineposition, _maxlineposition;
        private int _c;
        private bool _fullcomment;
        private System.Text.Encoding _streamencoding;
        private System.Text.Encoding _declaredencoding;
        private ArrayList _parseerrors = new ArrayList();
        private ParseState _state, _oldstate;

        // public props

        public bool OptionCheckSyntax = false;

        public bool OptionUseIdAttribute = false;

        public bool OptionWriteEmptyNodes = false;

        public bool OptionOutputAsXml = false;

        public bool OptionOutputUpperCase = false;

        public bool OptionOutputOptimizeAttributeValues = false;

        public bool OptionAddDebuggingAttributes = false;

        public bool OptionExtractErrorSourceText = false; // turning this on can dramatically slow performance if a lot of errors are detected

        public bool OptionAutoCloseOnEnd = false; // close errors at the end

        public bool OptionFixNestedTags = false; // fix li, tr, th, td tags

        public int OptionExtractErrorSourceTextMaxLength = 100;

        public System.Text.Encoding OptionDefaultStreamEncoding = System.Text.Encoding.Default;

        public string OptionStopperNodeName = null;

        public string Remainder {
            get {
                return _remainder;
            }
        }

        public int RemainderOffset {
            get {
                return _remainderOffset;
            }
        }

        public ArrayList ParseErrors {
            get {
                return _parseerrors;
            }
        }

        public System.Text.Encoding StreamEncoding {
            get {
                return _streamencoding;
            }
        }

        public System.Text.Encoding DeclaredEncoding {
            get {
                return _declaredencoding;
            }
        }

        public HtmlDocument() {
            _documentnode = CreateNode(HtmlNodeType.Document, 0);
        }

        internal HtmlNode GetXmlDeclaration() {
            if (!_documentnode.HasChildNodes) {
                return null;
            }

            foreach (HtmlNode node in _documentnode._childnodes) {
                if (node.Name == "?xml") // it's ok, names are case sensitive
				{
                    return node;
                }
            }
            return null;
        }

        public static string HtmlEncode(string html) {
            if (html == null) {
                throw new ArgumentNullException("html");
            }
            // replace & by &amp; but only once!
            Regex rx = new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase);
            return rx.Replace(html, "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        public void Load(Stream stream) {
            Load(new StreamReader(stream, OptionDefaultStreamEncoding));
        }

        public void Load(Stream stream, bool detectEncodingFromByteOrderMarks) {
            Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
        }

        public void Load(Stream stream, Encoding encoding) {
            Load(new StreamReader(stream, encoding));
        }

        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks) {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
        }

        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize) {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
        }

        public void Load(string path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            StreamReader sr = new StreamReader(path, OptionDefaultStreamEncoding);
            Load(sr);
            sr.Close();
        }

        public void Load(string path, bool detectEncodingFromByteOrderMarks) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            StreamReader sr = new StreamReader(path, detectEncodingFromByteOrderMarks);
            Load(sr);
            sr.Close();
        }

        public void Load(string path, Encoding encoding) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            StreamReader sr = new StreamReader(path, encoding);
            Load(sr);
            sr.Close();
        }

        public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            StreamReader sr = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks);
            Load(sr);
            sr.Close();
        }

        public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            StreamReader sr = new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize);
            Load(sr);
            sr.Close();
        }

        public void LoadHtml(string html) {
            if (html == null) {
                throw new ArgumentNullException("html");
            }
            StringReader sr = new StringReader(html);
            Load(sr);
            sr.Close();
        }

        public void Load(TextReader reader) {
            // all Load methods pass down to this one
            if (reader == null) {
                throw new ArgumentNullException("reader");
            }

            if (OptionCheckSyntax) {
                _openednodes = new Hashtable();
            }
            else {
                _openednodes = null;
            }

            if (OptionUseIdAttribute) {
                _nodesid = new Hashtable();
            }
            else {
                _nodesid = null;
            }

            StreamReader sr = reader as StreamReader;
            if (sr != null) {
                try {
                    // trigger bom read if needed
                    sr.Peek();
                }
                catch {
                    // void on purpose
                }
                _streamencoding = sr.CurrentEncoding;
            }
            else {
                _streamencoding = null;
            }
            _declaredencoding = null;

            _text = reader.ReadToEnd();
            _documentnode = CreateNode(HtmlNodeType.Document, 0);
            Parse();

            if (OptionCheckSyntax) {
                foreach (HtmlNode node in _openednodes.Values) {
                    if (!node._starttag)	// already reported
					{
                        continue;
                    }

                    string html;
                    if (OptionExtractErrorSourceText) {
                        html = node.OuterHtml;
                        if (html.Length > OptionExtractErrorSourceTextMaxLength) {
                            html = html.Substring(0, OptionExtractErrorSourceTextMaxLength);
                        }
                    }
                    else {
                        html = string.Empty;
                    }
                    AddError(
                        HtmlParseErrorCode.TagNotClosed,
                        node._line, node._lineposition,
                        node._streamposition, html,
                        "End tag </" + node.Name + "> was not found");
                }

                // we don't need this anymore
                _openednodes.Clear();
            }
        }

        internal System.Text.Encoding GetOutEncoding() {
            // when unspecified, use the stream encoding first
            if (_declaredencoding != null) {
                return _declaredencoding;
            }
            else {
                if (_streamencoding != null) {
                    return _streamencoding;
                }
            }
            return OptionDefaultStreamEncoding;
        }

        public System.Text.Encoding Encoding {
            get {
                return GetOutEncoding();
            }
        }

        public void Save(Stream outStream) {
            StreamWriter sw = new StreamWriter(outStream, GetOutEncoding());
            Save(sw);
        }

        public void Save(Stream outStream, System.Text.Encoding encoding) {
            if (outStream == null) {
                throw new ArgumentNullException("outStream");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            StreamWriter sw = new StreamWriter(outStream, encoding);
            Save(sw);
        }

        public void Save(string filename) {
            StreamWriter sw = new StreamWriter(filename, false, GetOutEncoding());
            Save(sw);
            sw.Close();
        }

        public void Save(string filename, System.Text.Encoding encoding) {
            if (filename == null) {
                throw new ArgumentNullException("filename");
            }
            if (encoding == null) {
                throw new ArgumentNullException("encoding");
            }
            StreamWriter sw = new StreamWriter(filename, false, encoding);
            Save(sw);
            sw.Close();
        }

        public void Save(StreamWriter writer) {
            Save((TextWriter)writer);
        }

        public void Save(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }
            DocumentNode.WriteTo(writer);
        }

        public void Save(XmlWriter writer) {
            DocumentNode.WriteTo(writer);
            writer.Flush();
        }

        public static string GetXmlName(string name) {
            string xmlname = string.Empty;
            bool nameisok = true;
            for (int i = 0; i < name.Length; i++) {
                // names are lcase
                // note: we are very limited here, too much?
                if (((name[i] >= 'a') && (name[i] <= 'z')) ||
                    ((name[i] >= '0') && (name[i] <= '9')) ||
                    //					(name[i]==':') || (name[i]=='_') || (name[i]=='-') || (name[i]=='.')) // these are bads in fact
                    (name[i] == '_') || (name[i] == '-') || (name[i] == '.')) {
                    xmlname += name[i];
                }
                else {
                    nameisok = false;
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(new char[] { name[i] });
                    for (int j = 0; j < bytes.Length; j++) {
                        xmlname += bytes[j].ToString("x2");
                    }
                    xmlname += "_";
                }

            }
            if (nameisok) {
                return xmlname;
            }
            return "_" + xmlname;
        }

        internal void SetIdForNode(HtmlNode node, string id) {
            if (!OptionUseIdAttribute) {
                return;
            }

            if ((_nodesid == null) || (id == null)) {
                return;
            }

            if (node == null) {
                _nodesid.Remove(id.ToLower());
            }
            else {
                _nodesid[id.ToLower()] = node;
            }
        }

        public HtmlNode GetElementbyId(string id) {
            if (id == null) {
                throw new ArgumentNullException("id");
            }
            if (_nodesid == null) {
                throw new Exception(HtmlExceptionUseIdAttributeFalse);
            }

            return _nodesid[id.ToLower()] as HtmlNode;
        }

        public HtmlNode CreateElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            HtmlNode node = CreateNode(HtmlNodeType.Element);
            node._name = name;
            return node;
        }

        public HtmlCommentNode CreateComment() {
            return (HtmlCommentNode)CreateNode(HtmlNodeType.Comment);
        }

        public HtmlCommentNode CreateComment(string comment) {
            if (comment == null) {
                throw new ArgumentNullException("comment");
            }
            HtmlCommentNode c = CreateComment();
            c.Comment = comment;
            return c;
        }

        public HtmlTextNode CreateTextNode() {
            return (HtmlTextNode)CreateNode(HtmlNodeType.Text);
        }

        public HtmlTextNode CreateTextNode(string text) {
            if (text == null) {
                throw new ArgumentNullException("text");
            }
            HtmlTextNode t = CreateTextNode();
            t.Text = text;
            return t;
        }

        internal HtmlNode CreateNode(HtmlNodeType type) {
            return CreateNode(type, -1);
        }

        internal HtmlNode CreateNode(HtmlNodeType type, int index) {
            switch (type) {
                case HtmlNodeType.Comment:
                    return new HtmlCommentNode(this, index);

                case HtmlNodeType.Text:
                    return new HtmlTextNode(this, index);

                default:
                    return new HtmlNode(type, this, index);
            }
        }

        internal HtmlAttribute CreateAttribute() {
            return new HtmlAttribute(this);
        }

        public HtmlAttribute CreateAttribute(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            HtmlAttribute att = CreateAttribute();
            att.Name = name;
            return att;
        }

        public HtmlAttribute CreateAttribute(string name, string value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            HtmlAttribute att = CreateAttribute(name);
            att.Value = value;
            return att;
        }

        public HtmlNode DocumentNode {
            get {
                return _documentnode;
            }
        }

        private HtmlParseError AddError(
                HtmlParseErrorCode code,
                int line,
                int linePosition,
                int streamPosition,
                string sourceText,
                string reason) {
            HtmlParseError err = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
            _parseerrors.Add(err);
            return err;
        }

        private enum ParseState {
            Text,
            WhichTag,
            Tag,
            BetweenAttributes,
            EmptyTag,
            AttributeName,
            AttributeBeforeEquals,
            AttributeAfterEquals,
            AttributeValue,
            Comment,
            QuotedAttributeValue,
            ServerSideCode,
            PcData
        }

        private void IncrementPosition() {
            _index++;
            _maxlineposition = _lineposition;
            if (_c == 10) {
                _lineposition = 1;
                _line++;
            }
            else {
                _lineposition++;
            }
        }

        private void DecrementPosition() {
            _index--;
            if (_lineposition == 1) {
                _lineposition = _maxlineposition;
                _line--;
            }
            else {
                _lineposition--;
            }
        }

        private void Parse() {
            int lastquote = 0;

            _lastnodes = new Hashtable();
            _c = 0;
            _fullcomment = false;
            _parseerrors = new ArrayList();
            _line = 1;
            _lineposition = 1;
            _maxlineposition = 1;

            _state = ParseState.Text;
            _oldstate = _state;
            _documentnode._innerlength = _text.Length;
            _documentnode._outerlength = _text.Length;
            _remainderOffset = _text.Length;

            _lastparentnode = _documentnode;
            _currentnode = CreateNode(HtmlNodeType.Text, 0);
            _currentattribute = null;

            _index = 0;
            PushNodeStart(HtmlNodeType.Text, 0);
            while (_index < _text.Length) {
                _c = _text[_index];
                IncrementPosition();

                switch (_state) {
                    case ParseState.Text:
                        if (NewCheck())
                            continue;
                        break;

                    case ParseState.WhichTag:
                        if (NewCheck())
                            continue;
                        if (_c == '/') {
                            PushNodeNameStart(false, _index);
                        }
                        else {
                            PushNodeNameStart(true, _index - 1);
                            DecrementPosition();
                        }
                        _state = ParseState.Tag;
                        break;

                    case ParseState.Tag:
                        if (NewCheck())
                            continue;
                        if (IsWhiteSpace(_c)) {
                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag)
                                continue;
                            _state = ParseState.BetweenAttributes;
                            continue;
                        }
                        if (_c == '/') {
                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag)
                                continue;
                            _state = ParseState.EmptyTag;
                            continue;
                        }
                        if (_c == '>') {
                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag)
                                continue;
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            if (_state != ParseState.Tag)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                        }
                        break;

                    case ParseState.BetweenAttributes:
                        if (NewCheck())
                            continue;

                        if (IsWhiteSpace(_c))
                            continue;

                        if ((_c == '/') || (_c == '?')) {
                            _state = ParseState.EmptyTag;
                            continue;
                        }

                        if (_c == '>') {
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }

                            if (_state != ParseState.BetweenAttributes)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }

                        PushAttributeNameStart(_index - 1);
                        _state = ParseState.AttributeName;
                        break;

                    case ParseState.EmptyTag:
                        if (NewCheck())
                            continue;

                        if (_c == '>') {
                            if (!PushNodeEnd(_index, true)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }

                            if (_state != ParseState.EmptyTag)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        _state = ParseState.BetweenAttributes;
                        break;

                    case ParseState.AttributeName:
                        if (NewCheck())
                            continue;

                        if (IsWhiteSpace(_c)) {
                            PushAttributeNameEnd(_index - 1);
                            _state = ParseState.AttributeBeforeEquals;
                            continue;
                        }
                        if (_c == '=') {
                            PushAttributeNameEnd(_index - 1);
                            _state = ParseState.AttributeAfterEquals;
                            continue;
                        }
                        if (_c == '>') {
                            PushAttributeNameEnd(_index - 1);
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            if (_state != ParseState.AttributeName)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        break;

                    case ParseState.AttributeBeforeEquals:
                        if (NewCheck())
                            continue;

                        if (IsWhiteSpace(_c))
                            continue;
                        if (_c == '>') {
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            if (_state != ParseState.AttributeBeforeEquals)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        if (_c == '=') {
                            _state = ParseState.AttributeAfterEquals;
                            continue;
                        }
                        // no equals, no whitespace, it's a new attrribute starting
                        _state = ParseState.BetweenAttributes;
                        DecrementPosition();
                        break;

                    case ParseState.AttributeAfterEquals:
                        if (NewCheck())
                            continue;

                        if (IsWhiteSpace(_c))
                            continue;

                        if ((_c == '\'') || (_c == '"')) {
                            _state = ParseState.QuotedAttributeValue;
                            PushAttributeValueStart(_index);
                            lastquote = _c;
                            continue;
                        }
                        if (_c == '>') {
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            if (_state != ParseState.AttributeAfterEquals)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        PushAttributeValueStart(_index - 1);
                        _state = ParseState.AttributeValue;
                        break;

                    case ParseState.AttributeValue:
                        if (NewCheck())
                            continue;

                        if (IsWhiteSpace(_c)) {
                            PushAttributeValueEnd(_index - 1);
                            _state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (_c == '>') {
                            PushAttributeValueEnd(_index - 1);
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            if (_state != ParseState.AttributeValue)
                                continue;
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        break;

                    case ParseState.QuotedAttributeValue:
                        if (_c == lastquote) {
                            PushAttributeValueEnd(_index - 1);
                            _state = ParseState.BetweenAttributes;
                            continue;
                        }
                        if (_c == '<') {
                            if (_index < _text.Length) {
                                if (_text[_index] == '%') {
                                    _oldstate = _state;
                                    _state = ParseState.ServerSideCode;
                                    continue;
                                }
                            }
                        }
                        break;

                    case ParseState.Comment:
                        if (_c == '>') {
                            if (_fullcomment) {
                                if ((_text[_index - 2] != '-') ||
                                    (_text[_index - 3] != '-')) {
                                    continue;
                                }
                            }
                            if (!PushNodeEnd(_index, false)) {
                                // stop parsing
                                _index = _text.Length;
                                break;
                            }
                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index);
                            continue;
                        }
                        break;

                    case ParseState.ServerSideCode:
                        if (_c == '%') {
                            if (_index < _text.Length) {
                                if (_text[_index] == '>') {
                                    switch (_oldstate) {
                                        case ParseState.AttributeAfterEquals:
                                            _state = ParseState.AttributeValue;
                                            break;

                                        case ParseState.BetweenAttributes:
                                            PushAttributeNameEnd(_index + 1);
                                            _state = ParseState.BetweenAttributes;
                                            break;

                                        default:
                                            _state = _oldstate;
                                            break;
                                    }
                                    IncrementPosition();
                                }
                            }
                        }
                        break;

                    case ParseState.PcData:
                        // look for </tag + 1 char

                        // check buffer end
                        if ((_currentnode._namelength + 3) <= (_text.Length - (_index - 1))) {
                            if (string.Compare(_text.Substring(_index - 1, _currentnode._namelength + 2),
                                "</" + _currentnode.Name, true) == 0) {
                                int c = _text[_index - 1 + 2 + _currentnode.Name.Length];
                                if ((c == '>') || (IsWhiteSpace(c))) {
                                    // add the script as a text node
                                    HtmlNode script = CreateNode(HtmlNodeType.Text,
                                        _currentnode._outerstartindex + _currentnode._outerlength);
                                    script._outerlength = _index - 1 - script._outerstartindex;
                                    _currentnode.AppendChild(script);

                                    PushNodeStart(HtmlNodeType.Element, _index - 1);
                                    PushNodeNameStart(false, _index - 1 + 2);
                                    _state = ParseState.Tag;
                                    IncrementPosition();
                                }
                            }
                        }
                        break;
                }
            }

            // finish the current work
            if (_currentnode._namestartindex > 0) {
                PushNodeNameEnd(_index);
            }
            PushNodeEnd(_index, false);

            // we don't need this anymore
            _lastnodes.Clear();
        }

        private bool NewCheck() {
            if (_c != '<') {
                return false;
            }
            if (_index < _text.Length) {
                if (_text[_index] == '%') {
                    switch (_state) {
                        case ParseState.AttributeAfterEquals:
                            PushAttributeValueStart(_index - 1);
                            break;

                        case ParseState.BetweenAttributes:
                            PushAttributeNameStart(_index - 1);
                            break;

                        case ParseState.WhichTag:
                            PushNodeNameStart(true, _index - 1);
                            _state = ParseState.Tag;
                            break;
                    }
                    _oldstate = _state;
                    _state = ParseState.ServerSideCode;
                    return true;
                }
            }

            if (!PushNodeEnd(_index - 1, true)) {
                // stop parsing
                _index = _text.Length;
                return true;
            }
            _state = ParseState.WhichTag;
            if ((_index - 1) <= (_text.Length - 2)) {
                if (_text[_index] == '!') {
                    PushNodeStart(HtmlNodeType.Comment, _index - 1);
                    PushNodeNameStart(true, _index);
                    PushNodeNameEnd(_index + 1);
                    _state = ParseState.Comment;
                    if (_index < (_text.Length - 2)) {
                        if ((_text[_index + 1] == '-') &&
                            (_text[_index + 2] == '-')) {
                            _fullcomment = true;
                        }
                        else {
                            _fullcomment = false;
                        }
                    }
                    return true;
                }
            }
            PushNodeStart(HtmlNodeType.Element, _index - 1);
            return true;
        }

        private void PushAttributeNameStart(int index) {
            _currentattribute = CreateAttribute();
            _currentattribute._namestartindex = index;
            _currentattribute._line = _line;
            _currentattribute._lineposition = _lineposition;
            _currentattribute._streamposition = index;
        }

        private void PushAttributeNameEnd(int index) {
            _currentattribute._namelength = index - _currentattribute._namestartindex;
            _currentnode.Attributes.Append(_currentattribute);
        }

        private void PushAttributeValueStart(int index) {
            _currentattribute._valuestartindex = index;
        }

        private void PushAttributeValueEnd(int index) {
            _currentattribute._valuelength = index - _currentattribute._valuestartindex;
        }

        private void PushNodeStart(HtmlNodeType type, int index) {
            _currentnode = CreateNode(type, index);
            _currentnode._line = _line;
            _currentnode._lineposition = _lineposition;
            if (type == HtmlNodeType.Element) {
                _currentnode._lineposition--;
            }
            _currentnode._streamposition = index;
        }

        private bool PushNodeEnd(int index, bool close) {
            _currentnode._outerlength = index - _currentnode._outerstartindex;

            if ((_currentnode._nodetype == HtmlNodeType.Text) ||
                (_currentnode._nodetype == HtmlNodeType.Comment)) {
                // forget about void nodes
                if (_currentnode._outerlength > 0) {
                    _currentnode._innerlength = _currentnode._outerlength;
                    _currentnode._innerstartindex = _currentnode._outerstartindex;
                    if (_lastparentnode != null) {
                        _lastparentnode.AppendChild(_currentnode);
                    }
                }
            }
            else {
                if ((_currentnode._starttag) && (_lastparentnode != _currentnode)) {
                    // add to parent node
                    if (_lastparentnode != null) {
                        _lastparentnode.AppendChild(_currentnode);
                    }

                    // remember last node of this kind
                    HtmlNode prev = (HtmlNode)_lastnodes[_currentnode.Name];
                    _currentnode._prevwithsamename = prev;
                    _lastnodes[_currentnode.Name] = _currentnode;

                    // change parent?
                    if ((_currentnode.NodeType == HtmlNodeType.Document) ||
                        (_currentnode.NodeType == HtmlNodeType.Element)) {
                        _lastparentnode = _currentnode;
                    }

                    if (HtmlNode.IsCDataElement(CurrentNodeName())) {
                        _state = ParseState.PcData;
                        return true;
                    }

                    if ((HtmlNode.IsClosedElement(_currentnode.Name)) ||
                        (HtmlNode.IsEmptyElement(_currentnode.Name))) {
                        close = true;
                    }
                }
            }

            if ((close) || (!_currentnode._starttag)) {
                if ((OptionStopperNodeName != null) && (_remainder == null) &&
                    (string.Compare(_currentnode.Name, OptionStopperNodeName, true) == 0)) {
                    _remainderOffset = index;
                    _remainder = _text.Substring(_remainderOffset);
                    CloseCurrentNode();
                    return false; // stop parsing
                }
                CloseCurrentNode();
            }
            return true;
        }

        private void PushNodeNameStart(bool starttag, int index) {
            _currentnode._starttag = starttag;
            _currentnode._namestartindex = index;
        }

        private string[] GetResetters(string name) {
            switch (name) {
                case "li":
                    return new string[] { "ul" };

                case "tr":
                    return new string[] { "table" };

                case "th":
                case "td":
                    return new string[] { "tr", "table" };

                default:
                    return null;
            }
        }

        private void FixNestedTags() {
            // we are only interested by start tags, not closing tags
            if (!_currentnode._starttag)
                return;

            string name = CurrentNodeName().ToLower();
            FixNestedTag(name, GetResetters(name));
        }

        private void FixNestedTag(string name, string[] resetters) {
            if (resetters == null)
                return;

            HtmlNode prev;

            // if we find a previous unclosed same name node, without a resetter node between, we must close it
            prev = (HtmlNode)_lastnodes[name];
            if ((prev != null) && (!prev.Closed)) {

                // try to find a resetter node, if found, we do nothing
                if (FindResetterNodes(prev, resetters)) {
                    return;
                }

                // ok we need to close the prev now
                // create a fake closer node
                HtmlNode close = new HtmlNode(prev.NodeType, this, -1);
                close._endnode = close;
                prev.CloseNode(close);

            }
        }

        private bool FindResetterNodes(HtmlNode node, string[] names) {
            if (names == null) {
                return false;
            }
            for (int i = 0; i < names.Length; i++) {
                if (FindResetterNode(node, names[i]) != null) {
                    return true;
                }
            }
            return false;
        }

        private HtmlNode FindResetterNode(HtmlNode node, string name) {
            HtmlNode resetter = (HtmlNode)_lastnodes[name];
            if (resetter == null)
                return null;
            if (resetter.Closed) {
                return null;
            }
            if (resetter._streamposition < node._streamposition) {
                return null;
            }
            return resetter;
        }

        private void PushNodeNameEnd(int index) {
            _currentnode._namelength = index - _currentnode._namestartindex;
            if (OptionFixNestedTags) {
                FixNestedTags();
            }
        }

        private void CloseCurrentNode() {
            if (_currentnode.Closed) // text or document are by def closed
                return;

            bool error = false;

            // find last node of this kind
            HtmlNode prev = (HtmlNode)_lastnodes[_currentnode.Name];
            if (prev == null) {
                if (HtmlNode.IsClosedElement(_currentnode.Name)) {
                    // </br> will be seen as <br>
                    _currentnode.CloseNode(_currentnode);

                    // add to parent node
                    if (_lastparentnode != null) {
                        HtmlNode foundNode = null;
                        Stack futureChild = new Stack();
                        for (HtmlNode node = _lastparentnode.LastChild; node != null; node = node.PreviousSibling) {
                            if ((node.Name == _currentnode.Name) && (!node.HasChildNodes)) {
                                foundNode = node;
                                break;
                            }
                            futureChild.Push(node);
                        }
                        if (foundNode != null) {
                            HtmlNode node = null;
                            while (futureChild.Count != 0) {
                                node = (HtmlNode)futureChild.Pop();
                                _lastparentnode.RemoveChild(node);
                                foundNode.AppendChild(node);
                            }
                        }
                        else {
                            _lastparentnode.AppendChild(_currentnode);
                        }

                    }
                }
                else {
                    // node has no parent
                    // node is not a closed node

                    if (HtmlNode.CanOverlapElement(_currentnode.Name)) {
                        // this is a hack: add it as a text node
                        HtmlNode closenode = CreateNode(HtmlNodeType.Text, _currentnode._outerstartindex);
                        closenode._outerlength = _currentnode._outerlength;
                        ((HtmlTextNode)closenode).Text = ((HtmlTextNode)closenode).Text.ToLower();
                        if (_lastparentnode != null) {
                            _lastparentnode.AppendChild(closenode);
                        }

                    }
                    else {
                        if (HtmlNode.IsEmptyElement(_currentnode.Name)) {
                            AddError(
                                HtmlParseErrorCode.EndTagNotRequired,
                                _currentnode._line, _currentnode._lineposition,
                                _currentnode._streamposition, _currentnode.OuterHtml,
                                "End tag </" + _currentnode.Name + "> is not required");
                        }
                        else {
                            // node cannot overlap, node is not empty
                            AddError(
                                HtmlParseErrorCode.TagNotOpened,
                                _currentnode._line, _currentnode._lineposition,
                                _currentnode._streamposition, _currentnode.OuterHtml,
                                "Start tag <" + _currentnode.Name + "> was not found");
                            error = true;
                        }
                    }
                }
            }
            else {
                if (OptionFixNestedTags) {
                    if (FindResetterNodes(prev, GetResetters(_currentnode.Name))) {
                        AddError(
                            HtmlParseErrorCode.EndTagInvalidHere,
                            _currentnode._line, _currentnode._lineposition,
                            _currentnode._streamposition, _currentnode.OuterHtml,
                            "End tag </" + _currentnode.Name + "> invalid here");
                        error = true;
                    }
                }

                if (!error) {
                    _lastnodes[_currentnode.Name] = prev._prevwithsamename;
                    prev.CloseNode(_currentnode);
                }
            }

            // we close this node, get grandparent
            if (!error) {
                if ((_lastparentnode != null) &&
                    ((!HtmlNode.IsClosedElement(_currentnode.Name)) ||
                    (_currentnode._starttag))) {
                    UpdateLastParentNode();
                }
            }
        }

        internal void UpdateLastParentNode() {
            do {
                if (_lastparentnode.Closed) {
                    _lastparentnode = _lastparentnode.ParentNode;
                }
            }
            while ((_lastparentnode != null) && (_lastparentnode.Closed));
            if (_lastparentnode == null) {
                _lastparentnode = _documentnode;
            }
        }

        private string CurrentAttributeName() {
            return _text.Substring(_currentattribute._namestartindex, _currentattribute._namelength);
        }

        private string CurrentAttributeValue() {
            return _text.Substring(_currentattribute._valuestartindex, _currentattribute._valuelength);
        }

        private string CurrentNodeName() {
            return _text.Substring(_currentnode._namestartindex, _currentnode._namelength);
        }

        private string CurrentNodeOuter() {
            return _text.Substring(_currentnode._outerstartindex, _currentnode._outerlength);
        }

        private string CurrentNodeInner() {
            return _text.Substring(_currentnode._innerstartindex, _currentnode._innerlength);
        }

        public static bool IsWhiteSpace(int c) {
            if ((c == 10) || (c == 13) || (c == 32) || (c == 9)) {
                return true;
            }
            return false;
        }
    }

    internal enum HtmlElementFlag {
        CData = 1,
        Empty = 2,
        Closed = 4,
        CanOverlap = 8
    }

    public class HtmlNode {

        public static readonly string HtmlNodeTypeNameComment = "#comment";

        public static readonly string HtmlNodeTypeNameDocument = "#document";

        public static readonly string HtmlNodeTypeNameText = "#text";

        public static Hashtable ElementsFlags;

        internal HtmlNodeType _nodetype;
        internal HtmlNode _nextnode;
        internal HtmlNode _prevnode;
        internal HtmlNode _parentnode;
        internal HtmlDocument _ownerdocument;
        internal HtmlNodeCollection _childnodes;
        internal HtmlAttributeCollection _attributes;
        internal int _line = 0;
        internal int _lineposition = 0;
        internal int _streamposition = 0;
        internal int _innerstartindex = 0;
        internal int _innerlength = 0;
        internal int _outerstartindex = 0;
        internal int _outerlength = 0;
        internal int _namestartindex = 0;
        internal int _namelength = 0;
        internal bool _starttag = false;
        internal string _name;
        internal HtmlNode _prevwithsamename = null;
        internal HtmlNode _endnode;

        internal bool _innerchanged = false;
        internal bool _outerchanged = false;
        internal string _innerhtml;
        internal string _outerhtml;

        static HtmlNode() {
            // tags whose content may be anything
            ElementsFlags = new Hashtable();
            ElementsFlags.Add("script", HtmlElementFlag.CData);
            ElementsFlags.Add("style", HtmlElementFlag.CData);
            ElementsFlags.Add("noxhtml", HtmlElementFlag.CData);

            // tags that can not contain other tags
            ElementsFlags.Add("base", HtmlElementFlag.Empty);
            ElementsFlags.Add("link", HtmlElementFlag.Empty);
            ElementsFlags.Add("meta", HtmlElementFlag.Empty);
            ElementsFlags.Add("isindex", HtmlElementFlag.Empty);
            ElementsFlags.Add("hr", HtmlElementFlag.Empty);
            ElementsFlags.Add("col", HtmlElementFlag.Empty);
            ElementsFlags.Add("img", HtmlElementFlag.Empty);
            ElementsFlags.Add("param", HtmlElementFlag.Empty);
            ElementsFlags.Add("embed", HtmlElementFlag.Empty);
            ElementsFlags.Add("frame", HtmlElementFlag.Empty);
            ElementsFlags.Add("wbr", HtmlElementFlag.Empty);
            ElementsFlags.Add("bgsound", HtmlElementFlag.Empty);
            ElementsFlags.Add("spacer", HtmlElementFlag.Empty);
            ElementsFlags.Add("keygen", HtmlElementFlag.Empty);
            ElementsFlags.Add("area", HtmlElementFlag.Empty);
            ElementsFlags.Add("input", HtmlElementFlag.Empty);
            ElementsFlags.Add("basefont", HtmlElementFlag.Empty);

            //ElementsFlags.Add("form", HtmlElementFlag.CanOverlap | HtmlElementFlag.Empty);

            // they sometimes contain, and sometimes they don 't...
            ElementsFlags.Add("option", HtmlElementFlag.Empty);

            // tag whose closing tag is equivalent to open tag:
            // <p>bla</p>bla will be transformed into <p>bla</p>bla
            // <p>bla<p>bla will be transformed into <p>bla<p>bla and not <p>bla></p><p>bla</p> or <p>bla<p>bla</p></p>
            //<br> see above
            ElementsFlags.Add("br", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
            ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
        }

        public static bool IsClosedElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            object flag = ElementsFlags[name.ToLower()];
            if (flag == null) {
                return false;
            }
            return (((HtmlElementFlag)flag) & HtmlElementFlag.Closed) != 0;
        }

        public static bool CanOverlapElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            object flag = ElementsFlags[name.ToLower()];
            if (flag == null) {
                return false;
            }
            return (((HtmlElementFlag)flag) & HtmlElementFlag.CanOverlap) != 0;
        }

        public static bool IsOverlappedClosingElement(string text) {
            if (text == null) {
                throw new ArgumentNullException("text");
            }
            // min is </x>: 4
            if (text.Length <= 4)
                return false;

            if ((text[0] != '<') ||
                (text[text.Length - 1] != '>') ||
                (text[1] != '/'))
                return false;

            string name = text.Substring(2, text.Length - 3);
            return CanOverlapElement(name);
        }

        public static bool IsCDataElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            object flag = ElementsFlags[name.ToLower()];
            if (flag == null) {
                return false;
            }
            return (((HtmlElementFlag)flag) & HtmlElementFlag.CData) != 0;
        }

        public static bool IsEmptyElement(string name) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (name.Length == 0) {
                return true;
            }

            // <!DOCTYPE ...
            if ('!' == name[0]) {
                return true;
            }

            // <?xml ...
            if ('?' == name[0]) {
                return true;
            }

            object flag = ElementsFlags[name.ToLower()];
            if (flag == null) {
                return false;
            }
            return (((HtmlElementFlag)flag) & HtmlElementFlag.Empty) != 0;
        }

        public static HtmlNode CreateNode(string html) {
            // REVIEW: this is *not* optimum...
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.FirstChild;
        }

        public void CopyFrom(HtmlNode node) {
            CopyFrom(node, true);
        }

        public void CopyFrom(HtmlNode node, bool deep) {
            if (node == null) {
                throw new ArgumentNullException("node");
            }

            Attributes.RemoveAll();
            if (node.HasAttributes) {
                foreach (HtmlAttribute att in node.Attributes) {
                    SetAttributeValue(att.Name, att.Value);
                }
            }

            if (!deep) {
                RemoveAllChildren();
                if (node.HasChildNodes) {
                    foreach (HtmlNode child in node.ChildNodes) {
                        AppendChild(child.CloneNode(true));
                    }
                }
            }
        }

        internal HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index) {
            _nodetype = type;
            _ownerdocument = ownerdocument;
            _outerstartindex = index;

            switch (type) {
                case HtmlNodeType.Comment:
                    _name = HtmlNodeTypeNameComment;
                    _endnode = this;
                    break;

                case HtmlNodeType.Document:
                    _name = HtmlNodeTypeNameDocument;
                    _endnode = this;
                    break;

                case HtmlNodeType.Text:
                    _name = HtmlNodeTypeNameText;
                    _endnode = this;
                    break;
            }

            if (_ownerdocument._openednodes != null) {
                if (!Closed) {
                    // we use the index as the key

                    // -1 means the node comes from public
                    if (-1 != index) {
                        _ownerdocument._openednodes.Add(index, this);
                    }
                }
            }

            if ((-1 == index) && (type != HtmlNodeType.Comment) && (type != HtmlNodeType.Text)) {
                // innerhtml and outerhtml must be calculated
                _outerchanged = true;
                _innerchanged = true;
            }
        }

        internal void CloseNode(HtmlNode endnode) {
            if (!_ownerdocument.OptionAutoCloseOnEnd) {
                // close all children
                if (_childnodes != null) {
                    foreach (HtmlNode child in _childnodes) {
                        if (child.Closed)
                            continue;

                        // create a fake closer node
                        HtmlNode close = new HtmlNode(NodeType, _ownerdocument, -1);
                        close._endnode = close;
                        child.CloseNode(close);
                    }
                }
            }

            if (!Closed) {
                _endnode = endnode;

                if (_ownerdocument._openednodes != null) {
                    _ownerdocument._openednodes.Remove(_outerstartindex);
                }

                HtmlNode self = _ownerdocument._lastnodes[Name] as HtmlNode;
                if (self == this) {
                    _ownerdocument._lastnodes.Remove(Name);
                    _ownerdocument.UpdateLastParentNode();
                }

                if (endnode == this)
                    return;

                // create an inner section
                _innerstartindex = _outerstartindex + _outerlength;
                _innerlength = endnode._outerstartindex - _innerstartindex;

                // update full length
                _outerlength = (endnode._outerstartindex + endnode._outerlength) - _outerstartindex;
            }
        }

        internal HtmlNode EndNode {
            get {
                return _endnode;
            }
        }

        internal string GetId() {
            HtmlAttribute att = Attributes["id"];
            if (att == null) {
                return null;
            }
            return att.Value;
        }

        internal void SetId(string id) {
            HtmlAttribute att = Attributes["id"];
            if (att == null) {
                att = _ownerdocument.CreateAttribute("id");
            }
            att.Value = id;
            _ownerdocument.SetIdForNode(this, att.Value);
            _outerchanged = true;
        }

        public string Id {
            get {
                if (_ownerdocument._nodesid == null) {
                    throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
                }
                return GetId();
            }
            set {
                if (_ownerdocument._nodesid == null) {
                    throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);
                }

                if (value == null) {
                    throw new ArgumentNullException("value");
                }
                SetId(value);
            }
        }

        public int Line {
            get {
                return _line;
            }
        }

        public int LinePosition {
            get {
                return _lineposition;
            }
        }

        public int StreamPosition {
            get {
                return _streamposition;
            }
        }

        public bool Closed {
            get {
                return (_endnode != null);
            }
        }

        public string Name {
            get {
                if (_name == null) {
                    _name = _ownerdocument._text.Substring(_namestartindex, _namelength).ToLower();
                }
                return _name;
            }
            set {
                _name = value;
            }
        }

        public virtual string InnerText {
            get {
                if (_nodetype == HtmlNodeType.Text) {
                    return ((HtmlTextNode)this).Text;
                }

                if (_nodetype == HtmlNodeType.Comment) {
                    return ((HtmlCommentNode)this).Comment;
                }

                // note: right now, this method is *slow*, because we recompute everything.
                // it could be optimised like innerhtml
                if (!HasChildNodes) {
                    return string.Empty;
                }

                string s = null;
                foreach (HtmlNode node in ChildNodes) {
                    s += node.InnerText;
                }
                return s;
            }
        }

        public virtual string InnerHtml {
            get {
                if (_innerchanged) {
                    _innerhtml = WriteContentTo();
                    _innerchanged = false;
                    return _innerhtml;
                }
                if (_innerhtml != null) {
                    return _innerhtml;
                }

                if (_innerstartindex < 0) {
                    return string.Empty;
                }

                return _ownerdocument._text.Substring(_innerstartindex, _innerlength);
            }
            set {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(value);

                RemoveAllChildren();
                AppendChildren(doc.DocumentNode.ChildNodes);
            }
        }

        public virtual string OuterHtml {
            get {
                if (_outerchanged) {
                    _outerhtml = WriteTo();
                    _outerchanged = false;
                    return _outerhtml;
                }

                if (_outerhtml != null) {
                    return _outerhtml;
                }

                if (_outerstartindex < 0) {
                    return string.Empty;
                }

                return _ownerdocument._text.Substring(_outerstartindex, _outerlength);
            }
        }

        public HtmlNode Clone() {
            return CloneNode(true);
        }

        public HtmlNode CloneNode(string newName) {
            return CloneNode(newName, true);
        }

        public HtmlNode CloneNode(string newName, bool deep) {
            if (newName == null) {
                throw new ArgumentNullException("newName");
            }

            HtmlNode node = CloneNode(deep);
            node._name = newName;
            return node;
        }

        public HtmlNode CloneNode(bool deep) {
            HtmlNode node = _ownerdocument.CreateNode(_nodetype);
            node._name = Name;

            switch (_nodetype) {
                case HtmlNodeType.Comment:
                    ((HtmlCommentNode)node).Comment = ((HtmlCommentNode)this).Comment;
                    return node;

                case HtmlNodeType.Text:
                    ((HtmlTextNode)node).Text = ((HtmlTextNode)this).Text;
                    return node;
            }

            // attributes
            if (HasAttributes) {
                foreach (HtmlAttribute att in _attributes) {
                    HtmlAttribute newatt = att.Clone();
                    node.Attributes.Append(newatt);
                }
            }

            // closing attributes
            if (HasClosingAttributes) {
                node._endnode = _endnode.CloneNode(false);
                foreach (HtmlAttribute att in _endnode._attributes) {
                    HtmlAttribute newatt = att.Clone();
                    node._endnode._attributes.Append(newatt);
                }
            }
            if (!deep) {
                return node;
            }

            if (!HasChildNodes) {
                return node;
            }

            // child nodes
            foreach (HtmlNode child in _childnodes) {
                HtmlNode newchild = child.Clone();
                node.AppendChild(newchild);
            }
            return node;
        }

        public HtmlNode NextSibling {
            get {
                return _nextnode;
            }
        }

        public HtmlNode PreviousSibling {
            get {
                return _prevnode;
            }
        }

        public void RemoveAll() {
            RemoveAllChildren();

            if (HasAttributes) {
                _attributes.Clear();
            }

            if ((_endnode != null) && (_endnode != this)) {
                if (_endnode._attributes != null) {
                    _endnode._attributes.Clear();
                }
            }
            _outerchanged = true;
            _innerchanged = true;
        }

        public void RemoveAllChildren() {
            if (!HasChildNodes) {
                return;
            }

            if (_ownerdocument.OptionUseIdAttribute) {
                // remove nodes from id list
                foreach (HtmlNode node in _childnodes) {
                    _ownerdocument.SetIdForNode(null, node.GetId());
                }
            }
            _childnodes.Clear();
            _outerchanged = true;
            _innerchanged = true;
        }

        public HtmlNode RemoveChild(HtmlNode oldChild) {
            if (oldChild == null) {
                throw new ArgumentNullException("oldChild");
            }

            int index = -1;

            if (_childnodes != null) {
                index = _childnodes[oldChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            _childnodes.Remove(index);

            _ownerdocument.SetIdForNode(null, oldChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return oldChild;
        }

        public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren) {
            if (oldChild == null) {
                throw new ArgumentNullException("oldChild");
            }

            if ((oldChild._childnodes != null) && keepGrandChildren) {
                // get prev sibling
                HtmlNode prev = oldChild.PreviousSibling;

                // reroute grand children to ourselves
                foreach (HtmlNode grandchild in oldChild._childnodes) {
                    InsertAfter(grandchild, prev);
                }
            }
            RemoveChild(oldChild);
            _outerchanged = true;
            _innerchanged = true;
            return oldChild;
        }

        public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild) {
            if (newChild == null) {
                return RemoveChild(oldChild);
            }

            if (oldChild == null) {
                return AppendChild(newChild);
            }

            int index = -1;

            if (_childnodes != null) {
                index = _childnodes[oldChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            _childnodes.Replace(index, newChild);

            _ownerdocument.SetIdForNode(null, oldChild.GetId());
            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return newChild;
        }

        public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            if (refChild == null) {
                return AppendChild(newChild);
            }

            if (newChild == refChild) {
                return newChild;
            }

            int index = -1;

            if (_childnodes != null) {
                index = _childnodes[refChild];
            }

            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            _childnodes.Insert(index, newChild);

            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return newChild;
        }

        public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            if (refChild == null) {
                return PrependChild(newChild);
            }

            if (newChild == refChild) {
                return newChild;
            }

            int index = -1;

            if (_childnodes != null) {
                index = _childnodes[refChild];
            }
            if (index == -1) {
                throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);
            }

            _childnodes.Insert(index + 1, newChild);

            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return newChild;
        }

        public HtmlNode FirstChild {
            get {
                if (!HasChildNodes) {
                    return null;
                }
                return _childnodes[0];
            }
        }

        public HtmlNode LastChild {
            get {
                if (!HasChildNodes) {
                    return null;
                }
                return _childnodes[_childnodes.Count - 1];
            }
        }

        public HtmlNodeType NodeType {
            get {
                return _nodetype;
            }
        }

        public HtmlNode ParentNode {
            get {
                return _parentnode;
            }
        }

        public HtmlDocument OwnerDocument {
            get {
                return _ownerdocument;
            }
        }

        public HtmlNodeCollection ChildNodes {
            get {
                if (_childnodes == null) {
                    _childnodes = new HtmlNodeCollection(this);
                }
                return _childnodes;
            }
        }

        public HtmlNode PrependChild(HtmlNode newChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }
            ChildNodes.Prepend(newChild);
            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return newChild;
        }

        public void PrependChildren(HtmlNodeCollection newChildren) {
            if (newChildren == null) {
                throw new ArgumentNullException("newChildren");
            }

            foreach (HtmlNode newChild in newChildren) {
                PrependChild(newChild);
            }
        }

        public HtmlNode AppendChild(HtmlNode newChild) {
            if (newChild == null) {
                throw new ArgumentNullException("newChild");
            }

            ChildNodes.Append(newChild);
            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            _outerchanged = true;
            _innerchanged = true;
            return newChild;
        }

        public void AppendChildren(HtmlNodeCollection newChildren) {
            if (newChildren == null)
                throw new ArgumentNullException("newChildrend");

            foreach (HtmlNode newChild in newChildren) {
                AppendChild(newChild);
            }
        }

        public bool HasAttributes {
            get {
                if (_attributes == null) {
                    return false;
                }

                if (_attributes.Count <= 0) {
                    return false;
                }
                return true;
            }
        }

        public bool HasClosingAttributes {
            get {
                if ((_endnode == null) || (_endnode == this)) {
                    return false;
                }

                if (_endnode._attributes == null) {
                    return false;
                }

                if (_endnode._attributes.Count <= 0) {
                    return false;
                }
                return true;
            }
        }

        public bool HasChildNodes {
            get {
                if (_childnodes == null) {
                    return false;
                }

                if (_childnodes.Count <= 0) {
                    return false;
                }
                return true;
            }
        }

        public string GetAttributeValue(string name, string def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!HasAttributes) {
                return def;
            }
            HtmlAttribute att = Attributes[name];
            if (att == null) {
                return def;
            }
            return att.Value;
        }

        public int GetAttributeValue(string name, int def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!HasAttributes) {
                return def;
            }
            HtmlAttribute att = Attributes[name];
            if (att == null) {
                return def;
            }
            try {
                return Convert.ToInt32(att.Value);
            }
            catch {
                return def;
            }
        }

        public bool GetAttributeValue(string name, bool def) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (!HasAttributes) {
                return def;
            }
            HtmlAttribute att = Attributes[name];
            if (att == null) {
                return def;
            }
            try {
                return Convert.ToBoolean(att.Value);
            }
            catch {
                return def;
            }
        }

        public HtmlAttribute SetAttributeValue(string name, string value) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }
            HtmlAttribute att = Attributes[name];
            if (att == null) {
                return Attributes.Append(_ownerdocument.CreateAttribute(name, value));
            }
            att.Value = value;
            return att;
        }

        public HtmlAttributeCollection Attributes {
            get {
                if (!HasAttributes) {
                    _attributes = new HtmlAttributeCollection(this);
                }
                return _attributes;
            }
        }

        public HtmlAttributeCollection ClosingAttributes {
            get {
                if (!HasClosingAttributes) {
                    return new HtmlAttributeCollection(this);
                }
                return _endnode.Attributes;
            }
        }

        internal void WriteAttribute(TextWriter outText, HtmlAttribute att) {
            string name;

            if (_ownerdocument.OptionOutputAsXml) {
                if (_ownerdocument.OptionOutputUpperCase) {
                    name = att.XmlName.ToUpper();
                }
                else {
                    name = att.XmlName;
                }

                outText.Write(" " + name + "=\"" + HtmlDocument.HtmlEncode(att.XmlValue) + "\"");
            }
            else {
                if (_ownerdocument.OptionOutputUpperCase) {
                    name = att.Name.ToUpper();
                }
                else {
                    name = att.Name;
                }

                if (att.Name.Length >= 4) {
                    if ((att.Name[0] == '<') && (att.Name[1] == '%') &&
                        (att.Name[att.Name.Length - 1] == '>') && (att.Name[att.Name.Length - 2] == '%')) {
                        outText.Write(" " + name);
                        return;
                    }
                }
                if (_ownerdocument.OptionOutputOptimizeAttributeValues) {
                    if (att.Value.IndexOfAny(new Char[] { (char)10, (char)13, (char)9, ' ' }) < 0) {
                        outText.Write(" " + name + "=" + att.Value);
                    }
                    else {
                        outText.Write(" " + name + "=\"" + att.Value + "\"");
                    }
                }
                else {
                    outText.Write(" " + name + "=\"" + att.Value + "\"");
                }
            }
        }

        internal static void WriteAttributes(XmlWriter writer, HtmlNode node) {
            if (!node.HasAttributes) {
                return;
            }
            // we use _hashitems to make sure attributes are written only once
            foreach (HtmlAttribute att in node.Attributes._hashitems.Values) {
                writer.WriteAttributeString(att.XmlName, att.Value);
            }
        }

        internal void WriteAttributes(TextWriter outText, bool closing) {
            if (_ownerdocument.OptionOutputAsXml) {
                if (_attributes == null) {
                    return;
                }
                // we use _hashitems to make sure attributes are written only once
                foreach (HtmlAttribute att in _attributes._hashitems.Values) {
                    WriteAttribute(outText, att);
                }
                return;
            }

            if (!closing) {
                if (_attributes != null) {

                    foreach (HtmlAttribute att in _attributes) {
                        WriteAttribute(outText, att);
                    }
                }
                if (_ownerdocument.OptionAddDebuggingAttributes) {
                    WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
                    WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));

                    int i = 0;
                    foreach (HtmlNode n in ChildNodes) {
                        WriteAttribute(outText, _ownerdocument.CreateAttribute("_child_" + i,
                            n.Name));
                        i++;
                    }
                }
            }
            else {
                if (_endnode == null) {
                    return;
                }

                if (_endnode._attributes == null) {
                    return;
                }

                if (_endnode == this) {
                    return;
                }

                foreach (HtmlAttribute att in _endnode._attributes) {
                    WriteAttribute(outText, att);
                }
                if (_ownerdocument.OptionAddDebuggingAttributes) {
                    WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
                    WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));
                }
            }
        }

        internal static string GetXmlComment(HtmlCommentNode comment) {
            string s = comment.Comment;
            return s.Substring(4, s.Length - 7).Replace("--", " - -");
        }

        public void WriteTo(TextWriter outText) {
            string html;
            switch (_nodetype) {
                case HtmlNodeType.Comment:
                    html = ((HtmlCommentNode)this).Comment;
                    if (_ownerdocument.OptionOutputAsXml) {
                        outText.Write("<!--" + GetXmlComment((HtmlCommentNode)this) + " -->");
                    }
                    else {
                        outText.Write(html);
                    }
                    break;

                case HtmlNodeType.Document:
                    if (_ownerdocument.OptionOutputAsXml) {
                        outText.Write("<?xml version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName + "\"?>");

                        // check there is a root element
                        if (_ownerdocument.DocumentNode.HasChildNodes) {
                            int rootnodes = _ownerdocument.DocumentNode._childnodes.Count;
                            if (rootnodes > 0) {
                                HtmlNode xml = _ownerdocument.GetXmlDeclaration();
                                if (xml != null) {
                                    rootnodes--;
                                }

                                if (rootnodes > 1) {
                                    if (_ownerdocument.OptionOutputUpperCase) {
                                        outText.Write("<SPAN>");
                                        WriteContentTo(outText);
                                        outText.Write("</SPAN>");
                                    }
                                    else {
                                        outText.Write("<span>");
                                        WriteContentTo(outText);
                                        outText.Write("</span>");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    WriteContentTo(outText);
                    break;

                case HtmlNodeType.Text:
                    html = ((HtmlTextNode)this).Text;
                    if (_ownerdocument.OptionOutputAsXml) {
                        outText.Write(HtmlDocument.HtmlEncode(html));
                    }
                    else {
                        outText.Write(html);
                    }
                    break;

                case HtmlNodeType.Element:
                    string name;
                    if (_ownerdocument.OptionOutputUpperCase) {
                        name = Name.ToUpper();
                    }
                    else {
                        name = Name;
                    }

                    if (_ownerdocument.OptionOutputAsXml) {
                        if (name.Length > 0) {
                            if (name[0] == '?') {
                                // forget this one, it's been done at the document level
                                break;
                            }

                            if (name.Trim().Length == 0) {
                                break;
                            }
                            name = HtmlDocument.GetXmlName(name);
                        }
                        else {
                            break;
                        }
                    }

                    outText.Write("<" + name);
                    WriteAttributes(outText, false);

                    if (!HasChildNodes) {
                        if (HtmlNode.IsEmptyElement(Name)) {
                            if ((_ownerdocument.OptionWriteEmptyNodes) || (_ownerdocument.OptionOutputAsXml)) {
                                outText.Write(" />");
                            }
                            else {
                                if (Name.Length > 0) {
                                    if (Name[0] == '?') {
                                        outText.Write("?");
                                    }
                                }

                                outText.Write(">");
                            }
                        }
                        else {
                            outText.Write("></" + name + ">");
                        }
                    }
                    else {
                        outText.Write(">");
                        bool cdata = false;
                        if (_ownerdocument.OptionOutputAsXml) {
                            if (HtmlNode.IsCDataElement(Name)) {
                                // this code and the following tries to output things as nicely as possible for old browsers.
                                cdata = true;
                                outText.Write("\r\n//<![CDATA[\r\n");
                            }
                        }

                        if (cdata) {
                            if (HasChildNodes) {
                                // child must be a text
                                ChildNodes[0].WriteTo(outText);
                            }
                            outText.Write("\r\n//]]>//\r\n");
                        }
                        else {
                            WriteContentTo(outText);
                        }

                        outText.Write("</" + name);
                        if (!_ownerdocument.OptionOutputAsXml) {
                            WriteAttributes(outText, true);
                        }
                        outText.Write(">");
                    }
                    break;
            }
        }

        public void WriteTo(XmlWriter writer) {
            string html;
            switch (_nodetype) {
                case HtmlNodeType.Comment:
                    writer.WriteComment(GetXmlComment((HtmlCommentNode)this));
                    break;

                case HtmlNodeType.Document:
                    writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName + "\"");
                    if (HasChildNodes) {
                        foreach (HtmlNode subnode in ChildNodes) {
                            subnode.WriteTo(writer);
                        }
                    }
                    break;

                case HtmlNodeType.Text:
                    html = ((HtmlTextNode)this).Text;
                    writer.WriteString(html);
                    break;

                case HtmlNodeType.Element:
                    string name;
                    if (_ownerdocument.OptionOutputUpperCase) {
                        name = Name.ToUpper();
                    }
                    else {
                        name = Name;
                    }
                    writer.WriteStartElement(name);
                    WriteAttributes(writer, this);

                    if (HasChildNodes) {
                        foreach (HtmlNode subnode in ChildNodes) {
                            subnode.WriteTo(writer);
                        }
                    }
                    writer.WriteEndElement();
                    break;
            }
        }

        public void WriteContentTo(TextWriter outText) {
            if (_childnodes == null) {
                return;
            }

            foreach (HtmlNode node in _childnodes) {
                node.WriteTo(outText);
            }
        }

        public string WriteTo() {
            StringWriter sw = new StringWriter();
            WriteTo(sw);
            sw.Flush();
            return sw.ToString();
        }

        public string WriteContentTo() {
            StringWriter sw = new StringWriter();
            WriteContentTo(sw);
            sw.Flush();
            return sw.ToString();
        }
    }

    public class HtmlNodeCollection : IEnumerable {
        private ArrayList _items = new ArrayList();
        private HtmlNode _parentnode;

        internal HtmlNodeCollection(HtmlNode parentnode) {
            _parentnode = parentnode; // may be null
        }

        public int Count {
            get {
                return _items.Count;
            }
        }

        internal void Clear() {
            foreach (HtmlNode node in _items) {
                node._parentnode = null;
                node._nextnode = null;
                node._prevnode = null;
            }
            _items.Clear();
        }

        internal void Remove(int index) {
            HtmlNode next = null;
            HtmlNode prev = null;
            HtmlNode oldnode = (HtmlNode)_items[index];

            if (index > 0) {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < (_items.Count - 1)) {
                next = (HtmlNode)_items[index + 1];
            }

            _items.RemoveAt(index);

            if (prev != null) {
                if (next == prev) {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = next;
            }

            if (next != null) {
                next._prevnode = prev;
            }

            oldnode._prevnode = null;
            oldnode._nextnode = null;
            oldnode._parentnode = null;
        }

        internal void Replace(int index, HtmlNode node) {
            HtmlNode next = null;
            HtmlNode prev = null;
            HtmlNode oldnode = (HtmlNode)_items[index];

            if (index > 0) {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < (_items.Count - 1)) {
                next = (HtmlNode)_items[index + 1];
            }

            _items[index] = node;

            if (prev != null) {
                if (node == prev) {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = node;
            }

            if (next != null) {
                next._prevnode = node;
            }

            node._prevnode = prev;
            if (next == node) {
                throw new InvalidProgramException("Unexpected error.");
            }
            node._nextnode = next;
            node._parentnode = _parentnode;

            oldnode._prevnode = null;
            oldnode._nextnode = null;
            oldnode._parentnode = null;
        }

        internal void Insert(int index, HtmlNode node) {
            HtmlNode next = null;
            HtmlNode prev = null;

            if (index > 0) {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < _items.Count) {
                next = (HtmlNode)_items[index];
            }

            _items.Insert(index, node);

            if (prev != null) {
                if (node == prev) {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = node;
            }

            if (next != null) {
                next._prevnode = node;
            }

            node._prevnode = prev;

            if (next == node) {
                throw new InvalidProgramException("Unexpected error.");
            }

            node._nextnode = next;
            node._parentnode = _parentnode;
        }

        internal void Append(HtmlNode node) {
            HtmlNode last = null;
            if (_items.Count > 0) {
                last = (HtmlNode)_items[_items.Count - 1];
            }

            _items.Add(node);
            node._prevnode = last;
            node._nextnode = null;
            node._parentnode = _parentnode;
            if (last != null) {
                if (last == node) {
                    throw new InvalidProgramException("Unexpected error.");
                }
                last._nextnode = node;
            }
        }

        internal void Prepend(HtmlNode node) {
            HtmlNode first = null;
            if (_items.Count > 0) {
                first = (HtmlNode)_items[0];
            }

            _items.Insert(0, node);

            if (node == first) {
                throw new InvalidProgramException("Unexpected error.");
            }
            node._nextnode = first;
            node._prevnode = null;
            node._parentnode = _parentnode;
            if (first != null) {
                first._prevnode = node;
            }
        }

        internal void Add(HtmlNode node) {
            _items.Add(node);
        }

        public HtmlNode this[int index] {
            get {
                return _items[index] as HtmlNode;
            }
        }

        internal int GetNodeIndex(HtmlNode node) {
            // TODO: should we rewrite this? what would be the key of a node?
            for (int i = 0; i < _items.Count; i++) {
                if (node == ((HtmlNode)_items[i])) {
                    return i;
                }
            }
            return -1;
        }

        public int this[HtmlNode node] {
            get {
                int index = GetNodeIndex(node);
                if (index == -1) {
                    throw new ArgumentOutOfRangeException("node", "Node \"" + node.CloneNode(false).OuterHtml + "\" was not found in the collection");
                }
                return index;
            }
        }

        public HtmlNodeEnumerator GetEnumerator() {
            return new HtmlNodeEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public class HtmlNodeEnumerator : IEnumerator {
            int _index;
            ArrayList _items;

            internal HtmlNodeEnumerator(ArrayList items) {
                _items = items;
                _index = -1;
            }

            public void Reset() {
                _index = -1;
            }

            public bool MoveNext() {
                _index++;
                return (_index < _items.Count);
            }

            public HtmlNode Current {
                get {
                    return (HtmlNode)(_items[_index]);
                }
            }

            object IEnumerator.Current {
                get {
                    return (Current);
                }
            }
        }
    }

    public enum HtmlNodeType {
        Document,
        Element,
        Comment,
        Text
    }

    public class HtmlParseError {
        private HtmlParseErrorCode _code;
        private int _line;
        private int _linePosition;
        private int _streamPosition;
        private string _sourceText;
        private string _reason;

        internal HtmlParseError(
            HtmlParseErrorCode code,
            int line,
            int linePosition,
            int streamPosition,
            string sourceText,
            string reason) {
            _code = code;
            _line = line;
            _linePosition = linePosition;
            _streamPosition = streamPosition;
            _sourceText = sourceText;
            _reason = reason;
        }

        public HtmlParseErrorCode Code {
            get {
                return _code;
            }
        }

        public int Line {
            get {
                return _line;
            }
        }

        public int LinePosition {
            get {
                return _linePosition;
            }
        }

        public int StreamPosition {
            get {
                return _streamPosition;
            }
        }

        public string SourceText {
            get {
                return _sourceText;
            }
        }

        public string Reason {
            get {
                return _reason;
            }
        }
    }

    public enum HtmlParseErrorCode {
        TagNotClosed,
        TagNotOpened,
        CharsetMismatch,
        EndTagNotRequired,
        EndTagInvalidHere
    }

    public class HtmlTextNode : HtmlNode {
        private string _text;

        internal HtmlTextNode(HtmlDocument ownerdocument, int index)
            :
            base(HtmlNodeType.Text, ownerdocument, index) {
        }

        public override string InnerHtml {
            get {
                return OuterHtml;
            }
            set {
                _text = value;
            }
        }

        public override string OuterHtml {
            get {
                if (_text == null) {
                    return base.OuterHtml;
                }
                return _text;
            }
        }

        public string Text {
            get {
                if (_text == null) {
                    return base.OuterHtml;
                }
                return _text;
            }
            set {
                _text = value;
            }
        }
    }

}
