using Serilog;
using SimpleNetworking.DI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SimpleNetworking
{
    public class ServerClient : IServerClient
    {
        private IServerClientTcpHandler tcp;

        public IServerClientTcpHandler Tcp
        {
            get => tcp;
        }
        public int Id { get; set; } = 0;
        public string Ip { get; set; } = string.Empty;

        public ServerClient() : this(ServiceLocator.Instance.Get<IServerClientTcpHandler>())
        {

        }
        internal ServerClient(IServerClientTcpHandler tcpHandler)
        {
            tcp = tcpHandler;
            //tcp.HandleReceivedPacket = OnPacketReceived;
            //this.logger = logger;
        }

    }
}
