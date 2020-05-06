using System.Collections.Generic;

namespace SimpleNetworking
{
    public interface IUserClient
    {
        int Id { get; set; }
        Queue<IPacket> ReceivedPackets { get; }
        IUserClientTcpHandler Tcp { get; }
    }
}