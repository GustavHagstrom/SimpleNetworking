using System;
using System.Linq;
using System.Net.Sockets;

namespace SimpleNetworking
{
    public abstract class TcpHandlerBase : IDisposable
    {
        protected TcpClient socket;

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
                socket.GetStream().BeginWrite(bytesToSend, 0, bytesToSend.Length, null, null);
            }
            catch (Exception e)
            {
                //throw e;
                Dispose();
                Disconnected?.Invoke(e, ProtocolType.Tcp, 0);
            }
        }
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
            StateObject state = (StateObject)result.AsyncState;
            int byteLength = socket.GetStream().EndRead(result);

            if (state.Resolve(byteLength))
            {
                Packet packet = new Packet { AllBytes = state.Data, Received = DateTime.Now };
                PacketReceived?.Invoke(packet);

                byte[] rest = state.RestData;
                state = new StateObject(DataBufferSize) { Data = rest };
            }

            BeginReadingNetworkStream(state);
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
    }
}
