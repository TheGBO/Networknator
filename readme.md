# Networknator
Another C# Networking library That uses TCP

## C# Example
```cs
class Program
    {
        static void Main(string[] args)
        {
            NetworknatorLogger.StartLogger(Console.WriteLine);


            Server server = new Server();
            server.OnDataReceived += (id, data) =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine("[Server Received] :: " + reader.ReadString());
                }
                server.SendDataTo(id, new PacketBuilder()
                    .Write("pong")
                    .Done());
                Console.WriteLine("[Server Sent] :: pong");

            };



            Client client = new Client();
            client.OnConnected += () =>
            {
                client.Send(new PacketBuilder()
                    .Write("ping")
                    .Done());
                Console.WriteLine("[Client Sent] :: ping");
            };

            client.OnDataReceived += data =>
            {
                using (PacketReader reader = new PacketReader(data))
                {
                    Console.WriteLine("[Client Received] :: " + reader.ReadString());
                }
            };

            server.Run(29509);
            client.Run(server.ConnectionString());
        }
    }
```