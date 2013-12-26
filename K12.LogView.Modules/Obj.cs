using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;

namespace K12.LogView.Modules
{
    public class Obj
    {
        public string id { get; set; }
        public string actor { get; set; }
        public string action_type { get; set; }
        public string action { get; set; }
        public string target_category { get; set; }
        public string target_id { get; set; }
        public string server_time { get; set; }
        public Info client_info { get; set; }
        public string action_by { get; set; }
        public string description { get; set; }

        public Obj(DataRow row)
        {
            id = "" + row["id"];
            actor = "" + row["actor"];
            action_type = "" + row["action_type"];
            action = "" + row["action"];
            target_category = "" + row["target_category"];
            target_id = "" + row["target_id"];
            server_time = "" + row["server_time"];
            if (!string.IsNullOrEmpty("" + row["client_info"]))
            {
                client_info = new Info(XmlHelper.LoadXml("" + row["client_info"]));
            }
            action_by = "" + row["action_by"];
            description = "" + row["description"];
        }
    }

    public class Info
    {
        public Info(XmlElement xml)
        {
            if (xml.SelectSingleNode("HostName") != null)
            {
                HostName = xml.SelectSingleNode("HostName").InnerText;
                foreach (XmlNode node in xml.SelectNodes("NetworkAdapterList/NetworkAdapter/IPAddress"))
                {
                    IPAddress1 += node.InnerText + "　";
                }
            }
        }
        public string HostName = "";
        public string IPAddress1 = "";
    }
}
