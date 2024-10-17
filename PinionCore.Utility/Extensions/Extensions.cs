using System.Linq;
using System.Security.Cryptography;

namespace PinionCore.Extensions
{
    public static class Extensions
    {
        public static byte[] ToMd5(this string source)
        {
            MD5 md5 = MD5.Create();
            return md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(source));
        }

        public static string ToMd5String(this System.Collections.Generic.IEnumerable<byte> source)
        {
            return System.BitConverter.ToString(source.ToArray());
        }
    }
}