// CSharpRouge Copyright (C) 2017 Tom Pinnock
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
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Lidgren.Network;
using PythonRouge.game;
using RLNET;

namespace PythonRouge.network
{
    internal class MpEngine
    {
        internal NetClient Client;
        internal RLConsole InvConsole = new RLConsole(20, 70);
        internal Map map;

        internal RLConsole MapConsole = new RLConsole(70, 50);

        internal Player Player = new Player(0, 0, 100, '@', "Tom");

        internal RLRootConsole RootConsole;

        internal bool WantMap = true;
        internal bool InitFin = false;
        internal bool mapReady = false;

        public static byte[] StringToByteArray(string hex) 
        {
            return Enumerable.Range(0, hex.Length)
                     .Where(x => x % 2 == 0)
                     .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                     .ToArray();
        }
        internal MpEngine(RLRootConsole rootConsole)
        {
            var config = new NetPeerConfiguration("PythonRouge");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            Client = new NetClient(config);
            Client.Start();
            Console.WriteLine("Discovering local peers");
            Client.Connect("127.0.0.1", 32078);
            //Client.DiscoverLocalPeers(32078);
            this.RootConsole = rootConsole;
            MapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            InvConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
            //var pos = Map.findPPos();
            //Player.pos = pos;
            InitFin = true;
        }

        internal void Render()
        {
            PreRender();
            RLConsole.Blit(MapConsole, 0, 0, 70, 50, RootConsole, 0, 10);
            RLConsole.Blit(InvConsole, 0, 0, 20, 70, RootConsole, 70, 0);
            PostRender();
        }

        internal void Update()
        {
            MsgReader();
            if (WantMap && InitFin && Client.ConnectionStatus == NetConnectionStatus.Connected)
            {
                WantMap = false;
                RequestMap();
            }
        }

        internal void RequestMap()
        {
            Console.WriteLine("Requesting Map");
            var msg = Client.CreateMessage();
            msg.Write(34);
            Client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
        }

        internal void MsgReader()
        {
            NetIncomingMessage msg;
            while ((msg = Client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
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
                        MsgHandler(msg);
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Found server at " + msg.SenderEndPoint + " name: " + msg.ReadString());
                        break;
                    default:
                        Console.WriteLine("Unhandled type: " + msg.MessageType);
                        break;
                }
            }
        }

        internal void MsgHandler(NetIncomingMessage msg)
        {
            var code = msg.ReadInt32();
            switch (code)
            {
                case 45:
                    var c = msg.ReadString();
                    //Console.WriteLine(c);
                    map = new Map(70, 50, null);
                    map.grid.mapFromString(c);
                    mapReady = true;
                    break;
            }
        }

        internal void RenderMap()
        {
            var game_map = map.grid.Game_map;
            foreach (var kvp in game_map)
            {
                var pos = kvp.Key;
                var tile = kvp.Value;
                switch (tile.type)
                {
                    case TileType.Floor:
                        if (tile.lit)
                            MapConsole.Set(pos.Item1, pos.Item2, Colours.floor_lit, Colours.floor_lit, tile.symbol);
                        else
                            MapConsole.Set(pos.Item1, pos.Item2, Colours.floor, Colours.floor, tile.symbol);
                        break;
                    case TileType.Wall:
                        if (tile.lit)
                            MapConsole.Set(pos.Item1, pos.Item2, Colours.wall_lit, Colours.wall_lit, tile.symbol);
                        else
                            MapConsole.Set(pos.Item1, pos.Item2, Colours.wall, Colours.wall, tile.symbol);
                        break;
                    case TileType.Empty:
                        MapConsole.Set(pos.Item1, pos.Item2, RLColor.Black, RLColor.Black, tile.symbol);
                        break;
                    default:
                        break;
                }
            }
        }


        internal void PreRender()
        {
            if (mapReady)
            {
                RenderMap();
            }

            Player.draw(MapConsole);
        }

        internal void PostRender()
        {
            Player.clear(MapConsole);
        }


        internal void HandleKey(RLKeyPress keyPress)
        {
            switch (keyPress.Key)
            {
                case RLKey.Up:
                    {
                        if (!map.canMove(Player.pos, 0, -1)) return;
                        map.resetLight();
                        Player.move(0, -1);
                        var pos = new Vector2(Player.pos.x, Player.pos.y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                    }
                    break;
                case RLKey.Down:
                    {
                        if (!map.canMove(Player.pos, 0, 1)) return;
                        map.resetLight();
                        Player.move(0, 1);
                        var pos = new Vector2(Player.pos.x, Player.pos.y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                    }
                    break;
                case RLKey.Left:
                    {
                        if (!map.canMove(Player.pos, -1, 0)) return;
                        map.resetLight();
                        Player.move(-1, 0);
                        var pos = new Vector2(Player.pos.x, Player.pos.y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                    }
                    break;
                case RLKey.Right:
                    {
                        if (!map.canMove(Player.pos, 1, 0)) return;
                        map.resetLight();
                        Player.move(1, 0);
                        var pos = new Vector2(Player.pos.x, Player.pos.y);
                        ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}