using AoCHelper;
using System;
using System.IO;
using Spectre.Console;
namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        private readonly string _input;

        public Day_01()
        {
            _input = File.ReadAllText(InputFilePath);
        }

        public override string Solve_1()
        {
            return "foo";
        }

        public override string Solve_2()
        {
            return "bar";
        }
    }
}
