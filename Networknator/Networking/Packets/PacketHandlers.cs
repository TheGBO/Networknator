using System;
using System.Collections.Generic;
using Networknator.Utils;

namespace Networknator.Networking.Packets
{
    public class PacketHandlers
    {
        public Dictionary<string, IPacketHandler> handlers = new Dictionary<string, IPacketHandler>();

        public void Register<THandler, TPacket>() 
        where THandler : PacketHandlerBase<TPacket> 
        where TPacket : class
        {
            handlers.Add(typeof(TPacket).Name, (THandler)Activator.CreateInstance(typeof(THandler)));
        }

        public void HandlePacket(int senderid, byte[] data, bool server)
        {
            using(PacketReader reader = new PacketReader(data))
            {
                string handlerName = reader.ReadString();
                int objectSize = reader.ReadInt();
                byte[] packetDataRead = reader.Read(objectSize);
                if(handlers.TryGetValue(handlerName, out IPacketHandler handler))
                {
                    if(server)
                        handler.HandleServer(packetDataRead, senderid);
                    else
                        handler.HandleClient(packetDataRead);
                }
                else
                {
                    NetworknatorLogger.ErrorLog($"Packet handler {handlerName} not implemented!");
                }
            }
        }
    }
}