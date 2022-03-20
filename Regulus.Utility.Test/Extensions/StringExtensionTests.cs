using System.Security.Cryptography;

namespace Regulus.Extensions.Tests
{
    public class StringExtensionTests
    {
        [NUnit.Framework.Test]
        public void Md5()
        {
            var source = "1";
            MD5 md5 = MD5.Create();
            var code1 = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(source));
            var code2 = source.ToMd5();


            NUnit.Framework.Assert.True(new Regulus.Utility.Comparer<byte>(code1, code2, (c1, c2) => c1 == c2).Same);

            NUnit.Framework.Assert.AreEqual(code1.ToMd5String() , code2.ToMd5String());
            


        }
    }
}