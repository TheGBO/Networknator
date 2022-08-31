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
        private Dictionary<int, ServerConnection> clients = new Dictionary<int, ServerConnection>();
        private TcpListener tcpListener;
        public int Port { get; private set; }
        public int MaxClients { get; set; }
        public bool IsRunning { get; private set; }

        public event Action<int> OnConnection;
        /// <summary>
        /// Called when any client disconnects, gets the id of the disconnection.
        /// </summary>
        public event Action<int> OnDisconnection;
        /// <summary>
        /// Called when the server receives data from a client buffer, gets the id and the data
        /// </summary>
        public event Action<int, byte[]> OnDataReceived;

        /// <summary>
        /// Gets the server connection string on the LAN
        /// </summary>
        /// <returns>a string in the format 127.0.0.1:5858</returns>
        public string GetLANConnectionString() => $"{IPUtils.GetLocalIPAddress()}:{Port}";

        /// <summary>
        /// Initialize and run the server
        /// </summary>
        /// <param name="port">port used to bind the server socket</param>
        /// <param name="maxClients">number of maximum clientss</param>
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


        /// <summary>
        /// Stops the server from running
        /// </summary>
        public void Stop()
        {
            tcpListener.Stop();
            IsRunning = false;
        }

        /// <summary>
        /// Fills the server with empty clients
        /// </summary>
        private void InitServerData()
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                clients.Add(i, new ServerConnection(i));
            }
        }

        /// <summary>
        /// Will add a TCP client to the client dictionary
        /// </summary>
        /// <param name="client">client socket to be added</param>
        private void AddConnectedClient(TcpClient client)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if(clients[i].tcpSocket == null)
                {
                    clients[i].OnDataFromClient += (id, data) => OnDataReceived.Invoke(id, data);
                    clients[i].OnDisconnected += (id) => OnDisconnection?.Invoke(id);
                    clients[i].Connect(client);
                    OnConnection?.Invoke(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Will disconnect a client from the server
        /// </summary>
        /// <param name="clientID">client to be disconnected</param>
        public void DisconnectClient(int clientID)
        {
            clients[clientID].Disconnect();
        }

        /// <summary>
        /// Send data to all clients
        /// </summary>
        /// <param name="data">the data to be sent</param>
        /// <param name="exclude">if should exclude a client from receiving data</param>
        /// <param name="excludeID">the client that will not receive data</param>
        public void SendTCPDataToAll(byte[] data, bool exclude = false, int excludeID = 0)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (exclude)
                {
                    if(i != excludeID)
                        SendTCPDataTo(i, data);
                }
                else
                {
                    SendTCPDataTo(i, data);
                }
            }
        }

        /// <summary>
        /// Sends data to a specified client
        /// </summary>
        /// <param name="clientID">the specified client</param>
        /// <param name="data">the data to be sent</param>
        public void SendTCPDataTo(int clientID, byte[] data)
        {
            for (int i = 1; i <= MaxClients; i++)
            {
                if (clients[i].tcpSocket != null && i == clientID)
                {
                    try
                    {
                        clients[i].SendTCP(data);
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
