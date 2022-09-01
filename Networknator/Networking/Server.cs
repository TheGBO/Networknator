using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Networknator.Networking.Packets;
using Networknator.Utils;

namespace Networknator.Networking
{
    public class Server
    {
        public static int MaxClients { get; private set; }
        public static int Port{ get; set; }

        private static TcpListener tcpListener;
        private static Dictionary<int, ServerConnection> clients = new Dictionary<int, ServerConnection>();

        public static PacketHandlers packetHandlers = new PacketHandlers();
        public static bool IsRunning { get; private set; }
        public static event Action<int> OnClientConnected;

        public static void Start(int port, int maxClients = 64)
        {
            MaxClients = maxClients;
            Port = port;

            NetworknatorLogger.NormalLog($"Starting server at port {Port}");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            IsRunning = true;
            NetworknatorLogger.NormalLog("Started server successfully!");
        }

        public static void Stop()
        {
            IsRunning = false;
            tcpListener.Stop();
            NetworknatorLogger.NormalLog("Server Stopped!");
        }

        public static void SendTCPDataTo(int id, byte[] data)
        {
            try
            {
                clients[id].tcp.Send(data);
            }
            catch (System.Exception)
            {
                
            }
        }

        public static void SendTCPDataToAll(byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients[i].tcp.Send(data);
            }
        }

        public static void SendTCPDataTo<T>(int id, T packet)
        {
            SendTCPDataTo(id, PacketSerializer.Serialize<T>(packet));
        }

        private static void TCPConnectCallback(IAsyncResult result)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
            NetworknatorLogger.NormalLog($"Incoming connection from {client.Client.RemoteEndPoint.ToString()}");

            for (int i = 1; i <= MaxClients; i++)
            {
                if(clients[i].tcp.socket == null)
                {
                    clients[i].tcp.ConnectServer(client);
                    OnClientConnected?.Invoke(i);
                    clients[i].tcp.OnData += (id, data) => packetHandlers.HandlePacket(id, data, true);
                    return;
                }
            }
        }

        private static void InitializeServerData()
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients.Add(i, new ServerConnection(i));
            }
        }

    }
}