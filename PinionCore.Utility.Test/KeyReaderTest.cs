using NUnit.Framework;

namespace PinionCoreLibraryTest
{
    public class KeyReaderTest
    {


        [NUnit.Framework.Test]
        public void TestSignle()
        {
            string message = "";
            PinionCore.Utility.KeyReader reader = new PinionCore.Utility.KeyReader('\r');
            reader.DoneEvent += (chars) =>
            {
                message = new string(chars);
            };
            reader.Push('a');
            reader.Push('b');
            reader.Push('\r');

            Assert.AreEqual("ab", message);
        }

        [NUnit.Framework.Test]
        public void TestMuti()
        {
            string message = "";
            PinionCore.Utility.KeyReader reader = new PinionCore.Utility.KeyReader('\r');
            reader.DoneEvent += (chars) =>
            {
                message = new string(chars);
            };
            reader.Push(new char[] { 'a', 'b', '\r' });



            Assert.AreEqual("ab", message);
        }

    }
}
