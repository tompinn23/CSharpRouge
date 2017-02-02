using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PythonRouge.game
{
    class mapGenerator
    {
        public void generate(int cellsX, int cellsY,  int cellSize = 5)
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
                        if (x.connected)
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
                var a = (cellsX + cellsY) / 4;
                var b = (cellsX + cellsY) / 1.2;
                b = Math.Round(b);
                var c = Convert.ToInt32(b);
                var extraConnections = rnd.Next(a, c);
                int maxRetries = 10;
                while (extraConnections > 0 && maxRetries > 0)
                {
                    var skip = false;
                    Cell cell = cells[celllist[rnd.Next(celllist.Count)]];
                    var neighbours = getNeighbourCells(cell, cells);
                    var neighbour = neighbours[rnd.Next(neighbours.Count)];
                    foreach(Cell x in neighbour.connectedTo)
                    {
                        if(x == cell)
                        {
                            maxRetries--;
                            skip = true;
                            break;
                        }
                    }
                    if(skip)
                    {
                        continue;
                    }
                    cell.connect(neighbour);
                    extraConnections--;
                }

                List<List<Tuple<int, int>>> rooms = new List<List<Tuple<int, int>>>();
                foreach(Cell k in cells.Values)
                {
                    var w = rnd.Next(3, cellSize - 2);
                    var h = rnd.Next(3, cellSize - 2);
                    var x = (k.x * cellSize) + rnd.Next(1, cellSize - w - 1);
                    var y2 = (k.y * cellSize) + rnd.Next(1, cellSize - h - 1);
                    List<Tuple<int, int>> floorTiles = new List<Tuple<int, int>>();
                    for(int i=0; i < w; i++)
                    {
                        for(int j=0; j < h; j++)
                        {
                            floorTiles.Add(new Tuple<int, int>(x+i,y2+j));
                        }
                    }
                    k.room = floorTiles;
                    rooms.Add(floorTiles);
                }

                Dictionary<Tuple<int, int>, Tuple<  List<Tuple<int, int>>, List<Tuple<int, int>> >> connections = new Dictionary<Tuple<int, int>, Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>>>(); 

                foreach(Cell cell in cells.Values)
                {
                    foreach(Cell other in cell.connectedTo)
                    {
                        int[] ids = { cell.id, other.id };
                        Array.Sort(ids);
                        connections[new Tuple<int, int>(ids[0], ids[1])] = new Tuple<List<Tuple<int, int>>, List<Tuple<int, int>>> (cell.room, other.room);
                    }
                    foreach(Tuple<List<Tuple<int,int>>, List<Tuple<int,int>>> bcc in connections.Values)
                    {
                        var start = bcc.Item1[rnd.Next(bcc.Item1.Count)];
                        var end = bcc.Item2[rnd.Next(bcc.Item2.Count)];

                        
                    }
                }
            }
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
        List <Tuple<int, int>> _AStar(Tuple<int, int> start, Tuple<int, int> goal)
        {
            return new List<Tuple<int, int>>();
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
