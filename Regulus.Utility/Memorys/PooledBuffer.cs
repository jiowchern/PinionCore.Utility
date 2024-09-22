using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Memorys
{
    internal class PooledBuffer : Buffer
    {
        private readonly ChunkPool _chunkPool;
        private readonly ArraySegment<byte> _array;
        private bool _disposed;
        private int _count; 
        private readonly object _syncRoot = new object();

        internal PooledBuffer(ChunkPool chunkPool, ArraySegment<byte> segment )
        {
            _chunkPool = chunkPool;
            _array = segment;
            _disposed = false;
            _count = 0;
        }



        public int Capacity => _array.Count;

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _count;
                }
            }
        }

        public byte this[int index]
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                lock (_syncRoot)
                {
                    return _array.Array[_array.Offset + index];
                }
            }
            set
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                lock (_syncRoot)
                {
                    _array.Array[_array.Offset + index] = value;
                }
            }
        }

      

        public IEnumerator<byte> GetEnumerator()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(PooledBuffer));

            for (int i = 0; i < Count; i++)
            {
                yield return _array.Array[_array.Offset + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_syncRoot)
                {
                    _disposed = true;
                    _count = 0;
                    _chunkPool.Return(this);
                }
            }
        }

        internal void SetCount(int count)
        {
            lock (_syncRoot)
                _count = count;
        }
    }
}
