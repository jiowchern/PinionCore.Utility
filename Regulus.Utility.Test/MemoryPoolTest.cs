using System;
using System.Linq;

namespace Regulus.Utility.Tests
{
    public class MemoryPoolTest
    {
        
        [NUnit.Framework.Test]
        public void Test16()
        {           
            var pool = new Regulus.Memorys.Pool(new[] { new Regulus.Memorys.ChunkSetting(4,1000) , new Regulus.Memorys.ChunkSetting(8, 1000), new Regulus.Memorys.ChunkSetting(16, 1000) } );
            var buffer = pool.Alloc(12);
            
            
            
            NUnit.Framework.Assert.AreEqual(16 , buffer.Capacity); 
            NUnit.Framework.Assert.AreEqual(12, buffer.Count); 
            
        }

        /*[NUnit.Framework.Test]
        public void Test16Dispose()
        {
            var pool = new Regulus.Memorys.Pool(new[] { new Regulus.Memorys.ChunkSetting(4, 1000), new Regulus.Memorys.ChunkSetting(8, 1000), new Regulus.Memorys.ChunkSetting(16, 1000) });
            var buffer = pool.Alloc(12);
            NUnit.Framework.Assert.AreEqual(16, buffer.Capacity);
            NUnit.Framework.Assert.AreEqual(12, buffer.Count);
            var chunk = pool.Chunks.Single( c=>c.BufferSize == buffer.Capacity);

            var availableCount1 = chunk.AvailableCount;

            buffer.Dispose();
           

            var availableCount2 = chunk.AvailableCount;
            var result = availableCount2 - availableCount1;
            NUnit.Framework.Assert.AreEqual(1, result);
        }*/

        [NUnit.Framework.Test]
        public void Test123()
        {
            var pool = new Regulus.Memorys.Pool(new[] { new Regulus.Memorys.ChunkSetting(4, 1000), new Regulus.Memorys.ChunkSetting(8, 1000), new Regulus.Memorys.ChunkSetting(16, 1000) });
            var buffer = pool.Alloc(123);
            
            
            NUnit.Framework.Assert.AreEqual(123, buffer.Capacity);
            
        }
    }
}
