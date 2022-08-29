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
        public Dictionary<int, Connection> clients = new Dictionary<int, Connection>();
        private TcpListener tcpListener;
        public int Port { get; set; }
        public int MaxClients { get; set; }
        public bool IsRunning { get; private set; }

        public event Action<int> OnConnection;
        public event Action<int> OnDisconnection;
        public event Action<int, byte[]> OnDataReceived;
        private byte[] buffer;

        public Dictionary<int, PacketHandler> packetHandlers = new Dictionary<int, PacketHandler>();
        public delegate void PacketHandler(byte[] data);

        public void Run(int port, int maxClients = 32)
        {
            if (IsRunning) return;
            Port = port;
            MaxClients = maxClients;
            buffer = new byte[4096];

            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            InitServerData();

            IsRunning = true;
            new Thread(ListenThread).Start();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private void InitServerData()
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients.Add(i, new Connection(i));
            }
        }

        private void ListenThread()
        {
            while (IsRunning)
            {
                TcpClient client = tcpListener.AcceptTcpClient();
                int newClientID = AddConnectedClient(client);
            }
        }

        private int AddConnectedClient(TcpClient client)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if(clients[i].Socket == null)
                {
                    clients[i].OnDataReceived += (id, data) => OnDataReceived.Invoke(id, data);
                    clients[i].Connect(client);
                    OnConnection?.Invoke(i);
                    return i;
                }
            }
            return -1;
        }

        public void DisconnectClient(int clientID)
        {
            clients[clientID].Disconnect();
            
            NetworknatorLogger.Log(LogType.normal, $"Client disconnected {clientID}");
            OnDisconnection?.Invoke(clientID);
        }

        public void SendDataToAll(byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                SendDataTo(i, data);
            }
        }

        public void SendDataTo(int clientID, byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (clients[i].Socket != null && i == clientID)
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
