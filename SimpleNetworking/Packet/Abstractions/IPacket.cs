using System;

namespace SimpleNetworking
{
    public interface IPacket
    {
        byte[] AllBytes { get; set; }
        byte[] Data { get; set; }
        int PacketTypeId { get; set; }
        DateTime Sent { get; set; }
        DateTime Received { get; set; }
        int ClientId { get; set; }
    }
}