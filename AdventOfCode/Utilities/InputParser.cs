using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Inputs
{
    class InputParser
    {
        public static string[] AsParagraphs(string path)
        {
            
            return File.ReadAllText(path).Split(Environment.NewLine + Environment.NewLine);
        }
        public static string[] AsLines(string path)
        {

            return File.ReadAllLines(path);
        }

        public static string AsLine(string path)
        {
            return AsLines(path)[0];
        }

        public static int[] AsDigitInts(string path)
        {
            return AsLine(path)
                   .Select(c => (int)Char.GetNumericValue(c))
                   .ToArray();
        }


        public static List<long> AsLinesLong(string path)
        {
            return
                AsLines(path)
                .Select(l => Int64.Parse(l))
                .ToList();
        }

        public static List<List<int>> AsLinesIntCSV(string path, char split)
        {
            var lines = AsLines(path);

            return lines.Select(l => splitLineToIntCells(l, split))
                        .ToList();
        }

        public static List<List<long>> AsLinesLongCSV(string path, char split)
        {
            var lines = AsLines(path);

            return lines.Select(l => splitLineToLongCells(l, split))
                        .ToList();
        }

        public static List<List<string>> AsLinesStringCSV(string path, char split)
        {
            var lines = AsLines(path);

            return lines.Select(l => splitLineToTextCells(l, split))
                        .ToList();
        }

        public static List<List<string>> AsLinesCharGrid(string path)
        {
            var lines = AsLines(path);

            return lines.Select(l => l.ToCharArray().Select(c => new string(c, 1)).ToList()).ToList();
        }

        public static List<Point> AsLinePoints(string path, char present)
        {
            List<Point> points = new List<Point>();
            string[] dayLines = AsLines(path);

            for (int y = 0; y < dayLines.Length; y++)
            {
                for (int x = 0; x < dayLines[y].Length; x++)
                {
                    if (dayLines[y][x] == present)
                    {
                        points.Add(new Point() { X = x, Y = y });
                    }
                }
            }

            return points;
        }

        private static List<int> splitLineToIntCells(string line, char split)
        {
            var splits = line.Split(split);

            return splits.Select(s => Int32.Parse(s)).ToList();
        }

        private static List<long> splitLineToLongCells(string line, char split)
        {
            var splits = line.Split(split);

            return splits.Select(s => Int64.Parse(s)).ToList();
        }

        private static List<string> splitLineToTextCells(string line, char split)
        {
            var splits = line.Split(split);
            return splits.ToList();
        }
    }
}
