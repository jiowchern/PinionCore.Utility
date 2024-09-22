namespace Regulus.Utility.Tests
{
    public class MemoryPoolTest
    {
        
        [NUnit.Framework.Test]
        public void Test16()
        {           
            var pool = new Regulus.Memorys.Pool(new[] { new Regulus.Memorys.ChunkSetting(4,1000) , new Regulus.Memorys.ChunkSetting(8, 1000), new Regulus.Memorys.ChunkSetting(16, 1000) } );
            var buffer = pool.Alloc(12);
            System.IDisposable disposable = buffer;
            
            
            NUnit.Framework.Assert.AreEqual(16 , buffer.Capacity); 
            NUnit.Framework.Assert.AreEqual(12, buffer.Count); 
            disposable.Dispose();
        }

        [NUnit.Framework.Test]
        public void Test123()
        {
            var pool = new Regulus.Memorys.Pool(new[] { new Regulus.Memorys.ChunkSetting(4, 1000), new Regulus.Memorys.ChunkSetting(8, 1000), new Regulus.Memorys.ChunkSetting(16, 1000) });
            var buffer = pool.Alloc(123);
            System.IDisposable disposable = buffer;
            
            NUnit.Framework.Assert.AreEqual(123, buffer.Capacity);
            disposable.Dispose();
        }
    }
}
