using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utilities
{
    static class PointExtensions
    {
        public static IEnumerable<Point> Around(this Point p)
        {
            yield return new Point(p.X + 1, p.Y);
            yield return new Point(p.X - 1, p.Y);
            yield return new Point(p.X, p.Y + 1);
            yield return new Point(p.X, p.Y - 1);
        }

        public static Point Add(this Point p, Point p2)
        {
            return new Point(p.X + p2.X, p.Y + p2.Y);
        }

        public static Point Subtract(this Point p, Point p2)
        {
            return new Point(p.X - p2.X, p.Y - p2.Y);
        }

        public static IEnumerable<Point> Around(this Point p, int minX, int minY, int maxX, int maxY)
        {
            if (p.X + 1 >= minX && p.X + 1 <= maxX)
            {
                yield return new Point(p.X + 1, p.Y);
            }
            if (p.X - 1 >= minX && p.X - 1 <= maxX)
            {
                yield return new Point(p.X - 1, p.Y);
            }
            if (p.Y + 1 >= minY && p.Y + 1 <= maxY)
            {
                yield return new Point(p.X, p.Y + 1);
            }
            if (p.Y - 1 >= minY && p.Y - 1 <= maxY)
            {
                yield return new Point(p.X, p.Y - 1);
            }
        }

        public static IEnumerable<(int X, int Y)> AroundOrthogonal(this (int X, int Y) p, int minX, int minY, int maxX, int maxY)
        {
            if (p.X + 1 >= minX && p.X + 1 <= maxX)
            {
                yield return (p.X + 1, p.Y);
            }
            if (p.X - 1 >= minX && p.X - 1 <= maxX)
            {
                yield return (p.X - 1, p.Y);
            }
            if (p.Y + 1 >= minY && p.Y + 1 <= maxY)
            {
                yield return (p.X, p.Y + 1);
            }
            if (p.Y - 1 >= minY && p.Y - 1 <= maxY)
            {
                yield return (p.X, p.Y - 1);
            }
        }

        public static IEnumerable<(int X,int Y)> AroundOrthogonal(this (int X,int Y) p)
        {
            yield return (p.X + 1, p.Y);
            yield return (p.X - 1, p.Y);
            yield return (p.X, p.Y + 1);
            yield return (p.X, p.Y - 1);
        }

        public static IEnumerable<(int X, int Y)> AroundAdjacent(this (int X, int Y) p)
        {
            yield return (p.X + 1, p.Y);
            yield return (p.X - 1, p.Y);
            yield return (p.X, p.Y + 1);
            yield return (p.X, p.Y - 1);

            yield return (p.X + 1, p.Y + 1);
            yield return (p.X - 1, p.Y + 1);
            yield return (p.X - 1, p.Y - 1);
            yield return (p.X + 1, p.Y - 1);

        }

        public static IEnumerable<(int X, int Y)> AroundAdjacent(this (int X, int Y) p, int minX, int minY, int maxX, int maxY)
        {
            var possibles = new (int X, int Y)[]
            {
                (p.X + 1, p.Y),
                (p.X - 1, p.Y),
                (p.X, p.Y + 1),
                (p.X, p.Y - 1),
                (p.X + 1, p.Y + 1),
                (p.X - 1, p.Y + 1),
                (p.X - 1, p.Y - 1),
                (p.X + 1, p.Y - 1)
            };

            foreach(var newPoint in possibles )
            {
                if (newPoint.X >= minX && newPoint.X <= maxX && newPoint.Y >= minY && newPoint.Y <= maxY)
                {
                    yield return newPoint;
                }
            }
        }

        public static void PrintGrid(this char[,] grid)
        {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);

            for(var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    Console.Write(grid[y, x]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static IEnumerable<char> AsEnumerable(this char[,] grid)
        {
            var height = grid.GetLength(0);
            var width = grid.GetLength(1);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    yield return grid[y, x];
                }
            }
        }
    }
}
