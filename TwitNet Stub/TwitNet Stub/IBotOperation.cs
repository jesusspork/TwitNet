using System;
using System.Collections.Generic;
using System.Text;

namespace TwitNetStub
{
    interface IBotOperation
    {
        //finished = true if you need to force the op to stop. errors etc.
        bool Finished { get; set; }
        void Initialize();
        void Run();
    }
}
