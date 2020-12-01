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
using System.Security.Cryptography.X509Certificates;

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
            for (var x = 0; x < _input.Length; x++)
            {
                var y = 2020 - _input[x];
                if (_input.Contains(y))
                {
                    return (_input[x] * y).ToString();
                }
            }
            return "";
            /* 
            IEnumerable<long[]> combos = Combinations.GetCombinations(_input, 2);
            var matchingPair = combos.Where(l => l.Sum() == 2020).First();
            return matchingPair.Aggregate((long)1, (a, b) => a * b).ToString(); */
        }

        public override string Solve_2()
        { 
            for (var x = 0; x < _input.Length; x++)
            {
                var numX = _input[x];
                var numYZ = 2020 - numX;

                for (var y = 0; y < _input.Length; y++)
                {
                    if (y == x) continue;

                    var numY = numYZ - _input[y];

                    if (_input.Contains(numY))
                    {

                        var numZ = numYZ - numY;

                        if (_input.Contains(numZ))
                        {
                            return (numX * numY * numZ).ToString();
                        }
                    }
                }
            }
            return "";

            /*
            IEnumerable<long[]> combos = Combinations.GetCombinations(_input, 3);
            var matchingPair = combos.Where(l => l.Sum() == 2020).First();
            return matchingPair.Aggregate((long)1, (a, b) => a * b).ToString();*/
            
        }
    }

}
