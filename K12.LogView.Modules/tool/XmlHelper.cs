using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;
using System.Runtime.CompilerServices;


namespace K12.LogView.Modules
{
    public class XmlHelper
    {
        /// <summary>
        /// 代表基礎的 Xml 資料。
        /// </summary>
        protected XmlElement BaseNode = null;

        /// <summary>
        /// 建立一個空的文件，預設會有「根」名稱「Content」。
        /// </summary>
        public XmlHelper()
        {
            BaseNode = XmlHelper.LoadXml("<Content/>");
        }

        /// <summary>
        /// 依指定的「根」元素名稱建立Document
        /// </summary>
        /// <param name="rootName">根元素的名稱，不可加任何的特殊符號</param>
        public XmlHelper(string rootName)
            : this()
        {
            BaseNode = XmlHelper.LoadXml("<" + rootName + "/>");
        }

        /// <summary>
        /// 依XmlElement的內容建立物件。
        /// </summary>
        /// <param name="xmldata">要依據的XmlElement物件。</param>
        public XmlHelper(XmlElement xmldata)
            : this()
        {
            if (xmldata.NodeType != XmlNodeType.Element)
                throw new ArgumentException("必需是 XmlElement 的型態。", "xmldata");

            BaseNode = xmldata;
        }

        /// <summary>
        /// 取得Xml的完整 Xml 字串。
        /// </summary>
        /// <returns>完整 Xml 字串。</returns>
        public string XmlString { get { return BaseNode.OuterXml; } }

        /// <summary>
        /// 取得目前文件的基礎XmlElement物件。
        /// </summary>
        /// <returns>此物件的基本XmlElement物件。</returns>
        public XmlElement BaseElement { get { return BaseNode; } }

        /// <summary>
        /// 文件根名稱。
        /// </summary>
        public string RootName { get { if (BaseNode == null) return ""; return BaseNode.LocalName; } }

        #region Get Methods

        /// <summary>
        /// 取得上一次執行 GetInteger、TryGetInteger 時的回傳值。
        /// </summary>
        public int LastInteger { get; private set; }

        /// <summary>
        /// 取得 Integer 資料。
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetInteger(string xpath, int defaultValue)
        {
            if (TryGetInteger(xpath))
                return LastInteger;
            else
            {
                LastInteger = defaultValue;
                return LastInteger;
            }
        }

        /// <summary>
        /// 嘗試將讀取的資料轉換為 Integer，並回傳作業是否成功。
        /// </summary>
        /// <param name="xpath">要讀取的資料路徑。</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryGetInteger(string xpath)
        {
            XmlNode n = BaseNode.SelectSingleNode(xpath);

            if (n == null) return false;

            string strValue;
            if (n is XmlAttribute)
                strValue = n.Value;
            else
                strValue = n.InnerText;

            int intValue;
            if (int.TryParse(strValue, out intValue))
            {
                LastInteger = intValue;
                return true;
            }
            else
                return false;
        }

