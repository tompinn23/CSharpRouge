using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
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
            int tilesX = cellsX * cellSize;
            int tilesY = cellsY * cellSize;
            List<List<Node>> grid = ConstructGrid(tilesX, tilesY);
            Astar A = new Astar(grid);
            foreach (Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>> bcc in connections.Values)
            {
                var start = bcc.Item1[rnd.Next(bcc.Item1.Count)];
                var end = bcc.Item2[rnd.Next(bcc.Item2.Count)];
                var ab = bcc.Item1;
                var bb = bcc.Item2;
                List<Tuple<int, int>> corridor = new List<Tuple<int, int>>();
                foreach (Node node in A.FindPath(new Vector2(start.Item1, start.Item2),new Vector2(end.Item1, end.Item2)))
                {
                    Tuple<int, int> tile = new Tuple<int, int>((int)node.Position.X, (int)node.Position.Y);
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
            for (int x = 0; x < tilesX; x++)
            {
                for (int y = 0; y < tilesY; y++)
                {
                    Tuple<int, int> key = new Tuple<int, int>(x, y);
                    if(tiles[key] != "." && getNeighbourTiles(key, tiles).Contains("."))
                {
                        tiles[key] = "#";
                    }
                }
            }
            tiles[stairsUp] = "<";
            tiles[stairsDown] = ">";
            Console.WriteLine("Done");
            for(int y = 0;y < tilesY; y++)
            {
                for(int x = 0;x < tilesX; x++)
                {
                    Console.Write(tiles[new Tuple<int, int>(x, y)]);
                }
                Console.Write("\n");
            }
                
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
                try
                {
                    neighbourTiles.Add(tiles[new Tuple<int, int>(tx + x, ty + y)]);
                }
                catch(KeyNotFoundException e)
                {
                    continue;
                }
                
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

        public class Node
        {
            // Change this depending on what the desired size is for each element in the grid
            public static int NODE_SIZE = 1;
            public Node Parent;
            public Vector2 Position;
            public Vector2 Center
            {
                get
                {
                    return new Vector2(Position.X + NODE_SIZE / 2, Position.Y + NODE_SIZE / 2);
                }
            }
            public float DistanceToTarget;
            public float Cost;
            public float F
            {
                get
                {
                    if (DistanceToTarget != -1 && Cost != -1)
                        return DistanceToTarget + Cost;
                    else
                        return -1;
                }
            }
            public bool Walkable;

            public Node(Vector2 pos, bool walkable)
            {
                Parent = null;
                Position = pos;
                DistanceToTarget = -1;
                Cost = 1;
                Walkable = walkable;
            }
        }

        private List<List<Node>> ConstructGrid(int width, int height)
        {
            List<List<Node>> temp = new List<List<Node>>();
            bool walkable = true;
            float tempX = 0;
            float tempY = 0;

            for (int i = 0; i < width; i++)
            {
                temp.Add(new List<Node>());
                for (int j = 0; j < height; j++)
                {
                    temp[i].Add(new Node(new Vector2(i, j), walkable));
                    tempX += 32;
                }
                tempX = 0;
                tempY += 32;
            }
            return temp;
        }

        public class Astar
        {
            List<List<Node>> Grid;
            int GridRows
            {
                get
                {
                    return Grid[0].Count;
                }
            }
            int GridCols
            {
                get
                {
                    return Grid.Count;
                }
            }

            public Astar(List<List<Node>> grid)
            {
                Grid = grid;
            }

            public Stack<Node> FindPath(Vector2 Start, Vector2 End)
            {
                Node start = new Node(new Vector2((int)(Start.X / Node.NODE_SIZE), (int)(Start.Y / Node.NODE_SIZE)), true);
                Node end = new Node(new Vector2((int)(End.X / Node.NODE_SIZE), (int)(End.Y / Node.NODE_SIZE)), true);

                Stack<Node> Path = new Stack<Node>();
                List<Node> OpenList = new List<Node>();
                List<Node> ClosedList = new List<Node>();
                List<Node> adjacencies;
                Node current = start;

                // add start node to Open List
                OpenList.Add(start);

                while (OpenList.Count != 0 && !ClosedList.Exists(x => x.Position == end.Position))
                {
                    current = OpenList[0];
                    OpenList.Remove(current);
                    ClosedList.Add(current);
                    adjacencies = GetAdjacentNodes(current);


                    foreach (Node n in adjacencies)
                    {
                        if (!ClosedList.Contains(n) && n.Walkable)
                        {
                            if (!OpenList.Contains(n))
                            {
                                n.Parent = current;
                                n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                                n.Cost = 1 + n.Parent.Cost;
                                OpenList.Add(n);
                                OpenList = OpenList.OrderBy(node => node.F).ToList<Node>();
                            }
                        }
                    }
                }

                // construct path, if end was not closed return null
                if (!ClosedList.Exists(x => x.Position == end.Position))
                {
                    return null;
                }

                // if all good, return path
                Node temp = ClosedList[ClosedList.IndexOf(current)];
                while (temp.Parent != start && temp != null)
                {
                    Path.Push(temp);
                    temp = temp.Parent;
                }
                return Path;
            }

            private List<Node> GetAdjacentNodes(Node n)
            {
                List<Node> temp = new List<Node>();

                int row = (int)n.Position.Y;
                int col = (int)n.Position.X;

                if (row + 1 < GridRows)
                {
                    temp.Add(Grid[col][row + 1]);
                }
                if (row - 1 >= 0)
                {
                    temp.Add(Grid[col][row - 1]);
                }
                if (col - 1 >= 0)
                {
                    temp.Add(Grid[col - 1][row]);
                }
                if (col + 1 < GridCols)
                {
                    temp.Add(Grid[col + 1][row]);
                }

                return temp;
            }
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
