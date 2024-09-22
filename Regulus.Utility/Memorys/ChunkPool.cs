using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Regulus.Memorys
{
    internal class ChunkPool
    {
        private readonly int _bufferSize;
        private readonly int _buffersPerPage;

        private readonly ConcurrentBag<PooledBuffer> _availableBuffers;
        private readonly List<byte[]> _pages;
        private readonly object _pageLock = new object();

        public ChunkPool(int bufferSize, int buffersPerPage)
        {
            _bufferSize = bufferSize;
            _buffersPerPage = buffersPerPage;
            _availableBuffers = new ConcurrentBag<PooledBuffer>();
            _pages = new List<byte[]>();
        }

        public Buffer Alloc(int count)
        {
            lock (_pageLock)
            {
                if (_availableBuffers.IsEmpty)
                {
                    AllocatePage();
                }
            }
                
            if (!_availableBuffers.TryTake(out PooledBuffer buffer))
            {
                throw new InvalidOperationException("Failed to allocate buffer after allocating a new page.");                
            }
            buffer.Reset(count);
            return buffer;
        }

        private void AllocatePage()
        {
            int pageSize = _bufferSize * _buffersPerPage;
            byte[] page = new byte[pageSize];
            _pages.Add(page);

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
    }
}
