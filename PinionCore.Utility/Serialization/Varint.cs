using System;
using System.Collections.Generic;

namespace PinionCore.Serialization
{
    public class Varint
    {

        public static readonly byte Endian = 0x80;


        public static ArraySegment<byte> FindVarint(ref ArraySegment<byte> bytes)
        {

            for (var i = 0; i < bytes.Count; i++)
            {

                var value = bytes.Array[bytes.Offset + i];
                if (value < Endian)
                {
                    return new ArraySegment<byte>(bytes.Array, bytes.Offset, i + 1);
                }
            }
            return new ArraySegment<byte>(bytes.Array, bytes.Offset, 0);
        }

        public struct Package
        {
            public ArraySegment<byte> Head;
            public ArraySegment<byte> Body;
        }
        public static IEnumerable<Package> FindPackages(ArraySegment<byte> bytes)
        {
            var offset = bytes.Offset;
            var end = bytes.Offset + bytes.Count;

            while (offset < end)
            {
                int dataLength;
                var bytesRead = BufferToNumber(bytes.Array, offset, out dataLength);

                if (bytesRead == 0)
                {
                    yield break;
                }
                else if (bytesRead < 0)
                {
                    yield break;
                }

                var dataOffset = offset + bytesRead;
                var dataCount = dataLength;

                if (dataOffset + dataCount > end)
                {
                    yield break;
                }

                var varintSegment = new ArraySegment<byte>(bytes.Array, offset, bytesRead);
                var dataSegment = new ArraySegment<byte>(bytes.Array, dataOffset, dataCount);

                yield return new Package
                {
                    Head = varintSegment,
                    Body = dataSegment
                };

                offset = dataOffset + dataCount;
            }
        }



        public static int NumberToBuffer(byte[] buffer, int offset, int value)
        {
            return Varint.NumberToBuffer(buffer, offset, (ulong)value);
        }
        public static int NumberToBuffer(byte[] buffer, int offset, ulong value)
        {
            var i = 0;
            while (value >= Endian)
            {
                buffer[offset + i] = (byte)(value | Endian);
                value >>= 7;
                i++;
            }
            buffer[offset + i] = (byte)value;
            return i + 1;
        }

        public static int GetByteCount(int value)
        {
            return Varint.GetByteCount((ulong)value);
        }
        public static int GetByteCount(ulong value)
        {
            int i;
            for (i = 0; i < 9 && value >= Endian; i++, value >>= 7) { }
            return i + 1;
        }
        public static int BufferToNumber(PinionCore.Memorys.Buffer buffer, int offset, out int value)
        {
            ulong val;
            var count = BufferToNumber(buffer, offset, out val);
            value = (int)val;

            return count;
        }
        public static int BufferToNumber(PinionCore.Memorys.Buffer buffer, int offset, out ulong value)
        {
            value = 0;
            var s = 0;
            for (var i = 0; i < buffer.Count - offset; i++)
            {
                ulong bufferValue = buffer[offset + i];
                if (bufferValue < Endian)
                {
                    if (i > 9 || i == 9 && bufferValue > 1)
                    {
                        value = 0;
                        return -(i + 1); // overflow
                    }
                    value |= bufferValue << s;
                    return i + 1;
                }
                value |= (bufferValue & 0x7f) << s;
                s += 7;
            }
            value = 0;
            return 0;
        }
        public static int BufferToNumber(byte[] buffer, int offset, out int value)
        {
            ulong val;
            var count = BufferToNumber(buffer, offset, out val);
            value = (int)val;

            return count;
        }
        public static int BufferToNumber(byte[] buffer, int offset, out ulong value)
        {
            value = 0;
            var s = 0;
            for (var i = 0; i < buffer.Length - offset; i++)
            {
                ulong bufferValue = buffer[offset + i];
                if (bufferValue < Endian)
                {
                    if (i > 9 || i == 9 && bufferValue > 1)
                    {
                        value = 0;
                        return -(i + 1); // overflow
                    }
                    value |= bufferValue << s;
                    return i + 1;
                }
                value |= (bufferValue & 0x7f) << s;
                s += 7;
            }
            value = 0;
            return 0;
        }

        public static int GetMaxInt32Length()
        {
            return GetByteCount(-1);
        }
    }
}
