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
using System.Net;

namespace PythonRouge.Server
{
    public class Program
    {
        public static Map map = new Map(70, 50, null);
        public static Dictionary<string, Player> Players = new Dictionary<string, Player>();
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

        private static void SendAll(NetOutgoingMessage msg)
        {
            foreach (NetConnection client in Server.Connections)
            {
                Server.SendMessage(msg, client, NetDeliveryMethod.ReliableOrdered);
            }
        }

        private static void msgHandler(NetIncomingMessage msg)
        {
            var code = msg.ReadInt32();
            switch (code)
            {
                case 1:
                {
                    var outMsg = Server.CreateMessage();
                    string contents = map.grid.mapTostring();
                    //Console.WriteLine(contents);
                    outMsg.Write(1);
                    outMsg.Write(contents);
                    Server.SendMessage(outMsg, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    break;
                }
                case 2:
                {
                    var name = msg.ReadString();
                    var x = msg.ReadInt32();
                    var y = msg.ReadInt32();
                    var newPos = new EntityPos(x, y);
                    Players[name].pos = newPos;
                    var outMsg = Server.CreateMessage();
                    outMsg.Write(2);
                    outMsg.Write(name);
                    outMsg.Write(x);
                    outMsg.Write(y);
                    SendAll(outMsg);
                    break;
                }
                case 3:
                {
                    var name = msg.ReadString();
                    var x = msg.ReadInt32();
                    var y = msg.ReadInt32();
                    Players[name] = new Player(x, y, 100, '@', name);
                    break;
                }

                default:
                    break;
            }

        }
    }
}
