using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.LogAgent;
using System.IO;
using System.Xml;

namespace K12.LogView.Modules
{
    public partial class TextBoxForm : BaseForm
    {
        private const string TimeDisplayFormat = "yyyy/MM/dd HH:mm:ss";

        public TextBoxForm(Obj tx)
        {
            InitializeComponent();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("========詳細內容========");
            sb.AppendLine(tx.description);
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("======其他參考資訊======");
            sb.AppendLine("記錄編號：" + tx.id);
            sb.AppendLine("日期時間：" + DateTime.Parse(tx.server_time).ToString(TimeDisplayFormat));
            sb.AppendLine("登入帳號：" + tx.actor);
            sb.AppendLine("電腦名稱：" + tx.client_info.HostName);
            sb.AppendLine("功能：" + tx.action_by);
            sb.AppendLine("動作：" + tx.action);
            sb.AppendLine("對象分類：" + tx.target_category);
            sb.AppendLine("對象系統編號：" + tx.target_id);
            //sb.AppendLine("動作類型：" + tx.action_type);
            //sb.AppendLine("");
            //sb.AppendLine("「ClientInfo - XML內容」");
            //sb.AppendLine(Format(tx.ClientInfo.OutputResult().OuterXml));

            textBoxX1.Text = sb.ToString().Replace("\n", "\r\n");
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string Format(string xmlContent)
        {
            MemoryStream ms = new MemoryStream();

            XmlTextWriter writer = new XmlTextWriter(ms, System.Text.Encoding.UTF8);

            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.IndentChar = ' ';

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

        private XmlReader GetXmlReader(string XmlData)
        {
            XmlReaderSettings setting = new XmlReaderSettings();
            setting.IgnoreWhitespace = true;

            XmlReader Reader = XmlReader.Create(new StringReader(XmlData), setting);

            return Reader;
        }
    }
}
