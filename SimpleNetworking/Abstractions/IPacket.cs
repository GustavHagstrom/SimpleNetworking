using System;

namespace SimpleNetworking
{
    public interface IPacket
    {
        byte[] Bytes { get; }
        byte[] Data { get; set; }
        int PacketTypeId { get; set; }
        DateTime Sent { get; set; }
        DateTime Received { get; set; }
        int ClientId { get; set; }
        void SetContentFromReceivedBytes(byte[] data);
    }
}