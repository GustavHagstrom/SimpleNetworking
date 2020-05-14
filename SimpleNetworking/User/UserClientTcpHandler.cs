using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetworking.User
{
    public class UserClientTcpHandler : IDisposable
    {
        public TcpClient client { get; private set; }// = new TcpClient();
        //private IPEndPoint endPoint;
        private List<byte> receivedUnhandledBytes = new List<byte>();

        //public bool IsConnected { get => client.Connected; }
        //public IPEndPoint ConnectedIPEndPoint { get => ((IPEndPoint)client.Client.RemoteEndPoint); }
        public int DataBufferSize { get; set; } = 1024 * 4;
        public event EventHandler<bool> ConnectionSucceded;
        public event EventHandler<Packet> PacketReceived;
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
        public void Send(Packet packet)
        {
            client.GetStream().Write(packet.Bytes, 0, packet.PacketLength);
            Thread.Sleep(1);
        }
        private void Receive()
        {
            byte[] buffer = new byte[DataBufferSize];
            client.GetStream().BeginRead(buffer, 0, buffer.Length, (result) =>
            {
                int readLength = client.GetStream().EndRead(result);
                Receive();

                byte[] buffer = (byte[])result.AsyncState;
                receivedUnhandledBytes.AddRange(buffer.Take(readLength));

                Packet packet = HandleReceivedBytes();
                if(packet != null)
                {
                    PacketReceived?.Invoke(this, packet);
                }

            }, buffer);
        }
        private Packet HandleReceivedBytes()
        {
            int receivedLength = receivedUnhandledBytes.Count;
            if (receivedLength <= 4)
            {
                return null;
            }

            int packetLength = BitConverter.ToInt32(receivedUnhandledBytes.Take(4).ToArray());
            if (receivedLength < Packet.HEADERSOFFSET)
            {
                return null;
            }

            Packet packet = new Packet
            {
                Bytes = receivedUnhandledBytes.Take(packetLength).ToArray(),
            };
            receivedUnhandledBytes.RemoveRange(0, packetLength);
            return packet;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
