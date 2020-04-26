using SimpleNetworking.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SimpleNetworking.Tests.Tests
{
    public class Server_Client_Tests
    {
        [Theory]
        [ClassData(typeof(ServerSample))]
        public void ConnectToServerTest(ServerSample server, ClientSample client)
        {
            //Arrange
            int port = 8888;
            string host = "localhost";
            string expectedServerString = "Client has recieved a WelcomePacket";
            bool expectedServerBool = true;
            string expectedClientString = "Welcome to the server";

            //Act
            server.Start(port);
            client.Connect(host, port);
            Thread.Sleep(3*1000);

            //Assert
            Assert.Equal(expectedClientString, client.WelcomeString);
            Assert.Equal(expectedServerString, server.WelcomeReceivedMessage);
            Assert.Equal(expectedServerBool, server.IsWelcomeReceived);

        }
    }
}
