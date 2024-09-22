using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Memorys
{
    /// 
    /// 我將 Buffer 修改為如下
    /// 
    /// 再幫我修改相關代碼
    /// IReadOnlyCollection.Count 代表 Alloc 的長度
    /// 
    public interface Buffer : IDisposable ,IEnumerable<byte> , IReadOnlyCollection<byte>
    {
        // chank size 
        int Capacity { get; }

       
        
        // 可存取最大為 Length
        byte this[int index] { get; set; }
    }
}
