using System;
using Lidgren.Network;
using PythonRouge.game;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PythonRouge.Server
{
    class Program
    {
        public static Map map = new Map(70, 50, null);
        public static Dictionary<string, Player> players = new Dictionary<string, Player>();
        public static IFormatter formatter = new BinaryFormatter();
        public static NetPeer server;


        static void Main(string[] args)
        {
            map.generate();
            var name = Console.ReadLine();
            NetPeerConfiguration config = new NetPeerConfiguration("PythonRouge");
            config.Port = 32078;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            server = new NetServer(config);

            NetIncomingMessage msg;
            while((msg = server.ReadMessage()) != null)
            {
                switch(msg.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        string reason = msg.ReadString();
                        Console.WriteLine(status + " " + reason);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.Data:
                        msgHandler(msg);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryRequest:
                        // Create a response and write some example data to it
                        NetOutgoingMessage response = server.CreateMessage();
                        response.Write(name);
                        // Send the response to the sender of the request
                        server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;

                }
            }
        }

        private static void msgHandler(NetIncomingMessage msg)
        {
            var code = msg.ReadInt32();
            switch (code)
            {
                case 34:
                    Stream stream = new MemoryStream();
                    formatter.Serialize(stream, map.grid);
                    stream.Position = 0;
                    var sr = new StreamReader(stream);
                    string contents = sr.ReadToEnd();
                    var outMsg = server.CreateMessage();
                    outMsg.Write(45);
                    outMsg.Write(contents);
                    server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    break;
            }

        }
    }
}
