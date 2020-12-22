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

namespace AdventOfCode
{
    enum PuzzleEdge
    {
        TOP, RIGHT, BOTTOM, LEFT
    }

    class PuzzlePiece
    {
        public long TileNumber;
        public char[,] data = new char[10, 10];
        public Dictionary<PuzzleEdge, char[]> Edges = new();
        public Dictionary<PuzzleEdge, PuzzlePiece> ConnectedPieces = new();

        public PuzzlePiece(List<string> tileInfo)
        {
            /* first line is tile number */
            TileNumber = int.Parse(Regex.Match(tileInfo[0], "(\\d+)").Groups[1].Value);

            Edges.Add(PuzzleEdge.TOP, new char[10]);
            Edges.Add(PuzzleEdge.BOTTOM, new char[10]);
            Edges.Add(PuzzleEdge.LEFT, new char[10]);
            Edges.Add(PuzzleEdge.RIGHT, new char[10]);

            for (var y = 1; y < tileInfo.Count; y++)
            {
                for(var x = 0; x < tileInfo[y].Length; x++)
                {
                    data[y - 1, x] = tileInfo[y][x];

                    if (y == 1)
                    {
                        Edges[PuzzleEdge.TOP][x] = tileInfo[y][x];
                    }
                    if (y == tileInfo.Count -1)
                    {
                        Edges[PuzzleEdge.BOTTOM][x] = tileInfo[y][x];
                    }
                    if (x == 0)
                    {
                        Edges[PuzzleEdge.LEFT][y-1] = tileInfo[y][x];
                    }
                    if (x == tileInfo[y].Length - 1)
                    {
                        Edges[PuzzleEdge.RIGHT][y-1] = tileInfo[y][x];
                    }
                }
            }

        }

        public PuzzlePiece(long tileNumber, char[,] newData)
        {
            TileNumber = tileNumber;
            data = newData;

            Edges.Add(PuzzleEdge.TOP, new char[newData.GetLength(1)]);
            Edges.Add(PuzzleEdge.BOTTOM, new char[newData.GetLength(1)]);
            Edges.Add(PuzzleEdge.LEFT, new char[newData.GetLength(0)]);
            Edges.Add(PuzzleEdge.RIGHT, new char[newData.GetLength(0)]);

            for (var y = 0; y < newData.GetLength(0); y++)
            {
                for(var x = 0; x < newData.GetLength(1); x++)
                {
                    if (y == 0)
                    {
                        Edges[PuzzleEdge.TOP][x] = newData[y, x];
                    }
                    if (y == newData.GetLength(0) - 1)
                    {
                        Edges[PuzzleEdge.BOTTOM][x] = newData[y, x];
                    }
                    if (x == 0)
                    {
                        Edges[PuzzleEdge.LEFT][y] = newData[y, x];
                    }
                    if (x == newData.GetLength(1) - 1)
                    {
                        Edges[PuzzleEdge.RIGHT][y] = newData[y, x];
                    }
                }
            }
        }

        public PuzzlePiece RotatePiece(int angle)
        {
            var newData = data.Clone() as char[,];
            var rotateCount = angle / 90;

            var N = newData.GetLength(0);
            for (var z = 0; z < rotateCount; z++)
            {
                // Consider all 
                // squares one by one 
                for (int x = 0; x < N / 2; x++)
                {
                    // Consider elements 
                    // in group of 4 in 
                    // current square 
                    for (int y = x; y < N - x - 1; y++)
                    {
                        // store current cell 
                        // in temp variable 
                        char temp = newData[x, y];

                        // move values from 
                        // right to top 
                        newData[x, y] = newData[y, N - 1 - x];

                        // move values from 
                        // bottom to right 
                        newData[y, N - 1 - x] = newData[N - 1 - x,
                                                N - 1 - y];

                        // move values from 
                        // left to bottom 
                        newData[N - 1 - x,
                            N - 1 - y]
                            = newData[N - 1 - y, x];

                        // assign temp to left 
                        newData[N - 1 - y, x] = temp;
                    }
                }
            }

            return new PuzzlePiece(this.TileNumber, newData);
        }

