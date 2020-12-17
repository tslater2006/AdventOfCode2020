using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using AdventOfCode.Utilities;
using System.Diagnostics;

namespace AdventOfCode
{
    
    public class Day_17 : BaseDay
    {
        char[,] initialMap;
        HashSet<(int X, int Y, int Z, int W)> activePointCloud = new();
        (int minX, int maxX, int minY, int maxY, int minZ, int maxZ, int minW, int maxW) boundary;
        public Day_17()
        {
            initialMap = InputParser.AsLinesCharGrid(InputFilePath);
        }

        void InitCloud()
        {
            activePointCloud.Clear();

            for(var x = 0; x < initialMap.GetLength(1); x++)
            {
                for (var y = 0; y < initialMap.GetLength(0); y++)
                {
                    if (initialMap[y, x] == '#')
                    {
                        activePointCloud.Add((x, y, 0, 0));
                    }
                }
            }
            boundary = GetBounds();


        }
        (int minX, int maxX, int minY, int maxY, int minZ, int maxZ, int minW, int maxW) GetBounds()
        {
            var minX = activePointCloud.Min(p => p.X);
            var maxX = activePointCloud.Max(p => p.X);
            var minY = activePointCloud.Min(p => p.Y);
            var maxY = activePointCloud.Max(p => p.Y);
            var minZ = activePointCloud.Min(p => p.Z);
            var maxZ = activePointCloud.Max(p => p.Z);
            var minW = activePointCloud.Max(p => p.W);
            var maxW = activePointCloud.Max(p => p.W);

            return (minX, maxX, minY, maxY, minZ, maxZ, minW, maxW);
        }

        IEnumerable<(int X, int Y, int Z, int W)> GetNeighborsForPoint((int X, int Y, int Z, int W) point)
        {
            var offsets = new int[] { -1, 0, 1 };
            (int X, int y, int Z, int W) newPoint;
            for (var x = 0; x < 3; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    for (var z = 0; z < 3; z++)
                    {
                        for (var w = 0; w < 3; w++)
                        {
                            if ((x, y, z, w) == (1, 1, 1, 1)) { continue; }
                            newPoint = (point.X + offsets[x], point.Y + offsets[y], point.Z + offsets[z], point.W + offsets[w]);
                            yield return newPoint;
                        }
                            
                    }
                }
            }
        }
        void EvolveCloud(int dimension = 3)
        {
            List<(int X, int Y, int Z, int W)> toAdd = new();
            List<(int X, int Y, int Z, int W)> toRemove = new();

            for (var x = boundary.minX - 1; x <= boundary.maxX + 1; x++)
            {
                for (var y = boundary.minY - 1; y <= boundary.maxY + 1; y++)
                {
                    for (var z = boundary.minZ - 1; z <= boundary.maxZ + 1; z++)
                    {

                        for (var w = boundary.minW-1; w <= boundary.maxW + 1; w++)
                        {
                            /* only process W == 0 in 3D mode */
                            if (dimension == 3 && w != 0) { continue; }
                            var aliveNeighbors = GetNeighborsForPoint((x, y, z, w)).Where(p => activePointCloud.Contains(p)).Count();

                            if (activePointCloud.Contains((x, y, z, w)))
                            {
                                /* current one is active, determine if we should remove */
                                if (aliveNeighbors < 2 || aliveNeighbors > 3)
                                {
                                    toRemove.Add((x, y, z, w));
                                }
                            }
                            else
                            {
                                /* not alive, should it be */
                                if (aliveNeighbors == 3)
                                {
                                    toAdd.Add((x, y, z, w));
                                }
                            }
                        }

                        
                    }
                }
            }

            foreach(var p in toAdd)
            {
                activePointCloud.Add(p);
            }

            foreach(var p in toRemove)
            {
                activePointCloud.Remove(p);
            }

            boundary.minX--;
            boundary.minY--;
            boundary.minZ--;

            boundary.maxX++;
            boundary.maxY++;
            boundary.maxZ++;

            if (dimension == 4)
            {
                boundary.minW--;
                boundary.maxW++;
            }

        }

        public override string Solve_1()
        {
            var list = GetNeighborsForPoint((0, 0, 0, 0)).ToList();

            InitCloud();
            for (var x = 0; x < 6; x++)
            {
                EvolveCloud();
            }
            var active = activePointCloud.Count;
            return active.ToString();
        }

        public override string Solve_2()
        {
            InitCloud();
            for (var x = 0; x < 6; x++)
            {
                EvolveCloud(4);
            }
            var active = activePointCloud.Count;
            return active.ToString();
        }

    }
}
