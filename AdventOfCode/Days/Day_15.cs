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
        int[] startingNumbers = null;
        Dictionary<int, int> seenTracker = new();
        int currentNumber;
        int turnNumber = 0;
        public Day_15()
        {
            startingNumbers = InputParser.AsLines(InputFilePath)[0].Split(",").Select(s => int.Parse(s)).ToArray();
        }

        void PlayTurn()
        {
            var newNumber = 0;
            if (seenTracker.ContainsKey(currentNumber))
            {
                newNumber = turnNumber - seenTracker[currentNumber];
            } else
            {
                newNumber = 0;
            }

            seenTracker[currentNumber] = turnNumber;
            turnNumber++;
            currentNumber = newNumber;
        }
        private void InitGame()
        {
            seenTracker.Clear();
            currentNumber = 0;
            turnNumber = 0;
            for (var x = 0; x < startingNumbers.Length - 1; x++)
            {
                turnNumber++;
                seenTracker.Add(startingNumbers[x], turnNumber);
            }
            turnNumber++;
            currentNumber = startingNumbers[^1];
        }
        public override string Solve_1()
        {
            InitGame();
            for (var x = 0; x < 2020 - startingNumbers.Length; x++)
            {
                PlayTurn();
            }
            return currentNumber.ToString();
        }

        public override string Solve_2()
        {
            InitGame();
            for (var x = 0; x < 30000000 - startingNumbers.Length; x++)
            {
                PlayTurn();
            }
            return currentNumber.ToString();
        }
    }
}