        public PuzzlePiece FlipX()
        {
            var newData = data.Clone() as char[,];

            var height = newData.GetLength(0);
            var width = newData.GetLength(1);
            
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width / 2; x++)
                {
                    var temp = newData[y, (width - 1) - x];
                    newData[y, (width - 1) - x] = newData[y, x];
                    newData[y, x] = temp;
                }
            }
            return new PuzzlePiece(this.TileNumber, newData);
        }

        public PuzzlePiece FlipY()
        {
            var newData = data.Clone() as char[,];

            var height = newData.GetLength(0);
            var width = newData.GetLength(1);

            for (var y = 0; y < height / 2; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var temp = newData[(height-1) - y, x];
                    newData[(height - 1) - y, x] = newData[y, x];
                    newData[y, x] = temp;
                }
            }

            return new PuzzlePiece(this.TileNumber, newData);
        }

        public bool ConnectsOnEdge(PuzzleEdge edge, PuzzlePiece tile)
        {
            char[] sourceEdge;
            char[] targetEdge = null;

            sourceEdge = this.Edges[edge];

            switch (edge)
            {
                case PuzzleEdge.TOP:
                    targetEdge = tile.Edges[PuzzleEdge.BOTTOM];
                    break;
                case PuzzleEdge.RIGHT:
                    targetEdge = tile.Edges[PuzzleEdge.LEFT];
                    break;
                case PuzzleEdge.BOTTOM:
                    targetEdge = tile.Edges[PuzzleEdge.TOP];
                    break;
                case PuzzleEdge.LEFT:
                    targetEdge = tile.Edges[PuzzleEdge.RIGHT];
                    break;
            }

            return sourceEdge.SequenceEqual(targetEdge);
        }
    }

    
    public class Day_20 : BaseDay
    {
        List<PuzzlePiece> InitialPieces = new();
        List<PuzzlePiece> AllPieces = new();
        PuzzlePiece debugStart = null;
        public Day_20()
        {
            var _lines = InputParser.AsLines(InputFilePath);

            for(var x = 0; x <= _lines.Length; x+=12)
            {
                List<string> tileLines = _lines.Skip(x).Take(11).ToList();
                InitialPieces.Add(new PuzzlePiece(tileLines));
            }

            foreach(var piece in InitialPieces)
            {
                /* Add the initial */
                AllPieces.Add(piece);
                for (var angle = 90; angle < 360; angle += 90)
                {
                    AllPieces.Add(piece.RotatePiece(angle));
                }


                var xFlip = piece.FlipX();
                
                AllPieces.Add(xFlip);
                for (var angle = 90; angle < 360; angle += 90)
                {
                    AllPieces.Add(xFlip.RotatePiece(angle));
                }

            }

        }



        public override string Solve_1()
        {
            var possibleEdges = new PuzzleEdge[] { PuzzleEdge.TOP, PuzzleEdge.BOTTOM, PuzzleEdge.LEFT, PuzzleEdge.RIGHT };
            Dictionary<PuzzleEdge, PuzzleEdge> OppositeEdges = new()
            {
                { PuzzleEdge.TOP, PuzzleEdge.BOTTOM },
                { PuzzleEdge.BOTTOM, PuzzleEdge.TOP },
                { PuzzleEdge.LEFT, PuzzleEdge.RIGHT },
                { PuzzleEdge.RIGHT, PuzzleEdge.LEFT }
            };


            var startingPiece = InitialPieces.First();
            if (debugStart != null)
            {
                startingPiece = debugStart;
            }

            Queue<PuzzlePiece> piecesToSolve = new();
            piecesToSolve.Enqueue(startingPiece);

            while (piecesToSolve.Count > 0)
            {
                var currentPiece = piecesToSolve.Dequeue();
                var piecesFound = 0;
                
                foreach(var piece in AllPieces)
                {
                    var pieceTileNumber = piece.TileNumber;
                    if (piece.TileNumber == currentPiece.TileNumber) continue;

                    foreach(var edge in possibleEdges)
                    {
                        if (currentPiece.ConnectedPieces.ContainsKey(edge)) continue;
                        if (piece.ConnectedPieces.ContainsKey(OppositeEdges[edge])) continue;
                        if (currentPiece.ConnectsOnEdge(edge, piece))
                        {
                            /* found piece that connects on top */
                            currentPiece.ConnectedPieces.Add(edge, piece);
                            piece.ConnectedPieces.Add(OppositeEdges[edge], currentPiece);
                            piecesToSolve.Enqueue(piece);
                            piecesFound++;
                            break;
                        }
                    }
                }
            }

            var corners = AllPieces.Where(p => p.ConnectedPieces.Keys.Count == 2).Select(p => p.TileNumber).ToList();


            var answer = corners.Aggregate((long)1, (acc, n) => acc * n);
            return answer.ToString();
        }

        public override string Solve_2()
        {
            var z = debugStart; /* yflip has right and bottom */
            int i = 3;


            Dictionary<(int X, int Y), PuzzlePiece> coordinates = new();

            int workingX = 0;
            int workingY = 0;

            PuzzlePiece rowStart = AllPieces.Where(p => p.ConnectedPieces.Keys.Count == 2 && p.ConnectedPieces.ContainsKey(PuzzleEdge.RIGHT) && p.ConnectedPieces.ContainsKey(PuzzleEdge.BOTTOM)).First();

            PuzzleEdge verticalDirection = (rowStart.ConnectedPieces.ContainsKey(PuzzleEdge.TOP) ? PuzzleEdge.TOP : PuzzleEdge.BOTTOM);
            PuzzleEdge horizontalDirection = (rowStart.ConnectedPieces.ContainsKey(PuzzleEdge.LEFT) ? PuzzleEdge.LEFT : PuzzleEdge.RIGHT);

            var currentPiece = rowStart;
            while (currentPiece.ConnectedPieces.ContainsKey(verticalDirection))
            {

                while (currentPiece.ConnectedPieces.ContainsKey(horizontalDirection))
                {
    
                    coordinates.Add((workingX, workingY), currentPiece);
                    currentPiece = currentPiece.ConnectedPieces[horizontalDirection];
                    workingX++;
                }

                /* add final one of row */
                coordinates.Add((workingX, workingY), currentPiece);

                if (rowStart.ConnectedPieces.ContainsKey(verticalDirection))
                {
    
                    currentPiece = rowStart.ConnectedPieces[verticalDirection];
                    rowStart = currentPiece;
                    workingY++;
                    workingX = 0;
                } else
                {
                    break;
                }

            }

            /* process final row */
            while (currentPiece.ConnectedPieces.ContainsKey(horizontalDirection))
            {
                coordinates.Add((workingX, workingY), currentPiece);
                currentPiece = currentPiece.ConnectedPieces[horizontalDirection];
                workingX++;
            }

            /* add final one of row */
            coordinates.Add((workingX, workingY), currentPiece);
            if (currentPiece.TileNumber == 1951) Debugger.Break();
            int maxX = coordinates.Keys.Max(k => k.X);
            int maxY = coordinates.Keys.Max(k => k.Y);
            int tileWidth = AllPieces[0].data.GetLength(1);
            int mergedWidth = (tileWidth - 2) * (maxX + 1);
            int tileHeight = AllPieces[0].data.GetLength(0);
            int mergedHeight = (tileHeight- 2) * (maxY + 1);

            char[,] merged = new char[mergedHeight, mergedWidth];


            for(var x = 0; x <= maxX; x++)
            {
                for (var y = 0; y <= maxY; y++)
                {
                    var curTile = coordinates[(x, y)];

                    for (var tileX = 1; tileX < tileWidth-1; tileX++)
                    {
                        for (var tileY = 1; tileY < tileHeight - 1; tileY++)
                        {
                            /* need to map tileX,tileY to merged[] */
                            var xCoord = (tileX - 1) + ((tileWidth - 2) * x);
                            var yCoord = (tileY - 1) + ((tileHeight - 2) * y);

                            merged[yCoord, xCoord] = curTile.data[tileY, tileX];

                        }
                    }


                }
            }

            PuzzlePiece mergedPuzzle = new PuzzlePiece(999, merged);

            var monsterCount = GetMonsterCount(mergedPuzzle.data);

            if (monsterCount == 0)
            {
                /* try all rotations */
                for (var angle = 90; angle < 360; angle += 90)
                {
                    monsterCount = GetMonsterCount(mergedPuzzle.RotatePiece(angle).data);
                    if (monsterCount > 0)
                    {
                        break;
                    }
                }
            }

            if (monsterCount == 0)
            {
                /* try it flipped */
                PuzzlePiece xFlip = mergedPuzzle.FlipX();
                monsterCount = GetMonsterCount(xFlip.data);

                if (monsterCount == 0)
                {
                    /* try all rotations of the x flip */
                    for (var angle = 90; angle < 360; angle += 90)
                    {
                        monsterCount = GetMonsterCount(xFlip.RotatePiece(angle).data);
                        if (monsterCount > 0)
                        {
                            break;
                        }
                    }
                }
            }


            var monsterCells = monsterCount * 15;
            var totalHashCount = 0;
            for(var x = 0; x < mergedWidth; x++)
            {
                for (var y = 0; y < mergedHeight; y++)
                {
                    totalHashCount += merged[y, x] == '#' ? 1 : 0;
                }
            }

            var answer = totalHashCount - monsterCells;
            return answer.ToString();
        }

        int GetMonsterCount(char[,] image)
        {
            (int X, int Y)[] monsterMask = new (int X, int Y)[]
            {
                (18,0),
                (0,1),
                (5,1),
                (6,1),
                (11,1),
                (12,1),
                (17,1),
                (18,1),
                (19,1),
                (1,2),
                (4,2),
                (7,2),
                (10,2),
                (13,2),
                (16,2)
            };

            var monsterWidth = 20;
            var monsterHeight = 3;

            var height = image.GetLength(0);
            var width = image.GetLength(1);
            var monsterCount = 0;
            for (var x = 0; x < width - monsterWidth; x++)
            {
                for (var y = 0; y < height - monsterHeight; y++)
                {
                    monsterCount += monsterMask.All(p => image[y + p.Y, x + p.X] == '#') ? 1 : 0;
                }
            }
            return monsterCount;
        }
    }
}
