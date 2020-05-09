using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{
    public delegate void PacketReceivedEventHandler(IPacket packet);
    public delegate void DisconnectedEventHandler(Exception e, ProtocolType type, int clientId);
    public delegate void ConnectedEventHandler(ProtocolType type, int clientId);
}
