using System.IO;
using System.IO.Compression;
//using System.Linq;

namespace TwitNetBuilder.Util.Compression
{
    public static class GZip
    {
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
