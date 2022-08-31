using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Networknator.Networking.Packets
{

    public class PacketBuilder
    {
        private MemoryStream dataStream = new MemoryStream();

        public byte[] Done()
        {
            byte[] buffer = dataStream.ToArray();

            dataStream.Dispose();
            return buffer;
        }
        
        public PacketBuilder WriteRaw(byte[] data)
        {
            dataStream.Write(data, 0, data.Length);

            return this;
        }
        
        public PacketBuilder Write(byte data) => WriteRaw(new[] { data });
        public PacketBuilder Write(int data) => WriteRaw(BitConverter.GetBytes(data));
        public PacketBuilder Write(long data) => WriteRaw(BitConverter.GetBytes(data));
        public PacketBuilder Write(float data) => WriteRaw(BitConverter.GetBytes(data));
        public PacketBuilder Write(bool data) => WriteRaw(BitConverter.GetBytes(data));
        public PacketBuilder Write(string data) => Write(Encoding.UTF8.GetBytes(data ?? ""));
        public PacketBuilder Write(byte[] data)
        {
            Write(data.Length);
            WriteRaw(data);
            return this;
        }


    }
}
