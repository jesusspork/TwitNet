
namespace TwitNetStub.Util.AntiCheck.Anti
{
    sealed class CheckProcess
    {

        public bool IsProcessRunning(string file_name)
        {
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.MainWindowTitle.Contains(file_name) || p.ProcessName.Contains(file_name))
                {
                    p.Dispose();
                    System.GC.Collect();
                    return true;
                }
            }
            return false;
        }
    }
}
