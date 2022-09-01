using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Networknator.Utils;

namespace Networknator.Networking
{
    public class Server
    {
        public static int MaxClients { get; private set; }
        public static int Port{ get; set; }

        private static TcpListener tcpListener;
        private static Dictionary<int, ServerConnection> clients = new Dictionary<int, ServerConnection>();

        public static event Action<int, byte[]> OnData;

        public static void Start(int port, int maxClients = 64)
        {
            MaxClients = maxClients;
            Port = port;

            NetworknatorLogger.NormalLog($"Starting server at port {Port}");
            InitializeServerData();

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

            NetworknatorLogger.NormalLog("Started server successfully!");
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
                    clients[i].tcp.OnData += (id, data) => OnData?.Invoke(id, data);
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