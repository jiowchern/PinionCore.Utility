using System;
using System.Collections;
using System.Collections.Generic;

namespace PinionCore.Memorys
{
    
    public static class BufferExtensions
    {
        public static Buffer AsBuffer(this byte[] buffer)
        {
            return new DirectBuffer(buffer);
        }

        public static Buffer AsBuffer(this ArraySegment<byte> buffer)
        {
            return new DirectBuffer(buffer);
        }        
    }
    public interface Buffer : IEnumerable<byte> , IReadOnlyCollection<byte> 
    {
        // chank size 
        int Capacity { get; }
        byte this[int index] { get; set; }

        ArraySegment<byte> Bytes { get; }
        
        
    }

    
}
