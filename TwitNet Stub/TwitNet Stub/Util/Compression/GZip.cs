using System;
using System.IO;
using System.IO.Compression;

namespace TwitNetStub.Util.Compression
{
    public static class GZip
    {
        public static byte[] DecompressData(byte[] data)
        {
            MemoryStream input = new MemoryStream();
            input.Write(data, 0, data.Length);
            input.Position = 0;
            MemoryStream output;
            using (GZipStream gZipStream = new GZipStream(input, CompressionMode.Decompress, true))
            {
                output = new MemoryStream();
                byte[] buff = new byte[64];
                int read = -1;

                read = gZipStream.Read(buff, 0, buff.Length);
                while (read > 0)
                {
                    output.Write(buff, 0, read);
                    read = gZipStream.Read(buff, 0, buff.Length);
                }
                gZipStream.Close();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return output.ToArray();
        }

        public static byte[] CompressData(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (GZipStream gZipStream = new GZipStream(output, CompressionMode.Compress, true))
            {
                gZipStream.Write(data, 0, data.Length);
                gZipStream.Close();
            }
            return output.ToArray();
        }
    }
}
