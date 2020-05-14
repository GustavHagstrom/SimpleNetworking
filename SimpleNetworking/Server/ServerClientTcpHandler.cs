using SimpleNetworking.Common;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace SimpleNetworking.Server
{
    internal class ServerClientTcpHandler : TcpHandler, IDisposable
    {
        public ServerClientTcpHandler()
        {

        }
        public void SetConnection(TcpClient client)
        {
            this.client = client;
            Receive();
        }
    }
}
