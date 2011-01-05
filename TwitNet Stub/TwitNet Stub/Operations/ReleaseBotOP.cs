namespace TwitNetStub.Operations
{
    class ReleaseBotOP : IBotOperation
    {
        public bool Finished { get; set; }

        public ReleaseBotOP()
        {
            
        }

        public void Initialize()
        {

        }
        public void Run()
        {
            new Commands.Release().ReleaseBot();
        }
    }
}