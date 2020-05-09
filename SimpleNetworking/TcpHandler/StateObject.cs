using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SimpleNetworking
{
    public class StateObject
    {
        //private bool disposed = false;
        private int? packetLength = null;
        private byte[] data = new byte[0];
        private bool isReceiveComplete = false;
        private bool isResolved = false;

        public StateObject(int bufferSize)
        {
            Buffer = new byte[bufferSize];
        }

        //public int BufferSize { get; set; }
        /// <summary>
        /// Buffer for the stream.
        /// </summary>
        public byte[] Buffer { get; set; }
        /// <summary>
        /// Contains all the data that belongs to the send instance. This is ready to use when Resolve method returns true.
        /// </summary>
        public byte[] Data
        {
            get
            {
                if(data.Length < 4)
                {
                    return new byte[0];
                }
                return data.Skip(4).ToArray();
            }

            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Contains bytes that belongs to the next packet after a resolve has been run. This is ready to use when Resolve method returns true.
        /// </summary>
        public byte[] RestData { get; private set; } = new byte[0];

        /// <summary>
        /// Resolves the received bytes
        /// </summary>
        /// <returns>Returns true if data is completly received.</returns>
        public bool Resolve(int byteLength)
        {
            if (!isResolved)
            {
                isResolved = true;
                byte[] readBytes = new byte[byteLength];
                Array.Copy(Buffer, readBytes, byteLength);
                Data = data.Concat(readBytes).ToArray();
                ResetBuffer();

                if (packetLength == null)
                {
                    if (data.Length < 4)
                    {
                        return isReceiveComplete;
                    }
                    packetLength = BitConverter.ToInt32(data.Take(4).ToArray());
                }

                if (packetLength <= Data.Length)
                {
                    RestData = Data.Skip(packetLength.Value).ToArray();
                    Data = data.Take(packetLength.Value + 4).ToArray();
                    isReceiveComplete = true;
                    return isReceiveComplete;
                }
            }
            return isReceiveComplete;
        }
        private void ResetBuffer()
        {
            Buffer = new byte[0];
        }

    }
}
