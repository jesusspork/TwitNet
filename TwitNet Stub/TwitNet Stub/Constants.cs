using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TwitNetStub
{
    public static class Constants
    {
        public static string DefaultEncryptionKey = "ZER0COOL";
        public static string CustomEncryptionKey;
        public static string CommandSplitter = " at ";
        public static string Splitter = "|split|";
        public static string FullExecutablePath = Application.ExecutablePath;
        public static string FileName = Path.GetFileName(Application.ExecutablePath);
        public static string ExecutablePath = Application.StartupPath;
        public static string MutexID = "pmyawmepviobryj";
    }
}
