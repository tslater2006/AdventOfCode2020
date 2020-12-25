using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using AdventOfCode.Utilities;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdventOfCode
{

    class HexTile
    {
        public bool IsWhite = true;
        public bool Highlight = false;
        /* using odd-r" layout */
        public (int Q, int R) Location;

        public IEnumerable<(int Q, int R)> Neighbors()
        {
            if (Location.R % 2 == 0)
            {
                yield return (Location.Q - 1, Location.R - 1);
                yield return (Location.Q, Location.R - 1);
                yield return (Location.Q + 1, Location.R);
                yield return (Location.Q, Location.R + 1);
                yield return (Location.Q - 1, Location.R + 1);
                yield return (Location.Q - 1, Location.R);
            }
            else
            {
                yield return (Location.Q, Location.R - 1);
                yield return (Location.Q + 1, Location.R - 1);
                yield return (Location.Q + 1, Location.R);
                yield return (Location.Q + 1, Location.R + 1);
                yield return (Location.Q, Location.R + 1);
                yield return (Location.Q - 1, Location.R);
            }
        }

        public override string ToString()
        {
            return $"({Location.Q}, {Location.R})";
        }
    }

    public class Day_24 : BaseDay
    {
        Dictionary<(int Q, int R), HexTile> tileMap = new();
        string[] _lines;
        Regex directionRegex = new Regex("(e|se|sw|w|nw|ne)");
        public Day_24()
        {
            _lines = InputParser.AsLines(InputFilePath);
        }

        public override string Solve_1()
        {
            HexTile origin = new HexTile() { IsWhite = true, Location = (0,0) };
            tileMap.Add((0,0), origin);
            foreach(var l in _lines)
            {
                (int Q, int R) nextCoord = (0,0);
                foreach (string dir in directionRegex.Matches(l).Select(m => m.Value))
                {

                    switch(dir)
                    {
                        case "e":
                            nextCoord.Q += 1;
                            break;
                        case "se":
                            if (nextCoord.R % 2 != 0)
                            {
                                nextCoord.Q += 1;
                                nextCoord.R += 1;
                                
                            } else
                            {
                                nextCoord.R += 1;
                            }
                            break;
                        case "sw":
                            if (nextCoord.R % 2 != 0)
                            {
                                nextCoord.R += 1;
                            } else
                            {
                                nextCoord.Q -= 1;
                                nextCoord.R += 1;
                            }
                            break;
                        case "w":
                            nextCoord.Q -= 1;
                            break;
                        case "nw":
                            if (nextCoord.R % 2 != 0)
                            {
                                nextCoord.R -= 1;
                            }
                            else
                            {
                                nextCoord.Q -= 1;
                                nextCoord.R -= 1;
                            }
                            break;
                        case "ne":
                            if (nextCoord.R % 2 != 0)
                            {
                                nextCoord.Q += 1;
                                nextCoord.R -= 1;
                                
                            }
                            else
                            {
                                nextCoord.R -= 1;
                            }
                            break;
                    }
                    if (!tileMap.ContainsKey(nextCoord))
                    {
                        tileMap.Add((nextCoord.Q, nextCoord.R), new HexTile() { IsWhite = true, Location = (nextCoord.Q, nextCoord.R) });
                    }
                }

                tileMap[nextCoord].IsWhite = !tileMap[nextCoord].IsWhite;
            }

            var count = tileMap.Where(kv => kv.Value.IsWhite == false).Count();
            return count.ToString();
        }

        public override string Solve_2()
        {
            HexTile t = new HexTile() { Location = (3, 2) };
            var nTest = t.Neighbors().ToList();
            
            for (var x = 1; x <= 100; x++)
            {
                ChangeTilesForDay();
                
            }

            var blackTileCount = 0;
            blackTileCount = tileMap.Where(kv => kv.Value.IsWhite == false).Count();
            return blackTileCount.ToString();
        }

        void ChangeTilesForDay()
        {
            List<HexTile> toChange = new();
            List<HexTile> newTiles = new();

            /* ensure we have neighbors to all black tiles */
            var blackTileList = tileMap.Values.Where(t => !t.IsWhite).ToList();
            foreach (var t in blackTileList)
            {
                foreach( var n in t.Neighbors())
                {
                    if (!tileMap.ContainsKey(n))
                    {
                        tileMap.Add(n, new HexTile() { IsWhite = true, Location = n });
                    }
                }
            }

            foreach(var x in tileMap.Values)
            {
                var blackTileCount = 0;
                foreach(var n in x.Neighbors())
                {
                    if (tileMap.ContainsKey(n))
                    {
                        blackTileCount += tileMap[n].IsWhite == false ? 1 : 0;
                    } else
                    {
                        /* tile doesn't exist in map, add it and make it white */
                        newTiles.Add(new HexTile() { IsWhite = true, Location = (n.Q, n.R) });
                    }
                }

                if (x.IsWhite)
                {
                    if (blackTileCount == 2)
                    {
                        toChange.Add(x);
                    }
                } else
                {
                    if (blackTileCount == 0 || blackTileCount > 2)
                    {
                        toChange.Add(x);
                    }
                }
            }
            foreach(var t in newTiles)
            {
                tileMap.TryAdd(t.Location, t);
            }
            foreach(var t in toChange)
            {
                t.Highlight = true;
                t.IsWhite = !t.IsWhite;
            }
        }

    }
}
