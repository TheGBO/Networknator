namespace Tests;

public class Tests
{

    [TearDown]
    public void Teardown()
    {
        Server.Stop();
        Client.Stop();
    }

    [Test]
    public void Client_Connects_To_Server_Causes_Success()
    {
        bool serverDetectedConnection = false;
        bool clientConnected = false;
        Server.Start(8600);
        Server.OnClientConnected += id => 
        {
            serverDetectedConnection = true;
        };

        Client.OnConnected += () =>
        {
            clientConnected = true;
            Assert.IsTrue(serverDetectedConnection && clientConnected && Client.IsRunning);
        };
        Client.Start("127.0.0.1", 8600);
    }
}