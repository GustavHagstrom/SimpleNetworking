using System;
using System.Linq;

namespace SimpleNetworking
{

    public sealed class Packet
    {
        public const int HEADERSOFFSET = 4 + 4 + 4;

        private byte[] headers = new byte[HEADERSOFFSET];
        private byte[] data = new byte[0];

        public int PacketLength
        {
            get
            {
                
                int offset = 0;
                int length = 4;
                int value = BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
                if(value == 0)
                {
                    return HEADERSOFFSET;
                }
                return BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
            }
            private set
            {
                int offset = 0;
                byte[] newBytes = BitConverter.GetBytes(value);
                for (int i = 0; i < newBytes.Length; i++)
                {
                    headers[i + offset] = newBytes[i];
                }
            }
        }
        public int PacketTypeId
        {
            get
            {
                int offset = 4;
                int length = 4;
                return BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
            }
            set
            {
                int offset = 4;
                byte[] newBytes = BitConverter.GetBytes(value);
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
                int offset = 4 + 4;
                int length = 4;
                return BitConverter.ToInt32(headers.Skip(offset).Take(length).ToArray());
            }
            set
            {
                int offset = 4 + 4;
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
                PacketLength = Bytes.Length;
            }
        }
        public byte[] Bytes
        {
            get
            {
                return headers.Concat(Data).ToArray();
            }
            set
            {
                if(value.Length < HEADERSOFFSET)
                {
                    throw new Exception("Bytes are to few to in order to set Allbytes in packet.");
                }
                this.headers = value.Take(HEADERSOFFSET).ToArray();
                this.Data = value.Skip(HEADERSOFFSET).ToArray();
            }
        }

        public Packet()
        {
            
        }



    }
}
