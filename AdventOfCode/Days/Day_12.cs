using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode
{
    public class Day_12 : BaseDay
    {
        private readonly (char Direction, int Amount)[] Directions;
        private (int x, int y)[] deltas = new (int x, int y)[]
        {
            (1,0), /* right east */
            (0,-1), /* down / south */
            (-1,0), /* left / west */
            (0,1) /* up / north */
        };
        public Day_12()
        {
            List<(char Direction, int Amount)> parsed = new List<(char Direction, int Amount)>();
            foreach (var line in InputParser.AsLines(InputFilePath))
            {
                parsed.Add((line[0], int.Parse(line[1..])));
            }
            Directions = parsed.ToArray();
        }

        public override string Solve_1()
        {
            Point curLocation = new Point(0, 0);
            int rotationIndex = 0;


            foreach (var dir in Directions)
            {
                switch (dir.Direction)
                {
                    case 'L':
                        rotationIndex -= (dir.Amount / 90);
                        while (rotationIndex < 0)
                        {
                            rotationIndex += 4;
                        }
                        break;
                    case 'R':
                        rotationIndex += (dir.Amount / 90);
                        rotationIndex %= 4;
                        break;
                    case 'F':
                        curLocation.X += deltas[rotationIndex + 0].x * dir.Amount;
                        curLocation.Y += deltas[rotationIndex + 0].y * dir.Amount;
                        break;
                    case 'N':
                        curLocation.X += deltas[3].x * dir.Amount;
                        curLocation.Y += deltas[3].y * dir.Amount;
                        break;
                    case 'E':
                        curLocation.X += deltas[0].x * dir.Amount;
                        curLocation.Y += deltas[0].y * dir.Amount;
                        break;
                    case 'S':
                        curLocation.X += deltas[1].x * dir.Amount;
                        curLocation.Y += deltas[1].y * dir.Amount;
                        break;
                    case 'W':
                        curLocation.X += deltas[2].x * dir.Amount;
                        curLocation.Y += deltas[2].y * dir.Amount;
                        break;
                }
            }

            return (Math.Abs(curLocation.X) + Math.Abs(curLocation.Y)).ToString();
        }

        public override string Solve_2()
        {
            Point curLocation = new Point(0, 0);
            Point wayPoint = new Point(10, 1);
            (int x, int y)[] deltas = new (int x, int y)[]
            {
                (1,0), /* right east */
                (0,-1), /* down / south */
                (-1,0), /* left / west */
                (0,1) /* up / north */
                
            };

            foreach (var dir in Directions)
            {
                switch (dir.Direction)
                {
                    case 'L':
                        wayPoint = RotatePoint(wayPoint, dir.Amount);
                        break;
                    case 'R':
                        wayPoint = RotatePoint(wayPoint, -1 * dir.Amount);
                        break;
                    case 'F':
                        curLocation.X += wayPoint.X * dir.Amount;
                        curLocation.Y += wayPoint.Y * dir.Amount;
                        break;
                    case 'N':
                        wayPoint.X += deltas[3].x * dir.Amount;
                        wayPoint.Y += deltas[3].y * dir.Amount;
                        break;
                    case 'E':
                        wayPoint.X += deltas[0].x * dir.Amount;
                        wayPoint.Y += deltas[0].y * dir.Amount;
                        break;
                    case 'S':
                        wayPoint.X += deltas[1].x * dir.Amount;
                        wayPoint.Y += deltas[1].y * dir.Amount;
                        break;
                    case 'W':
                        wayPoint.X += deltas[2].x * dir.Amount;
                        wayPoint.Y += deltas[2].y * dir.Amount;
                        break;
                }
            }
            return (Math.Abs(curLocation.X) + Math.Abs(curLocation.Y)).ToString();
        }

        /* For "R" rotates, need to pass in negative angle */
        private Point RotatePoint(Point p, int angle)
        {
            var c = Math.Cos(Math.PI * (angle) / 180.0);
            var s = Math.Sin(Math.PI * (angle) / 180.0);

            var xnew = (int)Math.Round(p.X * c - p.Y * s);
            var ynew = (int)Math.Round(p.X * s + p.Y * c);
            p.X = xnew;
            p.Y = ynew;

            return p;
        }
    }
}
