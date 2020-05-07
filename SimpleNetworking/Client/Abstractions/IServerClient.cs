
using System.Net;

namespace SimpleNetworking
{
    public interface IServerClient : IClientBase
    {
        ServerClientTcpHandler Tcp { get; }
        string Ip { get; set; }

        
    }
}