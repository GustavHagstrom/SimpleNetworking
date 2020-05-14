using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{

    public delegate void PacketReceivedEventHandler(Packet packet, ProtocolType type);
    public delegate void DisconnectedEventHandler(Exception e, ProtocolType type, int clientId);
    public delegate void ConnectedEventHandler(int clientId);
    public delegate void ConnectionFailedEventHandler(Exception e, ProtocolType type);

    
}
