using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IServer
    {
        List<ServerClient> Clients { get; }
        int MaxConnections { get; }
        int Port { get; }
        Queue<IPacket> ReceivedPackets { get; }
        void StartListening(int maxConnections, int port);
        void StopListening();
        event DisconnectedEventHandler ClientDisconnected;
        event ConnectedEventHandler ClientConnected;
        
    }
}