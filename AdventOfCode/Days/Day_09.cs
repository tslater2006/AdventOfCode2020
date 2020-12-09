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
            /* find a contingous list of numbers that sum to part 1 answer */
            var startIndex = 0;
            var curIndex = 0;
            long currentSum = 0;

            while (currentSum != part1Answer)
            {
                currentSum += list[startIndex + (curIndex++)];

                if (currentSum > part1Answer)
                {
                    /* no sense going further here, we've exceeded the number */
                    currentSum = 0;
                    curIndex = 0;
                    startIndex++;
                }
            }

            var range = new long[curIndex];
            Array.Copy(list, startIndex, range, 0, curIndex);
            var ans = range.Min() + range.Max();

            return ans.ToString();
        }
    }
}
