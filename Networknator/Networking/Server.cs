using Networknator.Networking.Packets;
using Networknator.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Networknator.Networking
{
    public class Server
    {
        public Dictionary<int, ServerConnection> clients = new Dictionary<int, ServerConnection>();
        private TcpListener tcpListener;
        public int Port { get; private set; }
        public int MaxClients { get; set; }
        public bool IsRunning { get; private set; }

        public event Action<int> OnConnection;
        public event Action<int> OnDisconnection;
        public event Action<int, byte[]> OnDataReceived;

        public Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>();
        public delegate void PacketHandler(byte[] data);

        public string ConnectionString() => $"{IPUtils.GetLocalIPAddress()}:{Port}";

        public void Run(int port, int maxClients = 32)
        {
            if (IsRunning) return;
            Port = port;
            MaxClients = maxClients;

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            InitServerData();

            NetworknatorLogger.Log(LogType.normal, $"Server Initialized at port: {Port} !");
            new Thread(() => 
            {
                IsRunning = true;
                while (IsRunning)
                {
                    try
                    {
                        TcpClient client = tcpListener.AcceptTcpClient();
                        AddConnectedClient(client);
                    }
                    catch (Exception) { }
                }
            }).Start();
        }

        public void Stop()
        {
            tcpListener.Stop();
            IsRunning = false;
        }

        private void InitServerData()
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients.Add(i, new ServerConnection(i));
            }
        }

        private void AddConnectedClient(TcpClient client)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if(clients[i].socket == null)
                {
                    clients[i].OnDataFromClient += (id, data) => OnDataReceived.Invoke(id, data);
                    clients[i].OnDisconnected += (id) => OnDisconnection?.Invoke(id);
                    clients[i].Connect(client);
                    OnConnection?.Invoke(i);
                    return;
                }
            }
        }

        public void DisconnectClient(int clientID)
        {
            clients[clientID].Disconnect();
        }

        public void SendDataToAll(byte[] data, bool exclude = false, int excludeID = 0)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (exclude)
                {
                    if(i != excludeID)
                        SendDataTo(i, data);
                }
                else
                {
                    SendDataTo(i, data);
                }
            }
        }

        public void SendDataTo(int clientID, byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (clients[i].socket != null && i == clientID)
                {
                    try
                    {
                        clients[i].Send(data);
                    }
                    catch (Exception e)
                    {
                        NetworknatorLogger.Log(LogType.error, "Error sending data: " + e);
                        DisconnectClient(i);
                    }
                }
            }
        }
    }
}
