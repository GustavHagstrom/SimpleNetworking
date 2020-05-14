using SimpleNetworking.Common;
using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetworking.User
{
    public class UserClientTcpHandler : TcpHandler, IDisposable
    {
        public UserClientTcpHandler()
        {
        }

        public event EventHandler<bool> ConnectionSucceded;
        public void Connect(IPAddress address, int port)
        {
            client = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };
            client.BeginConnect(address, port, (result) =>
            {
                client.EndConnect(result);
                if (client.Connected)
                {
                    Receive();
                    ConnectionSucceded.Invoke(this, true);
                }
                else
                {
                    ConnectionSucceded.Invoke(this, false);
                }

            }, null);
        }
    }
}
