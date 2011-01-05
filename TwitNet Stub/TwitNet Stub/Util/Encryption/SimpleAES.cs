using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
//using System.Collections.Generic;

//using System.Windows.Forms;

namespace TwitNetStub.Util.Encryption
{

    /// <summary>
    /// AES Class
    /// </summary>
    sealed class SimpleAES
    {
        private ICryptoTransform EncryptorTransform, DecryptorTransform;
        private System.Text.UTF8Encoding UTFEncoder;
        public byte[] Key;

        /// <summary>
        /// Return a string of random chars
        /// </summary>
        /// <param name="length">string length</param>
        string getRandNum(int length)
        {
            string str = "0123456789!@#$%^&*()";
            string str2 = "abcdefghijklmnopqrstuvwxyz";
            string str3;
            str3 = (str + str2);
            Random random = new Random();
            StringBuilder builder = new StringBuilder(length);
            int i;
            for (i = 0; i <= length - 1; i++)
            {
                int num2 = random.Next(str3.Length);
                builder.Append(str3[num2]);
            }
            return Convert.ToString(builder);
        }

        //Modified http://stackoverflow.com/questions/165808/simple-2-way-encryption-for-c
        /// <summary>
        /// AES Class
        /// </summary>
        public SimpleAES(bool defaultkey)
        {
            PasswordDeriveBytes derived = new PasswordDeriveBytes(
                Encoding.Default.GetBytes(new Random().Next(5000, 10000).ToString()),
                Encoding.Default.GetBytes(new Random().Next(5000, 10000).ToString()));
            //This is our encryption method
            RijndaelManaged rm = new RijndaelManaged();

            //Encryption key

                Key = Encoding.Default.GetBytes(Constants.DefaultEncryptionKey);

            //Key = Encoding.UTF8.GetBytes(Strings.Split(Config.Settings[8], Config.FSplit3, -1, CompareMethod.Text)[0]);//Encoding.UTF8.GetBytes(getRandNum(32).ToString());

            byte[] Vector = Encoding.Default.GetBytes("Ijd0!$FDdg8s(*&J");
            //Create an encryptor and a decryptor using our encryption method, key, and vector.
            EncryptorTransform = rm.CreateEncryptor(Key, Vector);
            DecryptorTransform = rm.CreateDecryptor(Key, Vector);

            //Used to translate bytes to text and vice versa
            UTFEncoder = new System.Text.UTF8Encoding();
        }


        /// Encrypt some text and return a string suitable for passing in a URL.
        public string EncryptToString(string TextValue)
        {
            return Encoding.Default.GetString(Encrypt(TextValue));
        }

        /// Encrypt some text and return an encrypted byte array.
        public byte[] Encrypt(string TextValue)
        {
            //Translates our text value into a byte array.
            Byte[] bytes = UTFEncoder.GetBytes(TextValue);

            //Used to stream the data in and out of the CryptoStream.
            MemoryStream memoryStream = new MemoryStream();

            /*
         * We will have to write the unencrypted bytes to the stream,
         * then read the encrypted result back from the stream.
         */
            #region Write the decrypted value to the encryption stream
            CryptoStream cs = new CryptoStream(memoryStream, EncryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();
            #endregion

            #region Read encrypted value back out of the stream
            memoryStream.Position = 0;
            byte[] encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);
            #endregion

            //Clean up.
            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        /// The other side: Decryption methods
        public string DecryptString(string EncryptedString)
        {
            return Decrypt(StrToByteArray(EncryptedString));
        }

        /// Decryption when working with byte arrays.    
        public string Decrypt(byte[] EncryptedValue)
        {
            #region Write the encrypted value to the decryption stream
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream decryptStream = new CryptoStream(encryptedStream, DecryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(EncryptedValue, 0, EncryptedValue.Length);
            decryptStream.FlushFinalBlock();
            #endregion

            #region Read the decrypted value from the stream.
            encryptedStream.Position = 0;
            Byte[] decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();
            #endregion
            return UTFEncoder.GetString(decryptedBytes);
        }

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            byte val;
            byte[] byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }
    }
}
