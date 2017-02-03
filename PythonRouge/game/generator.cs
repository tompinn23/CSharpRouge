using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    public class Dungeon
    {
        Dictionary<Tuple<int, int>, Tile> map = new Dictionary<Tuple<int, int>, Tile>();
        List<List<Tuple<int, int>>>



        void generateRoom(int x, int y, int w, int h)
        {
            for(int i =x; i < x+w; i++)
            {
                for(int j = y; j < y+h; j++)
                {
                    map[new Tuple<int, int>(i, j)].type = TileType.Floor;
                }
            }
        }
    }

    internal class Tile
    {
        public int x;
        public int y;
        public TileType type;
        public Tile(int x, int y, TileType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }
    internal enum TileType
    {
        Empty,
        Floor,
        Wall
    }

}
