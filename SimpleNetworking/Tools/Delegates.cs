using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking
{
    public delegate void PacketReceivedEventHandler(IPacket packet);
    public delegate void DisconnectedEventHandler(Exception e, ConnectionProtocolType type, int clientId);
    public delegate void ConnectedEventHandler(ConnectionProtocolType type, int clientId);
}
