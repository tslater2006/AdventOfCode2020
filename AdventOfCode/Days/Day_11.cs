using AdventOfCode.Inputs;
using AdventOfCode.Utilities;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day_11 : BaseDay
    {
        char[,] originalGrid;
        char[,] seatGrid;
        char[,] workingCopy;
        int gridHeight;
        int gridWidth;
        Dictionary<(int, int), List<(int, int)>> neighborMap = new Dictionary<(int, int), List<(int, int)>>();
        public Day_11()
        {
            originalGrid = InputParser.AsLinesCharGrid(InputFilePath);
            gridHeight = originalGrid.GetLength(0);
            gridWidth = originalGrid.GetLength(1);
        }
        public override string Solve_1()
        {
            seatGrid = originalGrid.Clone() as char[,];
            workingCopy = originalGrid.Clone() as char[,];

            bool changeHappened = true;
            while (changeHappened)
            {
                changeHappened = EvolveGrid();
            }

            var occupied = 0;
            for (var y = 0; y < gridHeight; y++)
            {
                for (var x = 0; x < gridWidth; x++)
                {
                    occupied += seatGrid[y, x] == '#' ? 1 : 0;
                }
            }

            return occupied.ToString();
        }

        private bool EvolveGrid(int numberToEmpty = 4, bool lineOfSight = false)
        {
            

            var madeChange = false;
            List<(int X, int Y)> seatsToChange = new List<(int X, int Y)>();
            for (var x = 0; x < gridWidth; x++)
            {
                for (var y = 0; y < gridHeight; y++)
                {
                    var curSeat = seatGrid[y, x];
                    if (curSeat != '.')
                    {
                        List<(int X, int Y)> neighbors = null;

                        if (neighborMap.ContainsKey((x, y)))
                        {
                            neighbors = neighborMap[(x, y)];
                        }
                        else
                        {
                            if (lineOfSight)
                            {
                                neighbors = AroundAdjacentLOS((x, y), 0, 0, gridWidth - 1, gridHeight - 1).ToList();
                            }
                            else
                            {
                                neighbors = (x, y).AroundAdjacent(0, 0, gridWidth - 1, gridHeight - 1).ToList();
                            }
                            neighborMap.Add((x, y), neighbors);
                        }
                        
                        var seatedNeighbors = 0;
                        foreach(var p in neighbors)
                        {
                            if (seatGrid[p.Y,p.X] == '#')
                            {
                                seatedNeighbors++;
                            }
                            if (seatedNeighbors == numberToEmpty)
                            {
                                break;
                            }
                        }

                        if (curSeat == 'L' && seatedNeighbors == 0)
                        {
                            madeChange = true;
                            workingCopy[y, x] = '#';
                        }
                        else if (curSeat == '#' && seatedNeighbors >= numberToEmpty)
                        {
                            madeChange = true;
                            workingCopy[y, x] = 'L';
                        }
                    }
                }
            }

            seatGrid = workingCopy.Clone() as char[,];

            return madeChange;
        }

        public override string Solve_2()
        {   
            seatGrid = originalGrid.Clone() as char[,];
            workingCopy = originalGrid.Clone() as char[,];
            neighborMap.Clear();
            bool changeHappened = true;
            while (changeHappened)
            {
                changeHappened = EvolveGrid(5, true);
            }

            var occupied = 0;
            for (var y = 0; y < gridHeight; y++)
            {
                for (var x = 0; x < gridWidth; x++)
                {
                    occupied += seatGrid[y, x] == '#' ? 1 : 0;
                }
            }

            return occupied.ToString();
        }

        public IEnumerable<(int X, int Y)> AroundAdjacentLOS((int X, int Y) p, int minX, int minY, int maxX, int maxY)
        {
            List<(int X, int Y)> possibles = new List<(int, int)>();

            /* each direction of line of sight */
            (int X, int Y)[] deltas = new (int X, int Y)[] {
                (1,0),
                (-1,0),
                (0,1),
                (0,-1),
                (1, -1),
                (1, 1),
                (-1, -1),
                (-1, 1)
            };

            foreach (var d in deltas)
            {
                (int X, int Y) newPoint = (p.X + d.X, p.Y + d.Y);

                while (newPoint.X >= minX && newPoint.X <= maxX && newPoint.Y >= minY && newPoint.Y <= maxY)
                {
                    yield return newPoint;
                    if (seatGrid[newPoint.Y, newPoint.X] != '.')
                    {
                        break;
                    }

                    newPoint = (newPoint.X + d.X, newPoint.Y + d.Y);
                }
            }
        }
    }
}
