using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleNetworking
{

    public class Packet : IPacket
    {
        public const int HEADERSOFFSET = 4 + 8 + 8 + 4;

        private byte[] headers = new byte[HEADERSOFFSET];
        private byte[] data = new byte[0];

        public int PacketTypeId
        {
            get
            {
                int offset = 0;
                int length = 4;
                return BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
            }
            set
            {
                int offset = 0;
                byte[] newBytes = BitConverter.GetBytes(value);
                for(int i = 0; i < newBytes.Length; i++)
                {
                    headers[i+offset] = newBytes[i];
                }
            }
        }
        public DateTime Sent
        {
            get
            {
                int offset = 4;
                int length = 8;
                byte[] bytes = headers.Skip(offset).Take(length).ToArray();
                long serializedDateTime = BitConverter.ToInt64(bytes);
                return DateTime.FromBinary(serializedDateTime);
            }
            set
            {
                int offset = 4;
                byte[] newBytes = BitConverter.GetBytes(value.ToBinary());
                for (int i = 0; i < newBytes.Length; i++)
                {
                    headers[i + offset] = newBytes[i];
                }
            }
        }
        public DateTime Received
        {
            get
            {
                int offset = 4 + 8;
                int length = 8;
                byte[] bytes = headers.Skip(offset).Take(length).ToArray();
                long serializedDateTime = BitConverter.ToInt64(bytes);
                return DateTime.FromBinary(serializedDateTime);
            }
            set
            {
                int offset = 4 + 8;
                byte[] newBytes = BitConverter.GetBytes(value.ToBinary());
                for (int i = 0; i < newBytes.Length; i++)
                {
                    headers[i + offset] = newBytes[i];
                }
            }
        }
        public int ClientId
        {
            get
            {
                int offset = 4 + 8 + 8;
                int length = 4;
                return BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
            }
            set
            {
                int offset = 4 + 8 + 8;
                byte[] newBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < newBytes.Length; i++)
                {
                    headers[i + offset] = newBytes[i];
                }
            }
        }
        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        public byte[] AllBytes
        {
            get
            {
                //PacketLength = headers.Length + data.Length;
                return headers.Concat(data).ToArray();
            }
            set
            {
                this.headers = value.Take(HEADERSOFFSET).ToArray();
                this.data = value.Skip(HEADERSOFFSET).ToArray();
            }
        }
        public Packet()
        {
            //data = new List<byte>();
        }



    }
}
