using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Xunit;

namespace SimpleNetworking.Tests.Tests
{
    public class StateObjectTests
    {
        [Fact]
        public void TotalTest()
        {
            Packet packet = new Packet
            {
                ClientId = 11,
                Sent = new DateTime(2020, 01, 01, 0, 0, 0),
                Received = new DateTime(2020, 01, 01, 0, 0, 2),
                PacketTypeId = 1,
                Data = BitConverter.GetBytes(255)
            };
            byte[] expectedData = packet.AllBytes;
            byte[] packetLength = BitConverter.GetBytes(expectedData.Length);
            byte[] expectedDataToSend = packetLength.Concat(expectedData).ToArray();

            byte[] expectedRestBytes = BitConverter.GetBytes(5).ToArray();
            byte[] receivedData = expectedDataToSend.Concat(expectedRestBytes).ToArray();
            
            //byte[] receivedData = packetLength.Concat(dataToSend).ToArray();

            StateObject state = new StateObject(4096) 
            {
                Buffer = receivedData
            };
            state.Resolve(receivedData.Length);

            byte[] actualRestBytes = state.RestData;
            byte[] actualData = state.Data;

            Assert.True(state.Resolve(receivedData.Length));
            Assert.Equal(expectedData, actualData);
            Assert.Equal(expectedRestBytes, actualRestBytes);

        }

    }
}
