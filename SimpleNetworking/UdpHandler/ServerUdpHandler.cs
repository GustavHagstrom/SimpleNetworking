using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SimpleNetworking
{
    public class ServerUdpHandler
    {
        private UdpClient socket = new UdpClient();
        private IPEndPoint endPoint;
        //private AsyncCallback receiveCallback = null;


        internal event PacketReceivedEventHandler PacketReceived;
        //public UdpHandler(ClientBase client) : this()
        //{ 
        //    //this.client = client;
        //    //this.socket = new UdpClient();
        //}
        public ServerUdpHandler()
        {
            //socket = new UdpClient();
        }
        public void Connect(IPAddress address, int port = 0)
        {
            //endPoint = new IPEndPoint(address, port);
            Debugger.Log(1, null, $"{nameof(ServerUdpHandler)}: Connecting to {address.ToString()} on port {port}\n");
            socket.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket.Connect(address, port);
            endPoint = (IPEndPoint)socket.Client.RemoteEndPoint;
            Debugger.Log(1, null, $"{nameof(ServerUdpHandler)}: UdpClient remoteEndpoint address:{endPoint.Address.ToString()}, port: {endPoint.Port}\n");
            BeginReceiving();
        }
            
        public void Send(Packet packet)
        {
            try
            {
                socket.BeginSend(packet.AllBytes, packet.AllBytes.Length, null, null);
            }
            catch (Exception e)
            {
                throw e;
            }
            Thread.Sleep(1);
        }
        private void BeginReceiving()
        {
            try
            {
                socket.BeginReceive(result => 
                {
                    Packet packet = new Packet
                    {
                        Received = DateTime.Now
                    };
                    PacketReceived?.Invoke(packet, ProtocolType.Udp);
                    //PacketReceived?.Invoke(packet, ProtocolType.Udp);
                    //try
                    //{
                    //    byte[] data = socket.EndReceive(result, ref endPoint);
                    //    BeginReceiving();
                    //    Packet packet = new Packet
                    //    {
                    //        AllBytes = data,
                    //        Received = DateTime.Now
                    //    };
                    //    PacketReceived?.Invoke(packet, ProtocolType.Udp);
                    //}
                    //catch (Exception e)
                    //{
                    //    throw e;
                    //}
                }, null);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
