using System;
using System.Net.Sockets;

namespace Networknator.Utils
{
    class BufferUtils
    {
        public static void ReceiveData(NetworkStream stream, ref byte[] buffer, Action<byte[]> OnData)
        {
            int byteLength = stream.Read(buffer, 0, 4096);

            byte[] data = new byte[byteLength];

            Array.Copy(buffer, data, byteLength);

            OnData?.Invoke(data);

            Array.Clear(buffer, 0, 4096);
            Array.Clear(data, 0, byteLength);
        }
    }
}