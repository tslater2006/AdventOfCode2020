using AdventOfCode.Common;
using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        long[] list;
        long part1Answer;
        public Day_09()
        {
            list = InputParser.AsLinesLong(InputFilePath).ToArray();
        }
        public override string Solve_1()
        {
            
            var preamble = 25;
            for (var x = preamble; x < list.Length; x++)
            {
                var curNumber = list[x];
                var found = false;
                for (var y = x-1; y >= x - preamble; y--)
                {
                    for (var z = x - 1; z >= x - preamble; z--)
                    {
                        if (z == y) { continue; }
                        if (list[y] + list[z] == list[x])
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == true)
                    {
                        break;
                    }
                }
                if (found == false)
                {
                    part1Answer = list[x];
                    return list[x].ToString();
                }
            }

            return "";
        }

        public override string Solve_2()
        {
            var startIndex = 0;
            var stopIndex = 0;
            long sum = 0;

            while (sum != part1Answer)
            {
                if (sum < part1Answer)
                {
                    sum += list[stopIndex++];
                }
                else
                {
                    sum -= list[startIndex++];
                }
            }

            var range = list.Skip(startIndex).Take(stopIndex - startIndex).ToArray();
            var ans = range.Min() + range.Max();

            return ans.ToString();
        }
    }
}
