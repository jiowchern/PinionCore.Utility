using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace PinionCore.Memorys
{
    internal class PooledBuffer : Buffer
    {
        private readonly ChunkPool _chunkPool;
        private readonly ArraySegment<byte> _Base;
        ArraySegment<byte> _Buffer;
        private bool _IsDisposed;
        public readonly object _Sync;

        public readonly byte[] Page;
        internal PooledBuffer(ChunkPool chunkPool, ArraySegment<byte> segment )
        {
            _Sync = new object();
            _chunkPool = chunkPool;
            _Base = segment;
            _IsDisposed = false;
            _SetCount(0);
            Page = segment.Array;
        }

        private void _SetCount(int count)
        {
            if(count > _Base.Count)
                throw new ArgumentOutOfRangeException(nameof(count));

            _Buffer = new ArraySegment<byte>(_Base.Array, _Base.Offset, count);   
        }

        ~PooledBuffer()
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                _chunkPool.Return(_Base);
            }
        }

        public int Capacity => _Base.Count;

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
                if (_IsDisposed)
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
                    if (_IsDisposed)
                        throw new ObjectDisposedException(nameof(PooledBuffer));

                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    try
                    {
                        return _Buffer.Array[_Buffer.Offset + index];
                    }
                    catch (Exception e)
                    {

                        throw e;
                    }
                    
                }
                
            }
            set
            {
                lock (_Sync)
                {
                    if (_IsDisposed)
                        throw new ObjectDisposedException(nameof(PooledBuffer));

                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));

                    try
                    {
                        _Buffer.Array[_Buffer.Offset + index] = value;
                    }
                    catch (Exception e)
                    {

                        throw e ;
                    }
                    
                }
                    
            }
        }

      

        public IEnumerator<byte> GetEnumerator()
        {
            lock (_Sync)
            {
                if (_IsDisposed)
                    throw new ObjectDisposedException(nameof(PooledBuffer));

                for (int i = 0; i < Count; i++)
                {
                    yield return _Buffer.Array[_Buffer.Offset + i];
                }
            }
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        

        internal void Reset(int count)
        {
            lock (_Sync)
            {
                _IsDisposed = false;
                _SetCount(count);
            }
                
        }
    }
}
