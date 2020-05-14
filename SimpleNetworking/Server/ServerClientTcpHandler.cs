using SimpleNetworking.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace SimpleNetworking.Server
{
    internal class ServerClientTcpHandler : IDisposable
    {
        private List<byte> receivedUnhandledBytes = new List<byte>();

        public TcpClient client { get; private set; }
        public int DataBufferSize { get; set; } = 1024 * 4;


        internal event EventHandler<DisconnectedEventArgs> Disconnected;
        internal event EventHandler<Packet> PacketReceived;

        public void Send(Packet packet)
        {
            try
            {
                client.GetStream().Write(packet.Bytes);
                Thread.Sleep(1);
            }
            catch (Exception e)
            {
                Dispose();
                Disconnected?.Invoke(this, new DisconnectedEventArgs(e, 0));
            }
        }
        public void SetConnection(TcpClient client)
        {
            this.client = client;
            Receive();
        }
        private void Receive()
        {
            try
            {
                byte[] buffer = new byte[DataBufferSize];
                client.GetStream().BeginRead(buffer, 0, DataBufferSize, new AsyncCallback(ReceiveCallback), buffer);
            }
            catch (Exception e)
            {
                Dispose();
                Disconnected?.Invoke(this, new DisconnectedEventArgs(e, 0));
            }
        }
        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = client.GetStream().EndRead(result);
                Receive();

                byte[] buffer = (byte[])result.AsyncState;
                receivedUnhandledBytes.AddRange(buffer.Take(byteLength));

                Packet packet = HandleReceivedBytes();
                if (packet != null)
                {
                    PacketReceived?.Invoke(this, packet);
                }
            }
            catch (Exception e)
            {
                Dispose();
                Disconnected?.Invoke(this, new DisconnectedEventArgs(e, 0));
            }
            
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
            if(client != null)
            {
                client.Dispose();
            }
            
        }
    }
}
