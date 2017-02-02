using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    class mapGenerator
    {
        public Dictionary<Tuple<int, int>, string> generate(int cellsX, int cellsY,  int cellSize = 5)
        {
            Dictionary<Tuple<int, int>, Cell> cells = new Dictionary<Tuple<int, int>, Cell>();
            List<Tuple<int, int>> celllist = new List<Tuple<int, int>>();
            Random rnd = new Random(); 
            for (int y = 0; y < cellsY; y++)
            {
                for (int x = 0; x < cellsX; x++)
                {
                    Cell cd = new Cell(x, y, cells.Count);
                    cells.Add(new Tuple<int, int>(cd.x, cd.y), cd);
                }
            }
            celllist = cells.Keys.ToList();
            Cell current = cells[celllist[rnd.Next(celllist.Count)]];
            Cell lastCell = current;
            Cell firstCell = current;
            current.connected = true;
            while (true)
            {
                List<Cell> unconnected = new List<Cell>();
                foreach (Cell cell in getNeighbourCells(current, cells))
                {
                    if (cell.connected == false)
                    {
                        unconnected.Add(cell);
                    }
                }
                if (unconnected.Count == 0)
                {
                    break;
                }
                var neighbour = unconnected[rnd.Next(unconnected.Count)];
                current.connect(neighbour);
            }
            Console.WriteLine("done 1");
            var stop = false;
            while (stop == false)
            {
                foreach (Cell x in cells.Values)
                {
                    if (x.connected == false)
                    {
                        stop = true;
                    }
                }
                List<Tuple<Cell, List<Cell>>> candidates = new List<Tuple<Cell, List<Cell>>>();
                foreach (Cell x in cells.Values)
                {
                    if (!x.connected)
                    {
                        List<Cell> neighbours = new List<Cell>();
                        foreach (Cell cell in getNeighbourCells(x, cells))
                        {
                            if (cell.connected)
                            {
                                neighbours.Add(cell);
                            }
                            if (neighbours.Count == 0)
                            {
                                continue;
                            }
                        }
                        candidates.Add(new Tuple<Cell, List<Cell>>(x, neighbours));
                    }
                    Tuple<Cell, List<Cell>> cellneighbour = candidates[rnd.Next(candidates.Count)];
                    cellneighbour.Item1.connect(cellneighbour.Item2[rnd.Next(cellneighbour.Item2.Count)]);
                }
            }
            Console.WriteLine("done 2");
            var a = (cellsX + cellsY) / 4;
            var b = (cellsX + cellsY) / 1.2;
            b = Math.Round(b);
            var c = Convert.ToInt32(b);
            var extraConnections = rnd.Next(a, c);
            int maxRetries = 10;
            while (extraConnections > 0 && maxRetries > 0)
            {
                Cell cell = cells[celllist[rnd.Next(celllist.Count)]];
                var neighbours = getNeighbourCells(cell, cells);
                var neighbour = neighbours[rnd.Next(neighbours.Count)];
                if (neighbour.connectedTo.Contains(cell))
                {
                    maxRetries--;
                }
                cell.connect(neighbour);
                extraConnections--;
            }
            Console.WriteLine("done 3");
            List<List<Tuple<int, int>>> rooms = new List<List<Tuple<int, int>>>();
            foreach (Cell k in cells.Values)
            {
                var w = rnd.Next(3, cellSize - 2);
                var h = rnd.Next(3, cellSize - 2);
                var x = (k.x * cellSize) + rnd.Next(1, cellSize - w - 1);
                var y2 = (k.y * cellSize) + rnd.Next(1, cellSize - h - 1);
                List<Tuple<int, int>> floorTiles = new List<Tuple<int, int>>();
                for (int i = 0; i < w; i++)
                {
                    for (int j = 0; j < h; j++)
                    {
                        floorTiles.Add(new Tuple<int, int>(x + i, y2 + j));
                    }
                }
                k.room = floorTiles;
                rooms.Add(floorTiles);
            }
            Console.WriteLine("done 4");
            Dictionary<Tuple<int, int>, Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>>> connections = new Dictionary<Tuple<int, int>, Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>>>();

            foreach (Cell cell in cells.Values)
            {
                foreach (Cell other in cell.connectedTo)
                {
                    int[] ids = { cell.id, other.id };
                    Array.Sort(ids);
                    connections[new Tuple<int, int>(ids[0], ids[1])] = new Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>>(cell.room, other.room);
                }
            }
            foreach (Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>> bcc in connections.Values)
            {
                var start = bcc.Item1[rnd.Next(bcc.Item1.Count)];
                var end = bcc.Item2[rnd.Next(bcc.Item2.Count)];
                var ab = bcc.Item1;
                var bb = bcc.Item2;
                List<Tuple<int, int>> corridor = new List<Tuple<int, int>>();

                foreach (Tuple<int, int> tile in _AStar(start, end))
                {
                    if (!ab.Contains(tile) && !bb.Contains(tile))
                    {
                        corridor.Add(tile);
                    }
                }
                rooms.Add(corridor);
            }
            Console.WriteLine("done 5");
            var stairsUp = firstCell.room[rnd.Next(firstCell.room.Count)];
            var stairsDown = lastCell.room[rnd.Next(lastCell.room.Count)];

            Dictionary<Tuple<int, int>, string> tiles = new Dictionary<Tuple<int, int>, string>();
            int tilesX = cellsX * cellSize;
            int tilesY = cellsY * cellSize;
            for(int x = 0;x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    tiles[new Tuple<int, int>(x, y)] = " ";
                }
            }
            foreach(List<Tuple<int, int>> room in rooms)
            {
                foreach(Tuple<int, int> xy in room)
                {
                    tiles[xy] = ".";
                }
            }
            foreach(KeyValuePair<Tuple<int, int>, string> kvp in tiles)
            {
                if(kvp.Value != "." && getNeighbourTiles(kvp.Key, tiles).Contains("."))
                {
                    tiles[kvp.Key] = "#";
                }
            }
            tiles[stairsUp] = "<";
            tiles[stairsDown] = ">";
            Console.WriteLine("Done");
            return tiles;
        }

        private List<string> getNeighbourTiles(Tuple<int, int> xy, Dictionary<Tuple<int, int>, string> tiles)
        {
            List<string> neighbourTiles = new List<string>();
            var tx = xy.Item1;
            var ty = xy.Item2;
            List<Tuple<int, int>> offsets = new List<Tuple<int, int>> {
                new Tuple<int, int>(-1, -1), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, -1),
                new Tuple<int, int>(-1, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(-1, 1),
                new Tuple<int, int>(0, 1), new Tuple<int, int>(1, 1)};
            foreach(Tuple<int, int> offset in offsets)
            {
                var x = offset.Item1;
                var y = offset.Item2;
                neighbourTiles.Add(tiles[new Tuple<int, int>(tx + x, ty + y)]);
            }
            return neighbourTiles;
        }

        private List<Cell> getNeighbourCells(Cell cell, Dictionary<Tuple<int, int>, Cell> cells)
        {
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>() { new Tuple<int, int>(-1, 0), new Tuple<int, int>(0, -1), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, 1) };
            List<Cell> neighbourCells = new List<Cell>();
            foreach (Tuple<int, int> neighbour in neighbours)
            {
                try
                {
                    neighbourCells.Add(cells[neighbour]);
                }
                catch (KeyNotFoundException e)
                {
                    continue;
                }
            }
            return neighbourCells;
        }


        private List <Tuple<int, int>> _AStar(Tuple<int, int> start, Tuple<int, int> goal)
        {
            List<Tuple<int, int>> closed = new List<Tuple<int, int>>();
            List<Tuple<int, int>> open = new List<Tuple<int, int>>();
            open.Add(start);
            Dictionary<Tuple<int, int>, Tuple<int, int>> cameFrom = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
            Dictionary<Tuple<int, int>, int> gScore = new Dictionary<Tuple<int, int>, int>();
            gScore.Add(start, 0);
            Dictionary<Tuple<int, int>, int> fScore = new Dictionary<Tuple<int, int>, int>();
            fScore.Add(start, heuristic(start, goal));
            Tuple<int, int> current = null;
            while (open.Count > 0)
            {
                current = null;
                foreach(Tuple<int, int> i in open)
                {
                   if (current == null || fScore[i] < fScore[current] )
                    {
                        current = i;
                    }

                }
                if(current == goal)
                {
                    return reconstructPath(goal, start, cameFrom);
                }
                open.Remove(current);
                closed.Add(current);

                foreach(Tuple<int, int> neighbor in neighbours(current))
                {
                    if(closed.Contains(neighbor))
                    {
                        continue;
                    }
                    var g = gScore[current] + 1;

                    if(!open.Contains(neighbor) || g < gScore[neighbor] )
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = g;
                        fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, goal);
                        if(!open.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                    }
                }
            }
            return new List<Tuple<int, int>>();
        }

        private int heuristic(Tuple<int, int> a, Tuple<int, int> b)
        {
            int ax = a.Item1;
            int ay = a.Item2;
            int bx = b.Item1;
            int by1 = b.Item2;
            return Math.Abs(ax - bx) + Math.Abs(ay - by1);
        }
        dynamic reconstructPath(Tuple<int, int> n, Tuple<int, int> start, Dictionary<Tuple<int, int>, Tuple<int, int>> cameFrom)
        {
            if(n == start)
            {
                return new List<Tuple<int, int>> { n };
            }
            return reconstructPath(cameFrom[n], start, cameFrom).Add(n);
        }

        List<Tuple<int, int>> neighbours(Tuple<int, int> n)
        {
            int x = n.Item1;
            int y = n.Item2;
            return new List<Tuple<int, int>> { new Tuple<int, int>(x - 1, y), new Tuple<int, int>(x + 1, y), new Tuple<int, int>(x, y - 1), new Tuple<int, int>(x, y + 1) };
        }

    }

    class Cell
    {
        public int x;
        public int y;
        public int id;
        public Boolean connected = false;
        public List<Cell> connectedTo = new List<Cell>();
        public List<Tuple<int, int>> room = new List<Tuple<int, int>>();

        public Cell(int x, int y, int id)
        {
            this.x = x;
            this.y = y;
            this.id = id;
        }

        public void connect(Cell other)
        {
            this.connectedTo.Add(other);
            other.connectedTo.Add(this);
            this.connected = true;
            other.connected = true;
        }
    }

}
