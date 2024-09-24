using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Memorys
{
    internal class DirectBuffer : Buffer
    {
        private readonly byte[] _array;
        private bool _disposed;
        private int _count;
        private readonly object _syncRoot = new object();
        ArraySegment<byte> _Buffer;
        internal DirectBuffer(byte[] buff)
        {
            _array = buff;
            Capacity = buff.Length;
            _disposed = false;
            _SetCount(buff.Length);
        }
        internal DirectBuffer(int size)
        {
            _array = new byte[size];
            Capacity = size;
            _disposed = false;
            _SetCount(size);
        }

        private void _SetCount(int count)
        {
            _count = count;
            _Buffer = new ArraySegment<byte>(_array, 0, _count);
        }

        public int Capacity { get; }

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

        ArraySegment<byte> Buffer.Bytes => _Buffer;

        public byte this[int index]
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DirectBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                lock (_syncRoot)
                {
                    return _array[index];
                }
            }
            set
            {
                if (_disposed)
                    throw new ObjectDisposedException(nameof(DirectBuffer));

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));

                lock (_syncRoot)
                {
                    _array[index] = value;
                }
            }
        }

        public int Write(ArraySegment<byte> buffer)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DirectBuffer));

            lock (_syncRoot)
            {
                int bytesToWrite = Math.Min(buffer.Count, Capacity - _count);
                if (bytesToWrite <= 0)
                    return 0;

                System.Buffer.BlockCopy(buffer.Array, buffer.Offset, _array, _count, bytesToWrite);
                _count += bytesToWrite;
                return bytesToWrite;
            }
        }

        public IEnumerator<byte> GetEnumerator()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DirectBuffer));

            for (int i = 0; i < Count; i++)
            {
                yield return _array[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            _disposed = true;
            
            _SetCount(0);
            // 对于直接分配的缓冲区，Dispose 时不需要执行任何操作
        }
    }
}
