using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleNetworking.Tools
{
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(Exception error, int clientId)
        {
            Error = error;
            ClientId = clientId;
        }

        public Exception Error { get; private set; }
        public int ClientId { get; private set; }
    }
}
