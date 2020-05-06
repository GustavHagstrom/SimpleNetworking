using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IServer
    {
        List<IServerClient> Clients { get; }
        int MaxConnections { get; }
        int Port { get; }
        Queue<IPacket> ReceivedPackets { get; }
        void Start(int maxConnections, int port);
        void Stop();
    }
}