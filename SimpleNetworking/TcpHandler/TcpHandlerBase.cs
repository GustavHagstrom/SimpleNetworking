using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace SimpleNetworking
{
    public abstract class TcpHandlerBase : IDisposable
    {

        protected TcpClient socket;
        private List<byte> receivedUnhandledBytes = new List<byte>();

        public bool IsConnected
        {
            get
            {
                if (socket == null)
                {
                    return false;
                }
                return socket.Connected;
            }
        }
        public int DataBufferSize { get; set; } = 4096; // Bad if user changes this mid connection
        public TcpHandlerBase()
        {

        }

        internal event DisconnectedEventHandler Disconnected; //ok
        internal event ConnectedEventHandler Connected; //ok
        internal event PacketReceivedEventHandler PacketReceived; //ok

        public void Disconnect()
        {
            Dispose();
        }
        public void Send(Packet packet)
        {
            packet.Sent = DateTime.Now;

            byte[] packetLength = BitConverter.GetBytes(packet.AllBytes.Length);
            byte[] bytesToSend = packetLength.Concat(packet.AllBytes).ToArray();
            try
            {
                //socket.GetStream().BeginWrite(bytesToSend, 0, bytesToSend.Length, new AsyncCallback(WriteCallback), null);
                socket.GetStream().Write(bytesToSend);
                Thread.Sleep(1);
            }
            catch (Exception e)
            {
                //throw e;
                Dispose();
                Disconnected?.Invoke(e, ProtocolType.Tcp, 0);
            }
        }
        //protected void WriteCallback(IAsyncResult result)
        //{
        //    socket.GetStream().EndWrite(result);
        //}
        protected void BeginReadingNetworkStream(StateObject state)
        {
            try
            {
                socket.GetStream().BeginRead(state.Buffer, 0, DataBufferSize, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                //throw e;
                Dispose();
                Disconnected?.Invoke(e, ProtocolType.Tcp, 0);
            }
        }
        protected void ReceiveCallback(IAsyncResult result)
        {
            int byteLength = socket.GetStream().EndRead(result);
            BeginReadingNetworkStream(new StateObject(DataBufferSize));

            StateObject state = (StateObject)result.AsyncState;
            receivedUnhandledBytes.AddRange(state.Buffer.Take(byteLength));

            Packet packet = HandleReceivedBytes();
            if (packet != null)
            {
                PacketReceived?.Invoke(packet);
            }

            //StateObject state = (StateObject)result.AsyncState;
            //int byteLength = socket.GetStream().EndRead(result);

            //if (state.Resolve(byteLength))
            //{
            //    Packet packet = new Packet { AllBytes = state.Data, Received = DateTime.Now };
            //    PacketReceived?.Invoke(packet);

            //    byte[] rest = state.RestData;
            //    state = new StateObject(DataBufferSize) { Data = rest };
            //}

            //BeginReadingNetworkStream(state);
        }
        protected void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }
            Connected?.Invoke(ProtocolType.Tcp, 0);

            this.BeginReadingNetworkStream(new StateObject(DataBufferSize));
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            socket.Dispose();
        }
        private Packet HandleReceivedBytes()
        {
            int receivedLength = receivedUnhandledBytes.Count;
            if (receivedLength <= 4)
            {
                return null;
            }

            int packetLength = BitConverter.ToInt32(receivedUnhandledBytes.Take(4).ToArray());
            if (receivedLength < packetLength + 4)
            {
                return null;
            }

            Packet packet = new Packet
            {
                AllBytes = receivedUnhandledBytes.Skip(4).Take(packetLength).ToArray(),
                Received = DateTime.Now,
            };
            receivedUnhandledBytes.RemoveRange(0, packetLength + 4);
            return packet;
        }
    }
}
