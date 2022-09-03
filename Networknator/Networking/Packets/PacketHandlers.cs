using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Networknator.Utils;

namespace Networknator.Networking.Packets
{
    public class PacketHandlers
    {
        public Dictionary<string, IPacketHandler> handlers = new Dictionary<string, IPacketHandler>();

        public void Register<THandler, TPacket>() 
        where THandler : PacketHandlerBase<TPacket>, new()
        where TPacket : class
        {
            NetworknatorLogger.NormalLog($"Registering packet handler, {typeof(THandler).Name}:{typeof(TPacket).Name}");
            handlers.Add(typeof(TPacket).Name, (THandler)Activator.CreateInstance(typeof(THandler)));
        }

        //Call this method to automatically handle packets without registering them manually
        public void AutoRegisterHandlers()
        {
            handlers.Clear();
            var assembly = Assembly.GetCallingAssembly();
            var types = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(RegisterHandlerAttribute), true).Length > 0);

            foreach (var t in types)
            {
                var thandler = t;
                var tpacket = t.GetCustomAttribute<RegisterHandlerAttribute>().PacketToRegister;
                var regmethod = typeof(PacketHandlers).GetMethod(nameof(Register));
                var reggeneric = regmethod.MakeGenericMethod(thandler, tpacket);
                reggeneric.Invoke(this, null);
            }
        }

        public async void HandlePacket(int senderid, byte[] data, bool server)
        {
            using(PacketReader reader = new PacketReader(data))
            {
                string handlerName = reader.ReadString();
                int objectSize = reader.ReadInt();
                byte[] packetDataRead = reader.Read(objectSize);
                if(handlers.TryGetValue(handlerName, out IPacketHandler handler))
                {
                    if(server)
                        await handler.HandleServer(packetDataRead, senderid);
                    else
                        await handler.HandleClient(packetDataRead);
                }
                else
                {
                    NetworknatorLogger.ErrorLog($"Packet handler {handlerName} not implemented!");
                }
            }
        }
    }
}