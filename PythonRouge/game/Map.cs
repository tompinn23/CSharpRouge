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
using System.Collections.Generic;
using Karcero.Engine;
using Karcero.Engine.Models;

namespace PythonRouge.game
{
    public class Map
    {
        public GameGrid grid;

        public Map(int width, int height, GameGrid grid = null)
        {
            mapHeight = height;
            mapWidth = width;
            if (grid == null)
                this.grid = new GameGrid(width, height);
            else
                this.grid = grid;
            fillMap();
        }

        public int mapWidth { get; set; }
        public int mapHeight { get; set; }

        public void fillMap()
        {
            for (var x = 0; x < mapWidth; x++)
            for (var y = 0; y < mapHeight; y++)
                grid.Game_map[new Tuple<int, int>(x, y)] = new Tile(x, y, TileType.Wall);
        }

        public void resetLight()
        {
            foreach (var kvp in grid.Game_map)
                kvp.Value.lit = false;
        }

        public bool canMove(EntityPos pos, int dx, int dy)
        {
            if (grid.Game_map[new Tuple<int, int>(pos.x + dx, pos.y + dy)].type == TileType.Wall)
                return false;
            return true;
        }

        public EntityPos findPPos()
        {
            foreach (var kvp in grid.Game_map)
                if (kvp.Value.type == TileType.Floor)
                    return new EntityPos(kvp.Key.Item1, kvp.Key.Item2);
            return new EntityPos(0, 0);
        }

        public bool NeighboursIsNotFloor(Tuple<int, int> pos)
        {
            var offsets = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(-1, -1),
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(-1, 1),
                new Tuple<int, int>(0, -1),
                new Tuple<int, int>(0, 1),
                new Tuple<int, int>(1, -1),
                new Tuple<int, int>(1, 0),
                new Tuple<int, int>(1, 1)
            };
            foreach (var offset in offsets)
                try
                {
                    if (grid.Game_map[new Tuple<int, int>(pos.Item1 + offset.Item1, pos.Item2 + offset.Item2)].type ==
                        TileType.Floor)
                        return false;
                }
#pragma warning disable 0168
                catch (KeyNotFoundException e)
#pragma warning restore 0168
                {
                }
            return true;
        }

        public void setEmpty()
        {
            foreach (var kvp in grid.Game_map)
                if (NeighboursIsNotFloor(new Tuple<int, int>(kvp.Key.Item1, kvp.Key.Item2)))
                    kvp.Value.type = TileType.Empty;
        }

        public void generate()
        {
            var generator = new DungeonGenerator<Cell>();
            var map = generator.GenerateA()
                .DungeonOfSize(69, 49)
                .ABitRandom()
                .VerySparse()
                .WithBigChanceToRemoveDeadEnds()
                .RemoveAllDeadEnds()
                .WithRoomSize(3, 10, 3, 10)
                .WithRoomCount(35)
                .Now();
            foreach (var cell in map.AllCells)
            {
                var pos = new Tuple<int, int>(cell.Column, cell.Row);
                switch (cell.Terrain)
                {
                    case TerrainType.Door:
                        grid.Game_map[pos].blocked = false;
                        grid.Game_map[pos].block_sight = false;
                        grid.Game_map[pos].type = TileType.Floor;
                        break;
                    case TerrainType.Floor:
                        grid.Game_map[pos].blocked = false;
                        grid.Game_map[pos].block_sight = false;
                        grid.Game_map[pos].type = TileType.Floor;
                        break;
                    case TerrainType.Rock:
                        grid.Game_map[pos].blocked = true;
                        grid.Game_map[pos].block_sight = true;
                        grid.Game_map[pos].type = TileType.Wall;
                        break;
                }
            }
            setEmpty();
        }
    }

    [Serializable]
    public class GameGrid
    {
        private Dictionary<Tuple<int, int>, Tile> game_map = new Dictionary<Tuple<int, int>, Tile>();
        public int xDim;
        public int yDim;

        public GameGrid(int w, int h)
        {
            xDim = w;
            yDim = h;
        }

        internal Dictionary<Tuple<int, int>, Tile> Game_map { get { return game_map; } set { game_map  = value; } }

        public bool IsWall(int x, int y)
        {
            return Game_map[new Tuple<int, int>(x, y)].type == TileType.Wall;
        }

        public void SetLight(int x, int y, float disSqrd)
        {
            Game_map[new Tuple<int, int>(x, y)].lit = true;
        }

        public string mapTostring()
        {
            string[] temp = new string[game_map.Count];
            int counter = 0;
            foreach (KeyValuePair<Tuple<int, int>, Tile> kvp in Game_map)
            {
                string xy = kvp.Key.Item1.ToString() + "/" + kvp.Key.Item2.ToString();
                string type = kvp.Value.ToString();
                string part = xy + ":" + type;
                temp[counter] = part;
            }
            return string.Join(",", temp);
        }

    }


    public struct Vector2
    {
        public int x;
        public int y;

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    [Serializable]
    internal class Tile
    {
        public char symbol = ' ';

        public Tile(int x, int y, TileType type)
        {
            this.x = x;
            this.y = y;
            switch (type)
            {
                case TileType.Floor:
                    blocked = false;
                    block_sight = false;
                    break;
                case TileType.Empty:
                    blocked = true;
                    block_sight = true;
                    break;
                case TileType.Wall:
                    blocked = true;
                    block_sight = true;
                    break;
            }
            lit = false;
            this.type = type;
        }

        public int x { get; set; }
        public int y { get; set; }
        public bool blocked { get; set; }
        public bool block_sight { get; set; }
        public bool lit { get; set; }
        public TileType type { get; set; }

        public void setType(TileType type)
        {
            switch (type)
            {
                case TileType.Floor:
                    blocked = false;
                    block_sight = false;
                    break;
                case TileType.Empty:
                    blocked = true;
                    block_sight = true;
                    break;
                case TileType.Wall:
                    blocked = true;
                    block_sight = true;
                    break;
            }
            this.type = type;
        }
    }

    internal enum TileType
    {
        Floor,
        Wall,
        Empty
    }
}