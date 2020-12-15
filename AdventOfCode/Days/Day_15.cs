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
    public class Day_15 : BaseDay
    {
        int[] seenNumbers = new int[30000000];
        int[] startingNumbers = null;
        int currentNumber;
        int turnNumber = 0;
        public Day_15()
        {
            startingNumbers = InputParser.AsLines(InputFilePath)[0].Split(",").Select(s => int.Parse(s)).ToArray();
            currentNumber = 0;
            turnNumber = 0;
            for (var x = 0; x < startingNumbers.Length - 1; x++)
            {
                turnNumber++;
                seenNumbers[startingNumbers[x]] = turnNumber;
            }
            turnNumber++;
            currentNumber = startingNumbers[^1];
        }

        void PlayTurn()
        {
            int newNumber;
            if (seenNumbers[currentNumber] > 0)
            {
                newNumber = turnNumber - seenNumbers[currentNumber];

            } else
            {
                newNumber = 0;
            }

            seenNumbers[currentNumber] = turnNumber;
            turnNumber++;
            currentNumber = newNumber;
        }

        public override string Solve_1()
        {
            for (var x = 0; x < 2020 - startingNumbers.Length; x++)
            {
                PlayTurn();
            }
            return currentNumber.ToString();
        }

        public override string Solve_2()
        {
            for (var x = 0; x < 30000000 - 2020; x++)
            {
                PlayTurn();
            }
            return currentNumber.ToString();
        }
    }
}
