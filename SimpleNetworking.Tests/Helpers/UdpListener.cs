using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking.Tests.Helpers
{
    public class UdpListener
    {
        //private Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private UdpClient _socket = new UdpClient();
        //private const int bufSize = 8 * 1024;
        //private State state = new State();
        private IPEndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private AsyncCallback recv = null;
        public List<byte[]> ReceivedData { get; } = new List<byte[]>();

        public class State
        {
            //public byte[] buffer = new byte[bufSize];
        }

        public void Server(string address, int port)
        {
            _socket.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            //_socket.Client.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            //_socket.Connect(IPAddress.Parse(address), port);
            //_socket.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
            _socket.Connect(IPAddress.Parse(address), port);
            Receive();
        }

        public void Client(string address, int port)
        {
            //_socket.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            //_socket.Client.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            _socket.Connect(IPAddress.Parse(address), port);
            
            Receive();
        }

        public void ServerSend(byte[] data, string address, int port)
        {
            var ep = new IPEndPoint(IPAddress.Parse(address), port);
            _socket.Send(data, data.Length, ep);
        }
        public void Send(byte[] data)
        {
            //byte[] data = Encoding.ASCII.GetBytes(text);
            _socket.BeginSend(data, data.Length, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndSend(ar);
                //Console.WriteLine("SEND: {0}, {1}", bytes, text);
            }, null);
        }

        private void Receive()
        {
            _socket.BeginReceive(recv = (ar) =>
            {
                //State so = (State)ar.AsyncState;
                byte[] data = _socket.EndReceive(ar, ref epFrom);

                _socket.BeginReceive(recv, null);

                ReceivedData.Add(data);
            }, null);
        }
    }
}
