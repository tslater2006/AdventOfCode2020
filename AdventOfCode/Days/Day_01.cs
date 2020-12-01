using AoCHelper;
using System;
using System.IO;
using Spectre.Console;
using AdventOfCode.Inputs;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using AdventOfCode.Utilities;
using System.ComponentModel;

namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        private readonly long[] _input;

        public Day_01()
        {
            _input = InputParser.AsLinesLong(InputFilePath).ToArray();
        }

        public override string Solve_1()
        {
            IEnumerable<long[]> combos = Combinations.GetCombinations(_input, 2);
            var matchingPair = combos.Where(l => l.Sum() == 2020).First();
            return matchingPair.Aggregate((long)1, (a, b) => a * b).ToString();
        }

        public override string Solve_2()
        {
            IEnumerable<long[]> combos = Combinations.GetCombinations(_input, 3);
            var matchingPair = combos.Where(l => l.Sum() == 2020).First();
            return matchingPair.Aggregate((long)1, (a, b) => a * b).ToString();
        }
    }

}
