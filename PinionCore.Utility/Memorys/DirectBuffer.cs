using System;
using System.Collections;
using System.Collections.Generic;

namespace PinionCore.Memorys
{
    internal class DirectBuffer : Buffer
    {

        private readonly bool _disposed;

        private readonly object _syncRoot = new object();
        ArraySegment<byte> _Buffer;
        internal DirectBuffer(ArraySegment<byte> buff)
        {
            _Buffer = buff;
            Capacity = buff.Count;
            _disposed = false;

        }
        internal DirectBuffer(byte[] buff) : this(new ArraySegment<byte>(buff, 0, buff.Length))
        {

        }
        internal DirectBuffer(int size) : this(new byte[size])
        {

        }

        public int Capacity { get; }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _Buffer.Count;
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
                    return _Buffer.Array[_Buffer.Offset + index];
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
                    _Buffer.Array[_Buffer.Offset + index] = value;
                }
            }
        }


        public IEnumerator<byte> GetEnumerator()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DirectBuffer));

            for (var i = 0; i < Count; i++)
            {
                yield return _Buffer.Array[_Buffer.Offset + i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


    }
}
