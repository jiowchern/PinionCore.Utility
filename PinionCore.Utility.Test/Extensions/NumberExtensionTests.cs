using System.Linq;

namespace PinionCore.Extensions.Tests
{
    public class NumberExtensionTests
    {

        [NUnit.Framework.Test]
        public void Count()
        {
            int count = 5;            
            var numbers = count.GetSeries().ToArray();
            NUnit.Framework.Assert.AreEqual(4, numbers[4]);
            
        }
    }
}