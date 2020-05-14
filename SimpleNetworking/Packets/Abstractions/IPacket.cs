using System;

namespace SimpleNetworking
{
    public interface IPacket
    {
        byte[] AllBytes { get; set; }
        int ClientId { get; set; }
        byte[] Data { get; set; }
        int PacketLength { get; }
        int PacketTypeId { get; set; }
    }
}