using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Tests.Helpers
{
    public class ExamplePacket : Packet
    {
        public ExamplePacket()
        {
        }

        public string DataString 
        { 
            get => Encoding.UTF8.GetString(this.Data); 
            set
            {
                Data = Encoding.UTF8.GetBytes(value);
            }
        }
    }
}
