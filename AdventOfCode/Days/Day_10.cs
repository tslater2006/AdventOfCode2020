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
    public class Day_10 : BaseDay
    {
        long[] adapters;
        long part1Answer;
        public Day_10()
        {
            List<long> adapterList = InputParser.AsLinesLong(InputFilePath);
            adapterList.Add(0);
            adapterList.Add(adapterList.Max() + 3);
            adapters = adapterList.ToArray();
        }
        public override string Solve_1()
        {
            var diff1 = 0;
            var diff3 = 0;
            long curDiff = 0;
            Array.Sort(adapters);

            for (var x = 0; x < adapters.Length - 1; x++)
            {
                curDiff = adapters[x + 1] - adapters[x];
                if (curDiff == 1) { diff1++; }
                if (curDiff == 3) { diff3++; }
            }

            return (diff1 * diff3).ToString();
        }

        public override string Solve_2()
        {
            var x = GetCombosForAdapter(0);
            return x.ToString();
        }
        Dictionary<long, long> memoize = new Dictionary<long, long>();
        long cacheHit;
        long cacheMiss;
        private long GetCombosForAdapter(long index)
        {
            if (memoize.ContainsKey(index))
            {
                cacheHit++;
                return memoize[index];
            }
            cacheMiss++;
            long count = 0;
            for(var x =index + 1; x < adapters.Length; x++)
            {
                if (adapters[x] - adapters[index] <=3)
                {
                    count += GetCombosForAdapter(x);
                } else
                {
                    break;
                }
            }

            if (count == 0)
            {
                count = 1;
            }

            memoize.Add(index, count);
            return count;
        }

    }
}
