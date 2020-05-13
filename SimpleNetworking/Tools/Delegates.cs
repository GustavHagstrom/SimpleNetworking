using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{
    public delegate void PacketReceivedEventHandler(IPacket packet, ProtocolType type);
    public delegate void DisconnectedEventHandler(Exception e, ProtocolType type, int clientId);
    public delegate void ConnectedEventHandler(IPAddress remoteAddress, ProtocolType type, int clientId);
    public delegate void ConnectionFailedEventHandler(Exception e, ProtocolType type);
}
