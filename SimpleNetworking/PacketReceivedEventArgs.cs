using System;

namespace SimpleNetworking
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public PacketReceivedEventArgs(Packet packet)
        {
            Packet = packet;
        }

        public Packet Packet { get; private set; }
    }
}
