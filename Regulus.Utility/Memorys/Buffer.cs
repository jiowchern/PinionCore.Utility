using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Memorys
{
    
    public class Buf 
    {
        public readonly ArraySegment<byte> Bytes;
        public Buf(ArraySegment<byte> bytes)
        {
            System.Memory<byte> memory = new System.Memory<byte>(bytes.Array, bytes.Offset, bytes.Count);
            memory.Pin();
            Bytes = bytes;
        }

    }
    public interface Buffer : IDisposable ,IEnumerable<byte> , IReadOnlyCollection<byte> 
    {
        // chank size 
        int Capacity { get; }
        byte this[int index] { get; set; }

        ArraySegment<byte> Bytes { get; }
        
        
    }

    
}
