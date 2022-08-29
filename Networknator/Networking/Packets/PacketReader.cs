using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Networknator.Networking.Packets
{
    public class PacketReader : IDisposable, IReader
    {
        private readonly MemoryStream dataStream;

        public PacketReader(byte[] data) : this(data, 0, data.Length)
        {
            
        }

        public PacketReader(byte[] data, int offset, int length)
        {
            dataStream = new MemoryStream(data, offset, length);
        }

        private byte[] Read(int count)
        {
            byte[] ret = new byte[count];
            dataStream.Read(ret, 0, count);
            return ret;
        }

        public byte[] ReadRaw(int length) => Read(length);
        public byte ReadByte() => Read(1)[0];
        public int ReadInt() => BitConverter.ToInt32(Read(sizeof(int)), 0);
        public long ReadLong() => BitConverter.ToInt64(Read(sizeof(long)), 0);
        public float ReadFloat() => BitConverter.ToSingle(Read(sizeof(float)), 0);
        public bool ReadBool() => BitConverter.ToBoolean(Read(sizeof(bool)), 0);
        public string ReadString()
        {
            int length = ReadInt();
            byte[] bytes = Read(length);

            return Encoding.UTF8.GetString(bytes);
        }

        public void Dispose()
        {
            dataStream.Dispose();
        }
    }

    public interface IReader
    {
        byte ReadByte();
        int ReadInt();
        long ReadLong();
        float ReadFloat();
        bool ReadBool();
        string ReadString();
        byte[] ReadRaw(int length);
    }
}
