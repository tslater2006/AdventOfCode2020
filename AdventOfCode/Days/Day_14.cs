using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using AdventOfCode.Utilities;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day_14 : BaseDay
    {
        string[] _lines;
        Regex numberRegex = new Regex("(\\d+)");
        public Day_14()
        {
            _lines = InputParser.AsLines(InputFilePath);
        }

        public override string Solve_1()
        {
            Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            ulong andMask = 0;
            ulong orMask = 0;
            foreach (var line in _lines)
            {
                if (line.StartsWith("mask"))
                {
                    (andMask, orMask) = ParseMask(line[7..]);
                } else
                {
                    var numbers = numberRegex.Matches(line);
                    var memAddr = ulong.Parse(numbers[0].Groups[1].Value);
                    var value = ulong.Parse(numbers[1].Groups[1].Value);

                    memory[memAddr] = (value & andMask) | orMask;
                }
            }
            ulong sum = 0;
            foreach( var v in memory.Values)
            {
                sum += v;
            }

            return sum.ToString();
        }

        public override string Solve_2()
        {
            var curMask = "";
            Dictionary<ulong, ulong> memory = new Dictionary<ulong, ulong>();
            foreach (var line in _lines)
            {
                if (line.StartsWith("mask"))
                {
                    curMask = line[7..];
                }
                else
                {
                    var numbers = numberRegex.Matches(line);
                    var memAddr = ulong.Parse(numbers[0].Groups[1].Value);
                    var value = ulong.Parse(numbers[1].Groups[1].Value);

                    foreach(var addr in ApplyV2Mask(curMask, memAddr))
                    {
                        memory[addr] = value;
                    }

                }
            }

            ulong sum = 0;
            foreach (var v in memory.Values)
            {
                sum += v;
            }

            return sum.ToString();
        }

        private List<ulong> ApplyV2Mask(string str, ulong address)
        {
            List<ulong> results = new List<ulong>();
            results.Add(0);
            for(var x = 0; x < str.Length; x++)
            {
                for(var z = 0; z < results.Count; z++)
                {
                    results[z] <<= 1;
                }

                if (x == 29)
                {
                    int i = 3;
                }
                var c = str[x];
                var addrBit = (address >> (36 - (x + 1)) & 1);
                if (c == '1')
                {
                    for(var y = 0; y < results.Count; y++)
                    {
                        results[y] += 1;
                    }
                } if (c == '0')
                {
                    for (var y = 0; y < results.Count; y++)
                    {
                        results[y] += addrBit;
                    }
                } if (c == 'X')
                {
                    var curCount = results.Count;
                    for (var y = 0; y < curCount; y++)
                    {
                        results.Add(results[y]);
                        results[y] += 1;
                    }
                }

            }

            return results;
        }

        private (ulong and, ulong or) ParseMask(string str)
        {
            ulong andMask = 0;
            ulong orMask = 0;

            foreach(var c in str)
            {
                andMask <<= 1;
                orMask <<= 1;

                if(c == '1')
                {
                    andMask += 1;
                    orMask += 1;
                }
                if (c == 'X')
                {
                    andMask += 1;
                }

            }

            return (andMask, orMask);
        }

    }
}
