using SimpleNetworking.Tools;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleNetworking.Common
{
    public abstract class TcpHandler
    {
        public TcpClient client { get; protected set; }
        public int DataBufferSize { get; set; } = 1024 * 4;
        public event EventHandler<Packet> PacketReceived;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
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
        protected void Receive()
        {
            try
            {
                byte[] buffer = new byte[DataBufferSize];
                client.GetStream().BeginRead(buffer, 0, buffer.Length, (result) =>
                {
                    try
                    {
                        int byteLength = client.GetStream().EndRead(result);
                        Receive();

                        byte[] buffer = (byte[])result.AsyncState;
                        PacketReceived?.Invoke(this, new Packet { Bytes = buffer.Take(byteLength).ToArray() });
                    }
                    catch (Exception e)
                    {
                        Dispose();
                        Disconnected?.Invoke(this, new DisconnectedEventArgs(e, 0));
                    }
                }, buffer);
            }
            catch (Exception e)
            {
                Dispose();
                Disconnected?.Invoke(this, new DisconnectedEventArgs(e, 0));
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
            }

        }
    }
}
