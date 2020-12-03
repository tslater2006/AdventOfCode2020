using AoCHelper;
using AdventOfCode.Inputs;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public class Day_03 : BaseDay
    {
        private readonly string[] _input;

        public Day_03()
        {
            _input = InputParser.AsLines(InputFilePath);
        }

        public override string Solve_1()
        {
            return TreesForSlope(3, 1).ToString();
        }

        public override string Solve_2()
        {
            (int right, int down)[] slopes = new(int, int)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
            long product = 1;
            foreach(var slope in slopes)
            {
                product *= TreesForSlope(slope.right, slope.down);
            }
            return product.ToString();
        }

        private int TreesForSlope(int x, int y)
        {
            var mapHeight = _input.Length;
            var mapWidth = _input[0].Length;
            var xIndex = 0;
            var treeCount = 0;

            for(var yIndex = 0; yIndex < mapHeight; yIndex += y)
            {
                if (_input[yIndex][xIndex % mapWidth] == '#')
                {
                    treeCount++;
                }
                xIndex += x;
            }
            return treeCount;
        }
    }

}
