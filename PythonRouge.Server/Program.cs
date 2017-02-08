// CSharpRouge Copyright (C) {2017} {Tom Pinnock}
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
//  details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see
// http://www.gnu.org/licenses/.
using System;
using Lidgren.Network;
using PythonRouge.game;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PythonRouge.Server
{
    public class Program
    {
        public static Map map = new Map(70, 50, null);
        public static Dictionary<string, Player> Players = new Dictionary<string, Player>();
        public static IFormatter Formatter = new BinaryFormatter();
        public static NetPeer Server;


        private static void Main(string[] args)
        {
            map.generate();
            Console.WriteLine(("Enter Server name:"));
            var name = Console.ReadLine();
            var config = new NetPeerConfiguration("PythonRouge") {Port = 32078};
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            Server = new NetServer(config);
            Server.Start();
            Console.WriteLine("Ready");
            NetIncomingMessage msg;
            while (true)
            {
                while ((msg = Server.ReadMessage()) != null)
                {
                    Console.WriteLine("Got Message");
                    switch (msg.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            NetConnectionStatus status = (NetConnectionStatus) msg.ReadByte();
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
                            NetOutgoingMessage response = Server.CreateMessage();
                            Console.WriteLine("Client request");
                            response.Write(name);
                            // Send the response to the sender of the request
                            Server.SendDiscoveryResponse(response, msg.SenderEndPoint);
                            break;
                        default:
                            Console.WriteLine("Unhandled type: " + msg.MessageType);
                            break;

                    }
                    Server.Recycle(msg);
                }
            }
        }

        private static void msgHandler(NetIncomingMessage msg)
        {
            var code = msg.ReadInt32();
            switch (code)
            {
                case 34:
                {
                    Console.WriteLine(map.grid.mapTostring());
                    //outMsg.Write(45);
                    //outMsg.Write(b);
                    //Server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    break;
                }
            }

        }
    }
}
