using SimpleNetworking.DI;
using System.Collections.Generic;

namespace SimpleNetworking
{


    public class UserClient : IUserClient
    {


        public int Id { get; set; } = 0;
        public IUserClientTcpHandler Tcp { get; private set; }
        public Queue<IPacket> ReceivedPackets { get; private set; } 
        public UserClient() : this(
            ServiceLocator.Instance.Get<IUserClientTcpHandler>())
        {

        }
        internal UserClient(IUserClientTcpHandler tcpHandler)
        {
            Tcp = tcpHandler;
            Tcp.HandleReceivedPacket = OnPacketReceived;
            ReceivedPackets = new Queue<IPacket>();
        }
        private void OnPacketReceived(IPacket packet)
        {
            //IPacket packet = args.Packet;
            packet.ClientId = Id;
            ReceivedPackets.Enqueue(packet);
        }

    }
}
