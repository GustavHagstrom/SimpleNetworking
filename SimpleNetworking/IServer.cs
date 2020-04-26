using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IServer
    {
        Dictionary<int, IClient> Clients { get; }
        event ClientConnectedToServerEventHandler ClientConnectedToServer;
        int MaxConnections { get; }
        int Port { get; }

        void Start();
        void Stop();
    }
}