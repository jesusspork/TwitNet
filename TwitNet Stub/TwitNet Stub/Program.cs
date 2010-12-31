using System;
using System.Windows.Forms;
using System.Threading;

namespace TwitNetStub
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            new Thread(new ThreadStart(TwitNetStub.Main.Instance.Start)).Start();
        }
    }
}
