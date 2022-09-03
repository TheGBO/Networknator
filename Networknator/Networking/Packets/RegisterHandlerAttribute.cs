using System;

namespace Networknator.Networking.Packets
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterHandlerAttribute : Attribute
    {
        public Type PacketToRegister { get; set; }

        public RegisterHandlerAttribute(Type packetToRegister)
        {
            PacketToRegister = packetToRegister;
        }
    }
}