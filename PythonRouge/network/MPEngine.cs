using System;
using Lidgren.Network;
using RLNET;
using PythonRouge.game;
using System.Collections.Generic;

namespace PythonRouge.network
{
    internal class MPEngine
    {
        internal NetClient client;

        internal RLRootConsole rootConsole;

        internal RLConsole mapConsole = new RLConsole(70, 50);
        internal RLConsole invConsole = new RLConsole(20, 70);

        internal Player player = new Player(0, 0, 100, '@', "Tom");
        internal Map map = new Map(70, 50);

        internal MPEngine(RLRootConsole rootConsole)
        {
            var config = new NetPeerConfiguration("PythonRouge");
            config.Port = 32078;
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            client = new NetClient(config);

            this.rootConsole = rootConsole;
            mapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            invConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
            var pos = map.findPPos();
            player.pos = pos;
        }

        internal void render()
        {
            startUpdate();
            RLConsole.Blit(mapConsole, 0, 0, 70, 50, this.rootConsole, 0, 10);
            RLConsole.Blit(invConsole, 0, 0, 20, 70, this.rootConsole, 70, 0);
            endUpdate();
        }

        internal void renderMap()
        {
            var game_map = map.grid.Game_map;
            foreach (KeyValuePair<Tuple<int, int>, Tile> kvp in game_map)
            {
                Tuple<int, int> pos = kvp.Key;
                Tile tile = kvp.Value;
                switch (tile.type)
                {
                    case TileType.Floor:
                        if (tile.lit)
                        {
                            mapConsole.Set(pos.Item1, pos.Item2, Colours.floor_lit, Colours.floor_lit, tile.symbol);
                        }
                        else
                        {
                            mapConsole.Set(pos.Item1, pos.Item2, Colours.floor, Colours.floor, tile.symbol);
                        }
                        break;
                    case TileType.Wall:
                        if (tile.lit)
                        {
                            mapConsole.Set(pos.Item1, pos.Item2, Colours.wall_lit, Colours.wall_lit, tile.symbol);
                        }
                        else
                        {
                            mapConsole.Set(pos.Item1, pos.Item2, Colours.wall, Colours.wall, tile.symbol);
                        }
                        break;
                    case TileType.Empty:
                        mapConsole.Set(pos.Item1, pos.Item2, RLColor.Black, RLColor.Black, tile.symbol);
                        break;
                }


            }
        }


        internal void startUpdate()
        {
            w
            renderMap();
            player.draw(mapConsole);
        }
        internal void endUpdate()
        {
            player.clear(mapConsole);
        }


        internal void handleKey(RLKeyPress keyPress)
        {
            if (keyPress.Key == RLKey.Up)
            {
                if (map.canMove(player.pos, 0, -1))
                {
                    map.resetLight();
                    player.move(0, -1);
                    Vector2 pos = new Vector2(player.pos.x, player.pos.y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Down)
            {
                if (map.canMove(player.pos, 0, 1))
                {
                    map.resetLight();
                    player.move(0, 1);
                    Vector2 pos = new Vector2(player.pos.x, player.pos.y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Left)
            {
                if (map.canMove(player.pos, -1, 0))
                {
                    map.resetLight();
                    player.move(-1, 0);
                    Vector2 pos = new Vector2(player.pos.x, player.pos.y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
            else if (keyPress.Key == RLKey.Right)
            {
                if (map.canMove(player.pos, 1, 0))
                {
                    map.resetLight();
                    player.move(1, 0);
                    Vector2 pos = new Vector2(player.pos.x, player.pos.y);
                    ShadowCast.ComputeVisibility(map.grid, pos, 7.5f);
                }
            }
        }

    }
}