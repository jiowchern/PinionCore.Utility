using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Regulus.Memorys
{
    internal class ChunkPool : Chankable
    {
        private readonly int _bufferSize;
        private readonly int _buffersPerPage;

        private readonly ConcurrentBag<PooledBuffer> _availableBuffers;
        private readonly List<byte[]> _pages;
        private readonly object _pageLock = new object();
        int _DefaultAllocationThreshold;
       
        int Chankable.BufferSize => _bufferSize;

        int Chankable.AvailableCount => _availableBuffers.Count;

        int Chankable.DefaultAllocationThreshold => _DefaultAllocationThreshold;

        int Chankable.PageSize => _pages.Count;

        public ChunkPool(int bufferSize, int buffersPerPage)
        {
            _bufferSize = bufferSize;
            _buffersPerPage = buffersPerPage;
            _availableBuffers = new ConcurrentBag<PooledBuffer>();
            _pages = new List<byte[]>();
        }

        public Buffer Alloc(int count)
        {            
            GeneratePage();
                
            if (!_availableBuffers.TryTake(out PooledBuffer buffer))
            {                
                System.Threading.Interlocked.Increment(ref _DefaultAllocationThreshold);                                    
                return new DirectBuffer(count);
            }
            buffer.Reset(count);
            return buffer;
        }

        private void GeneratePage()
        {
            byte[] page = null;
            lock (_pageLock)
            {
                if (_availableBuffers.Count > _DefaultAllocationThreshold)
                    return;
                int pageSize = _bufferSize * _buffersPerPage;
                page = new byte[pageSize];
                _pages.Add(page);                
            }

            for (int i = 0; i < _buffersPerPage; i++)
            {
                int offset = i * _bufferSize;
                var segment = new ArraySegment<byte>(page, offset, _bufferSize);
                var buffer = new PooledBuffer(this, segment);
                _availableBuffers.Add(buffer);
                
            }

        }

        public void Return(PooledBuffer buffer)
        {
            lock (_pageLock)
            {
                if (!_pages.Any(p => p == buffer.Page))
                    return;
            }
            _availableBuffers.Add(buffer);
        }

        internal void Return(ArraySegment<byte> bytes)
        {
            lock (_pageLock)
            {
                if (!_pages.Any(p => p == bytes.Array))
                    return;
            }

            var buffer = new PooledBuffer(this, bytes);
            _availableBuffers.Add(buffer);
        }
    }
}
