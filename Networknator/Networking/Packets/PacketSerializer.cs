using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MessagePack;

namespace Networknator.Networking.Packets
{
    public class PacketSerializer
    {
        public static T Deserialize<T>(byte[] packetBytes) where T : class
        {
            return MessagePackSerializer.Deserialize<T>(packetBytes);
        }

        public static byte[] Serialize<T>(T packet)
        {
            return new PacketBuilder()
                .Write(typeof(T).Name)
                .Write(MessagePackSerializer.Serialize<T>(packet))
                .Done();
        }

    }
}