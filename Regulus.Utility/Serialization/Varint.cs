using System;
using System.Collections;
using System.Collections.Generic;

namespace Regulus.Serialization
{
    public class Varint
    {

        public static readonly byte Endian = 0x80;

        
        public static ArraySegment<byte> FindVarint(ref ArraySegment<byte> bytes)
        {
            
            for (int i = 0; i < bytes.Count; i++)
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
            int offset = bytes.Offset;
            int end = bytes.Offset + bytes.Count;

            while (offset < end)
            {
                // 使用 BufferToNumber 来解析 varint
                int dataLength;
                int bytesRead = BufferToNumber(bytes.Array, offset, out dataLength);

                if (bytesRead == 0)
                {
                    // varint 不完整，无法继续读取
                    yield break;
                }
                else if (bytesRead < 0)
                {
                    // 发生溢出，停止处理
                    yield break;
                }

                // 计算数据段的位置和长度
                int dataOffset = offset + bytesRead;
                int dataCount = dataLength;

                if (dataOffset + dataCount > end)
                {
                    // 剩余的字节不足以组成一个完整的数据段，不返回不完整的对
                    yield break;
                }

                // 创建 varint 和数据的 ArraySegment
                var varintSegment = new ArraySegment<byte>(bytes.Array, offset, bytesRead);
                var dataSegment = new ArraySegment<byte>(bytes.Array, dataOffset, dataCount);

                // 返回完整的 varint-data 对
                yield return new Package
                {
                    Head = varintSegment,
                    Body = dataSegment
                };

                // 更新偏移量，继续处理下一个对
                offset = dataOffset + dataCount;
            }
        }



        public static int NumberToBuffer(byte[] buffer, int offset, int value)
        {
            return Varint.NumberToBuffer(buffer, offset, (ulong)value);
        }
        public static int NumberToBuffer(byte[] buffer, int offset, ulong value)
        {
            int i = 0;
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
        public static int BufferToNumber(Regulus.Memorys.Buffer buffer, int offset, out int value)
        {
            ulong val;
            int count = BufferToNumber(buffer, offset, out val);
            value = (int)val;

            return count;
        }
        public static int BufferToNumber(Regulus.Memorys.Buffer buffer, int offset, out ulong value)
        {            
            value = 0;
            int s = 0;
            for (int i = 0; i < buffer.Count - offset; i++)
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
            int count = BufferToNumber(buffer, offset, out val);
            value = (int)val;

            return count;
        }
        public static int BufferToNumber(byte[] buffer, int offset, out ulong value)
        {
            value = 0;
            int s = 0;
            for (int i = 0; i < buffer.Length - offset; i++)
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