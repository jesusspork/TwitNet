using TwitNetStub.Commands;

namespace TwitNetStub.Operations
{
    class SwitchMasterOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;

        public SwitchMasterOP(string url)
        {
            MainURL = url;
        }

        public void Initialize()
        {
            
        }
        public void Run()
        {
            new SwitchMaster().SwitchEm(MainURL);
        }
    }
}
