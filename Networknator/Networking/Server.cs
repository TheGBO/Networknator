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
        public static Dictionary<int, ServerConnection> clients = new Dictionary<int, ServerConnection>();

        public static PacketHandlers packetHandlers = new PacketHandlers();
        public static bool IsRunning { get; private set; }
        public static event Action<int> OnClientConnected;
        public static event Action<int> OnClientDisconnected;

        public static void Start(int port, int maxClients = 64)
        {
            MaxClients = maxClients;
            Port = port;
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();

            NetworknatorLogger.NormalLog($"Starting server at port {Port}");
            InitializeServerData();

            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            IsRunning = true;
            NetworknatorLogger.NormalLog("Started server successfully!");
        }

        public static void Stop()
        {
            IsRunning = false;
            tcpListener.Stop();
            packetHandlers.handlers.Clear();
            NetworknatorLogger.NormalLog("Server Stopped!");
            clients.Clear();
        }

        private static void SendTCPDataTo(int id, byte[] data)
        {
            try
            {
                clients[id].tcp.Send(data);
            }
            catch (System.Exception)
            {
                clients[id].tcp.Disconnect();
            }
        }

        private static void SendTCPDataToAll(byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients[i].tcp.Send(data);
            }
        }

        public static void SendTCPDataToAll<T>(T packet) => SendTCPDataToAll(PacketSerializer.Serialize<T>(packet));

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
                    clients[i].tcp.OnDisconnection += id => OnClientDisconnected.Invoke(id);
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