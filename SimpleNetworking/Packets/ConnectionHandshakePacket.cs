using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Packets
{
    public class ConnectionHandshakePacket : Packet
    {
        public int ServerAssignedId 
        { 
            get => BitConverter.ToInt32(Data); 
            set
            {
                Data = BitConverter.GetBytes(value);
            } 
        }
        public ConnectionHandshakePacket() : base()
        {
        }

    }
}
