using System;
using Networknator.Networking;
using Networknator.Networking.Packets;
using Networknator.Utils;

// This is an example of how to use Networknator in the most basic use case. a ping pong client server //


//Initialize the Logger by writing internal log messages into the console
NetworknatorLogger.StartLogger(Console.WriteLine);


//Create a server instance
Server server = new Server();
// Callback of data received from a clinet
// id is the client identifier, so you can know who sent data.
// data is the byte array containing data
server.OnDataReceived += (id, data) =>
{
    //Here you read the byte array, Networknator has two cool utilities
    //for dealing with data in form of bytes, PacketReader and PacketBuilder. 
    using (PacketReader reader = new PacketReader(data))
    {
        //It IS important to read data in the same order you write it,
        //otherwise you will get REALLY weird glitches.
        string message = reader.ReadString();
        Console.WriteLine("[Server Received] :: " + reader.ReadString());
    }

    //Here is where you Send data, remember the id parameter from the callback? 
    //It is, you can send data to a specific client, in this case, the same client who sent "ping"
    server.SendDataTo(id, new PacketBuilder()
        .Write("pong")
        .Done());
    Console.WriteLine("[Server Sent] :: pong");

};


//Instantiate the client.
Client client = new Client();
//this callback is called when the client is succesfully connected to the server
//In this case we are going to use it for sending data to the server. 
client.OnConnected += () =>
{
    client.Send(new PacketBuilder()
        .Write("ping")
        .Done());
    Console.WriteLine("[Client Sent] :: ping");
};

//since the only thing that will send you data is always the server,
//you can just get the data and there is no id
client.OnDataReceived += data =>
{
    //i forgot to mention it earlier, but it is recommended to use PacketReader in a using statement,
    //so you can dispose the memory stream.
    using (PacketReader reader = new PacketReader(data))
    {
        string message = reader.ReadString();
        Console.WriteLine("[Client Received] :: " + message);
    }
};

//bind the server socket on the specified port
server.Run(29509);
//connect the client on the specified connection string
client.Run("127.0.0.1:29509");