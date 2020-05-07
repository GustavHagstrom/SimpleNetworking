using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IUserClient : IClientBase
    {
        UserClientTcpHandler Tcp { get; }
        //int Id { get; set; }
        Queue<IPacket> ReceivedPackets { get; }

        event DisconnectedEventHandler Disconnected;
        event ConnectedEventHandler Connected;
        //IUserClientTcpHandler Tcp { get; }
    }
}