using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Regulus.Memorys
{
    internal class PooledBuffer : Buffer
    {
        private readonly ChunkPool _chunkPool;
        private readonly ArraySegment<byte> _array;
        ArraySegment<byte> _Buffer;
        private bool _disposed;
        public readonly object _Sync;

        public readonly byte[] Page;
        internal PooledBuffer(ChunkPool chunkPool, ArraySegment<byte> segment )
        {
            _Sync = new object();
            _chunkPool = chunkPool;
            _array = segment;
            _disposed = false;
            _SetCount(0);
            Page = segment.Array;
        }

        private void _SetCount(int count)
        {
            _Buffer = new ArraySegment<byte>(_array.Array, _array.Offset, count);   
        }

        ~PooledBuffer()
        {
            if (!_disposed)
            {
                _disposed = true;
                _chunkPool.Return(_array);
            }
        }

        public int Capacity => _array.Count;

        public int Count
        {
            get
            {
                return _Buffer.Count;
            }
        }

        ArraySegment<byte> Buffer.Bytes => _Create();

        private ArraySegment<byte> _Create()
        {
            lock (_Sync)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));
            }
                

            return _Buffer;
        }

        public byte this[int index]
        {
            get
            {
                lock (_Sync)
                {
                    if (_disposed)
                        throw new ObjectDisposedException(nameof(PooledBuffer));

                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    return _array.Array[_array.Offset + index];
                }
                
            }
            set
            {
                lock (_Sync)
                {
                    if (_disposed)
                        throw new ObjectDisposedException(nameof(PooledBuffer));

                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    _array.Array[_array.Offset + index] = value;
                }
                    
            }
        }

      

        public IEnumerator<byte> GetEnumerator()
        {
            lock (_Sync)
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                for (int i = 0; i < Count; i++)
                {
                    yield return _array.Array[_array.Offset + i];
                }
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            lock(_Sync)
            {
                if (!_disposed)
                {
                    // clean _array                
                   // _disposed = true;
                    //_SetCount(0);
                    //_chunkPool.Return(this);
                }
            }
            
        }

        internal void Reset(int count)
        {
            lock (_Sync)
            {
                _disposed = false;
                _SetCount(count);
            }
                
        }
    }
}
