using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace TwitNet_Stub
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            new Thread(new ThreadStart(TwitNet_Stub.Main.Instance.Start)).Start();
        }
    }
}
