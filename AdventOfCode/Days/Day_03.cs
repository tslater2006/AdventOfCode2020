using AoCHelper;
using AdventOfCode.Inputs;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode
{
    public class Day_03 : BaseDay
    {
        private readonly List<List<string>> _input;

        public Day_03()
        {
            _input = InputParser.AsLinesCharGrid(InputFilePath);
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
            var mapHeight = _input.Count;
            var mapWidth = _input[0].Count;

            var treeCount = 0;

            Point location = new Point(0, 0);

            while (location.Y < mapHeight)
            {
                if (_input[location.Y][location.X] == "#")
                {
                    treeCount++;
                }

                location.X += x;
                if (location.X > mapWidth - 1)
                {
                    location.X -= mapWidth;
                }
                location.Y += y;

            }
            return treeCount;
        }

    }

}
