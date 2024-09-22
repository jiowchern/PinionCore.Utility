using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Memorys
{
    
    public interface Buffer : IDisposable ,IEnumerable<byte> , IReadOnlyCollection<byte>
    {
        // chank size 
        int Capacity { get; }
        byte this[int index] { get; set; }

        ArraySegment<byte> Bytes { get; }
    }
}
