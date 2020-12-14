using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode
{
    public class Day_13 : BaseDay
    {
        string[] _lines;
        public Day_13()
        {
            _lines = InputParser.AsLines(InputFilePath);
        }

        public override string Solve_1()
        {
            var earliest = int.Parse(_lines[0]);
            var splitBusses = _lines[1].Split(',');
            List<int> busses = new List<int>();

            foreach(var b in splitBusses)
            {
                if (b != "x")
                {
                    busses.Add(int.Parse(b));
                }
            }

            var shortestDelay = 10000000;
            var soonestBus = 0;
            foreach (int bus in busses)
            {
                var firstPossible = (int)Math.Ceiling(earliest / (decimal)bus) * bus;
                var diff = firstPossible - earliest;
                if (diff < shortestDelay)
                {
                    shortestDelay = diff;
                    soonestBus = bus;
                }
            }

            return (soonestBus * shortestDelay).ToString();
        }

        public override string Solve_2()
        {
            /* thanks to the fine folks on discord for the heavy hints here */
            /* we are basically calculating the Chinese Remainder of the busses with their given offsets */

            var busses = _lines[1].Split(',');
            Queue<(int bus, int offset)> busAndOffset = new Queue<(int bus, int offset)>();
            for(var x = 0; x < busses.Length;x++)
            {
                if (busses[x] == "x") { continue; }
                busAndOffset.Enqueue(new(int.Parse(busses[x]), x));
            }

            long currentStamp = 1;
            long lcm = 1;

            while (busAndOffset.Count > 0)
            {
                var curBus = busAndOffset.Dequeue();
                while ((currentStamp + curBus.offset) % curBus.bus != 0)
                {
                    currentStamp += lcm;
                }
                lcm *= curBus.bus;
            }

            return currentStamp.ToString();
        }

    }
}
