using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IServer
    {
        Dictionary<int, IClient> Clients { get; }
        event ClientConnectionChangedEventHandler ClientConnectionChanged;
        int MaxConnections { get; }
        int Port { get; }

        void Start();
        void Stop();
    }
}