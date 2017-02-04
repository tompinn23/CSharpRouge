﻿using System;
using System.Collections.Generic;
using RLNET;

namespace PythonRouge.game
{
    class SPEngine
    {
        private RLRootConsole rootConsole;

        private RLConsole mapConsole = new RLConsole(70, 50);
        private RLConsole invConsole = new RLConsole(20, 70);

        private Player player = new Player(0,0,100,'@', "Tom");
        private Map map = new Map(70, 50);

        public SPEngine(RLRootConsole rootConsole)
        {
            this.rootConsole = rootConsole;
            mapConsole.SetBackColor(0, 0, 70, 50, RLColor.Blue);
            invConsole.SetBackColor(0, 0, 20, 70, RLColor.Cyan);
            mapGenerate();
            var pos = map.findPPos();
            player.pos = pos;
        }

        public void render()
        {
            startUpdate();
            RLConsole.Blit(mapConsole, 0, 0, 70, 50, this.rootConsole, 0, 10);
            RLConsole.Blit(invConsole, 0, 0, 20, 70, this.rootConsole, 70, 0);
            endUpdate();
        }

        public void mapGenerate()
        {
            map.generate();
        }

        public void renderMap()
        {
            var game_map = map.grid.Game_map;
            foreach(KeyValuePair<Tuple<int, int>, Tile> kvp in game_map)
            {
                Tuple<int, int> pos = kvp.Key;
                Tile tile = kvp.Value;
                switch (tile.type)
                {
                    case TileType.Floor:
                        if(tile.lit)
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


        public void startUpdate()
        {
            renderMap();
            player.draw(mapConsole);                     
        }
        public void endUpdate()
        {
            player.clear(mapConsole);
        }
        

        public void handleKey(RLKeyPress keyPress)
        {
            if (keyPress.Key == RLKey.Up)
            {
                if(map.canMove(player.pos, 0, -1))
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
