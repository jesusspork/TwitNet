using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace TwitNet_Builder
{
    sealed class GZip
    {
        /// <summary>
        /// GZip compress a byte array
        /// </summary>
        /// <param name="data">byte array to compress</param>
        public static byte[] CompressData(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            GZipStream GZipStream = new GZipStream(output, CompressionMode.Compress, true);
            GZipStream.Write(data, 0, data.Length);
            GZipStream.Close();
            return output.ToArray();
        }
    }
}
