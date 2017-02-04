using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karcero.Engine;
using Karcero.Engine.Models;
using RLNET;

namespace PythonRouge.game
{
    public class Map
    {
        public int mapWidth { get; set; }
        public int mapHeight { get; set; }
        public GameGrid grid;

        public Map(int width, int height, GameGrid grid=null)
        {
            this.mapHeight = height;
            this.mapWidth = width;
            if (grid == null)
            {
                this.grid = new GameGrid(width, height);
            }
            else
            {
                this.grid = grid;
            }
            fillMap();
        }
        
        public void fillMap()
        {
            for(int x =0; x < mapWidth; x++)
            {
                for(int y =0; y < mapHeight; y++)
                {
                    grid.Game_map[new Tuple<int, int>(x, y)] = new Tile(x, y, TileType.Wall);
                }
            }
        }
        public void resetLight()
        {
            foreach(KeyValuePair<Tuple<int, int>, Tile> kvp in grid.Game_map)
            {
                kvp.Value.lit = false;
            }
        }
        
        public bool canMove(EntityPos pos, int dx, int dy)
        {
            if(grid.Game_map[new Tuple<int,int> (pos.x + dx, pos.y + dy)].type == TileType.Wall)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public EntityPos findPPos()
        {
            foreach(KeyValuePair<Tuple<int, int>, Tile> kvp in grid.Game_map)
            {
                if(kvp.Value.type == TileType.Floor)
                {
                    return new EntityPos(kvp.Key.Item1, kvp.Key.Item2);
                }
            }
            return new EntityPos(0, 0);
        }

        public bool NeighboursIsNotFloor(Tuple<int, int> pos)
        {
            List<Tuple<int, int>> offsets = new List<Tuple<int, int>> { new Tuple<int, int>(-1, -1), new Tuple<int, int>(-1, 0), new Tuple<int, int>(-1, 1), new Tuple<int, int>(0, -1), new Tuple<int, int>(0, 1), new Tuple<int, int>(1, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(1,1)};
            foreach(Tuple<int,int> offset in offsets)
            {
                try
                {
                    if (grid.Game_map[new Tuple<int, int>(pos.Item1 + offset.Item1, pos.Item2 + offset.Item2)].type == TileType.Floor)
                    {
                        return false;
                    }
                }
#pragma warning disable 0168
                catch(KeyNotFoundException e)
#pragma warning restore 0168
                {
                    continue;
                }
            }
            return true;
        }

        public void setEmpty()
        {
            foreach(KeyValuePair<Tuple<int, int>, Tile> kvp in grid.Game_map)
            {
                if(NeighboursIsNotFloor(new Tuple<int, int>(kvp.Key.Item1, kvp.Key.Item2)))
                {
                    kvp.Value.type = TileType.Empty;
                }
            }
        }

        public void generate()
        {
            
            var generator = new DungeonGenerator<Karcero.Engine.Models.Cell>();
            var map = generator.GenerateA()
                .DungeonOfSize(69, 49)
                .ABitRandom()
                .VerySparse()
                .WithBigChanceToRemoveDeadEnds()
                .RemoveAllDeadEnds()
                .WithRoomSize(3, 10, 3, 10)
                .WithRoomCount(35)
                .Now();
            foreach(Karcero.Engine.Models.Cell cell in map.AllCells)
            {
                var pos = new Tuple<int, int>(cell.Column, cell.Row);
                switch(cell.Terrain)
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

        internal Dictionary<Tuple<int, int>, Tile> Game_map { get => game_map; set => game_map = value; }

        public GameGrid(int w, int h)
        {
            this.xDim = w;
            this.yDim = h;
        }

        public bool IsWall(int x, int y)
        {
            return Game_map[new Tuple<int, int>(x, y)].type == TileType.Wall;
        }

        public void SetLight(int x, int y, float disSqrd)
        {
            Game_map[new Tuple<int, int>(x, y)].lit = true;
        }


    }



    public struct Vector2
    {
        public int x;
        public int y;
        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Tile 
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool blocked { get; set; }
        public bool block_sight { get; set; }
        public bool lit { get; set; }
        public TileType type { get; set; }
        public char symbol = ' ';

        public Tile(int x, int y, TileType type)
        {
            this.x = x;
            this.y = y;
            switch(type)
            {
                case TileType.Floor:
                    this.blocked = false;
                    this.block_sight = false;
                    break;
                case TileType.Empty:
                    this.blocked = true;
                    this.block_sight = true;
                    break;
                case TileType.Wall:
                    this.blocked = true;
                    this.block_sight = true;
                    break;
            }
            this.lit = false;
            this.type = type;

        }

        public void setType(TileType type)
        {
            switch (type)
            {
                case TileType.Floor:
                    this.blocked = false;
                    this.block_sight = false;
                    break;
                case TileType.Empty:
                    this.blocked = true;
                    this.block_sight = true;
                    break;
                case TileType.Wall:
                    this.blocked = true;
                    this.block_sight = true;
                    break;
            }
            this.type = type;
        }

    }

    enum TileType
    {
        Floor,
        Wall,
        Empty,
    }


}
