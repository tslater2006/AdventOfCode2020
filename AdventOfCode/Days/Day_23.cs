using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using AdventOfCode.Utilities;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class CupGame
    {
        int[] cupArray;
        int currentCup;
        public CupGame(string cupOrder, int cupCount = 0)
        {
            if (cupCount == 0)
            {
                cupCount = cupOrder.Length;
            }
            cupArray = new int[cupCount];
            int firstCup = -1; ;
            int previousCup = -1;
            foreach (var c in cupOrder.Select(c => c - 0x30))
            {
                if (firstCup == -1)
                {
                    firstCup = c - 1;
                    previousCup = c - 1;
                }
                else
                {
                    cupArray[previousCup] = c - 1;
                    previousCup = c - 1;
                }
            }
            for (var x = cupOrder.Length; x < cupCount ; x++)
            {
                cupArray[previousCup] = x ;
                previousCup = x;
            }
            /* connect the last one to the first one */
            cupArray[previousCup] = firstCup;
            currentCup = firstCup;
        }


        public void Move()
        {
            var chainStart = cupArray[currentCup];
            var chainEnd = cupArray[cupArray[cupArray[currentCup]]];
            cupArray[currentCup] = cupArray[chainEnd];

            var dest = currentCup - 1;
            if (dest < 0)
            {
                dest = cupArray.Length - 1;
            }
            while (dest == chainStart || dest == cupArray[chainStart] || dest == chainEnd)
            {
                dest--;
                if (dest < 0)
                {
                    dest = cupArray.Length - 1;
                }
            }
            var target = dest;
            var targetNext = cupArray[target];
            cupArray[target] = chainStart;
            cupArray[chainEnd] = targetNext;

            currentCup = cupArray[currentCup];
        }

        public void PrintCups()
        {
            Console.Write("(" + (currentCup + 1) + ") ");
            var nextCup = cupArray[currentCup];
            while (nextCup != currentCup)
            {
                Console.Write((nextCup + 1) + " ");
                nextCup = cupArray[nextCup];
            }
            Console.WriteLine();
        }

        public string CupsStartingAt(int cupNumber)
        {

            StringBuilder sb = new();


            var nextCup = cupArray[cupNumber-1];

            while (nextCup != (cupNumber-1))
            {
                sb.Append((nextCup + 1));
                nextCup = cupArray[nextCup];
            }

            return sb.ToString();
        }

        public int GetNextCup(int cupNumber)
        {
            return cupArray[cupNumber - 1] + 1;
        }

    }

    public class Day_23 : BaseDay
    {

        public override string Solve_1()
        {
            CupGame g = new CupGame(InputParser.AsLine(InputFilePath));
            for(var x = 0; x <100; x++)
            {
                g.Move();
            }

            var ans = g.CupsStartingAt(1);
            return ans.ToString();
        }

        public override string Solve_2()
        {
            CupGame g = new CupGame(InputParser.AsLine(InputFilePath), 1000000);
            for (var x = 0; x < 10000000; x++)
            {
                g.Move();
            }
            var cup1 = g.GetNextCup(1);
            var cup2 = g.GetNextCup(cup1);


            //long ans = cup2.Label * (long)cup3.Label;
            long ans = cup1 * (long)cup2;
            return ans.ToString();
        }

    }
}