        public decimal LastDecimal { get; private set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public decimal GetDecimal(string xpath, decimal defaultValue)
        {
            if (TryGetDecimal(xpath))
                return LastDecimal;
            else
            {
                LastDecimal = defaultValue;
                return LastDecimal;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryGetDecimal(string xpath)
        {
            XmlNode n = BaseNode.SelectSingleNode(xpath);

            if (n == null) return false;

            string strValue;
            if (n is XmlAttribute)
                strValue = n.Value;
            else
                strValue = n.InnerText;

            decimal decValue;
            if (decimal.TryParse(strValue, out decValue))
            {
                LastDecimal = decValue;
                return true;
            }
            else
                return false;
        }

        public bool LastBoolean { get; private set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool GetBoolean(string xpath, bool defaultValue)
        {
            if (TryGetBoolean(xpath))
                return LastBoolean;
            else
            {
                LastBoolean = defaultValue;
                return LastBoolean;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool TryGetBoolean(string xpath)
        {
            XmlNode n = BaseNode.SelectSingleNode(xpath);

            if (n == null) return false;

            string strValue;
            if (n is XmlAttribute)
                strValue = n.Value;
            else
                strValue = n.InnerText;

            bool boolValue;
            if (bool.TryParse(strValue, out boolValue))
            {
                LastBoolean = boolValue;
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 取得日期資料，格式「yyyy/MM/dd」。不存在或來源資料不正確會回傳空字串。
        /// </summary>
        public string GetDateString(string xpath)
        {
            DateTime dt;

            if (DateTime.TryParse(GetString(xpath), out dt))
                return dt.ToString("yyyy/MM/dd");
            else
                return string.Empty;
        }

        public string LastString { get; private set; }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string GetString(string xpath)
        {
            XmlNode n = BaseNode.SelectSingleNode(xpath);
            string strValue;

            if (n == null) return string.Empty;

            if (n is XmlAttribute)
                strValue = n.Value;
            else
                strValue = n.InnerText;

            LastString = strValue;
            return strValue + "";
        }

        /// <summary>
        /// 取得元素物件，但僅取得符合「元素路徑」的第一個元素。
        /// </summary>
        /// <param name="xpath">元素路徑。</param>
        /// <returns>回傳的XmlElement實體。</returns>
        /// <exception cref="Exception">發生再xpath取出的物件不是元素(Element)時。</exception>
        public XmlElement GetElement(string xpath)
        {
            XmlNode nd = BaseNode.SelectSingleNode(xpath);

            if (nd != null && !(nd is XmlElement))
                throw new Exception("取得的資料不是一個元素(Element)。");

            //如果nd是Null，則會回傳Null(表示as運算失敗)
            return (nd as XmlElement);
        }

        /// <summary>
        /// 取得元素物件陣列，將會取得所有符合「元素路徑」的所有元素。
        /// </summary>
        /// <param name="xpath">元素路徑。</param>
        /// <returns>XmlElement的陣列。</returns>
        public XmlElement[] GetElements(string xpath)
        {
            XmlNodeList ndl = BaseNode.SelectNodes(xpath);

            XmlElement[] result = new XmlElement[ndl.Count];
            for (int i = 0; i < ndl.Count; i++)
                result[i] = (XmlElement)ndl[i];
            return result;
        }

        /// <summary>
        /// 回傳完整的Xml字串。
        /// </summary>
        /// <returns>完整的Xml字串。</returns>
        public override string ToString()
        {
            return BaseElement.OuterXml;
        }

        #endregion

        /// <summary>
        /// 將內部Xml資料以UTF-8的編碼方式儲存到檔案中，如果檔案已存在，會覆寫該檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        public void Save(string fileName)
        {
            Save(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// 將內部Xml資料儲存到檔案中，如果檔案已存在，會覆寫該檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        /// <param name="enc">檔案編碼方式</param>
        public void Save(string fileName, Encoding enc)
        {
            File.WriteAllText(fileName, BaseNode.OuterXml, enc);
        }

        #region Static Methods
        /// <summary>
        /// 格式化 Xml 內容。
        /// </summary>
        /// <returns></returns>
        public static string Format(string xmlContent)
        {
            MemoryStream ms = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = '\t';

            XmlReader Reader = GetXmlReader(xmlContent);
            writer.WriteNode(Reader, true);
            writer.Flush();
            Reader.Close();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms, System.Text.Encoding.UTF8);

            string Result = sr.ReadToEnd();
            sr.Close();

            writer.Close();
            ms.Close();

            return Result;
        }

        private static XmlReader GetXmlReader(string XmlData)
        {
            XmlReaderSettings setting = new XmlReaderSettings();
            setting.IgnoreWhitespace = true;

            XmlReader Reader = XmlReader.Create(new StringReader(XmlData), setting);

            return Reader;
        }

        /// <summary>
        /// 複製 XmlElement 物件，變更其內容不會反應到原來的XmlElement中。
        /// </summary>
        /// <param name="srcElement">要複製的XmlElement物件。</param>
        /// <returns>已複製的XmlElement物件。</returns>
        public static XmlElement Clone(XmlElement srcElement)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.PreserveWhitespace = true;
            xmldoc.LoadXml(srcElement.OuterXml);

            return (XmlElement)xmldoc.DocumentElement;
        }


        /// <summary>
        /// 載入指定的 Xml 檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        /// <returns><see cref="XmlElement"/>物件。</returns>
        public static XmlElement LoadFrom(string fileName)
        {
            return LoadFrom(fileName, true);
        }

        /// <summary>
        /// 載入指定的 Xml 檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        /// <returns><see cref="XmlElement"/>物件。</returns>
        public static XmlElement LoadFrom(string fileName, bool preserveWhitespace)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.PreserveWhitespace = preserveWhitespace;
            xmldoc.Load(fileName);

            return xmldoc.DocumentElement;
        }

        /// <summary>
        /// 載入指定的 Xml 資料。
        /// </summary>
        /// <param name="xmlContent">要載入的 Xml 字串資料。</param>
        /// <returns><see cref="XmlElement"/>物件。</returns>
        public static XmlElement LoadXml(string xmlString)
        {
            return LoadXml(xmlString, true);
        }

        /// <summary>
        /// 載入指定的 Xml 資料。
        /// </summary>
        /// <param name="xmlString">要載入的 Xml 字串資料。</param>
        /// <param name="preserveWhitespace">是否保留字串中的泛空白字元。</param>
        /// <returns><see cref="XmlElement"/>物件。</returns>
        public static XmlElement LoadXml(string xmlString, bool preserveWhitespace)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.PreserveWhitespace = preserveWhitespace;
            xmldoc.LoadXml(xmlString);

            return xmldoc.DocumentElement;
        }

        /// <summary>
        /// 將指定的 Xml 資料以 UTF-8 的編碼方式儲存到檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        /// <param name="elm">要儲存的 Xml 物件。</param>
        public static void SaveTo(string fileName, XmlNode node)
        {
            SaveTo(fileName, node, Encoding.UTF8);
        }

        /// <summary>
        /// 將指定的 Xml 資料儲存到檔案。
        /// </summary>
        /// <param name="fileName">檔案名稱。</param>
        /// <param name="node">要儲存的 Xml 物件。</param>
        /// <param name="enc">儲存的編碼方式。</param>
        public static void SaveTo(string fileName, XmlNode node, Encoding enc)
        {
            File.WriteAllText(fileName, node.OuterXml, enc);
        }

        /// <summary>
        /// 將指定的 Xml 資料以UTF-8的編碼方式寫入到串流中。
        /// </summary>
        /// <param name="outStream">指定的串流。</param>
        /// <param name="node">要輸出的 Xml 物件。</param>
        public static void SaveTo(Stream outStream, XmlNode node)
        {
            SaveTo(outStream, node, Encoding.UTF8);
        }

        /// <summary>
        /// 將指定的 Xml 資料寫入到串流中。
        /// </summary>
        /// <param name="outStream">指定的串流。</param>
        /// <param name="node">要輸出的 Xml 物件。</param>
        /// <param name="enc">輸出的編碼方式。</param>
        public static void SaveTo(Stream outStream, XmlNode node, Encoding enc)
        {
            StreamWriter sw = new StreamWriter(outStream, enc);
            sw.Write(node.OuterXml);
        }

        /// <summary>
        /// 傳送Xml內容到某個網址。
        /// </summary>
        /// <param name="url">目的URL。</param>
        /// <param name="xmlContent">要傳送的Xml內容。</param>
        /// <returns>回傳的Xml資料。</returns>
        public static string HttpSendTo(string url, string xmlContent)
        {
            return HttpSendTo(url, "POST", xmlContent);
        }

        /// <summary>
        /// 傳送Xml內容到某個網址。
        /// </summary>
        /// <param name="url">目的URL。</param>
        /// <param name="method">傳送的方法(POST、GET)</param>
        /// <param name="xmlContent">要傳送的Xml內容。</param>
        /// <returns>回傳的Xml資料。</returns>
        public static string HttpSendTo(string url, string method, string xmlContent)
        {
            //建立Http連線
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(url);

            //基本設定
            httpReq.Method = method;
            httpReq.ContentType = "text/xml";

            if (method == "POST")
            {
                //寫入Request主體
                StreamWriter reqWriter = new StreamWriter(httpReq.GetRequestStream(), Encoding.UTF8);
                reqWriter.Write(xmlContent);
                reqWriter.Close();
            }
            else if (method == "GET")
            {
                httpReq = (HttpWebRequest)WebRequest.Create(url + "?" + xmlContent);
            }
            else
            {
                throw new InvalidOperationException("不支援指定的 Method，只允許「POST、GET」。");
            }

            //取得Response
            WebResponse httpRsp = httpReq.GetResponse();
            StreamReader rspStream = new StreamReader(httpRsp.GetResponseStream(), Encoding.UTF8);

            string result = rspStream.ReadToEnd();
            rspStream.Close(); //這個要記得關閉。

            return result;
        }
        #endregion
    }
}
