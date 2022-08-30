public enum ClientToServer
{
    welcomeReceived,
    playerPositionUpdate
}

public enum ServerToClient
{
    welcome,
    playerConnected,
    playerDisconnected,
    playerPositionUpdate,
    playerLeft
}