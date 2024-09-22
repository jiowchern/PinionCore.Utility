﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Regulus.Memorys
{
    internal class PooledBuffer : Buffer
    {
        private readonly ChunkPool _chunkPool;
        private readonly ArraySegment<byte> _array;
        
        private bool _disposed;
        private int _count;

        public readonly byte[] Page;
        internal PooledBuffer(ChunkPool chunkPool, ArraySegment<byte> segment )
        {
            _chunkPool = chunkPool;
            _array = segment;
            _disposed = false;
            _count = 0;
            Page = segment.Array;
        }



        public int Capacity => _array.Count;

        public int Count
        {
            get
            {
                return _count;
            }
        }

        ArraySegment<byte> Buffer.Bytes => new ArraySegment<byte>(_array.Array, _array.Offset, _count); 

        public byte this[int index]
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _array.Array[_array.Offset + index];
            }
            set
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                _array.Array[_array.Offset + index] = value;
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
                // clean _array                
                _disposed = true;
                _count = 0;
                _chunkPool.Return(this);
            }
        }

        internal void Reset(int count)
        {
            _disposed = false;
            _count = count;
        }
    }
}