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
        Dictionary<int, Cup> cupMap = new();
        Cup currentCup;
        int maxCup;
        int minCup = 100;
        public CupGame(string cupOrder)
        {
            Cup firstCup = null;
            Cup previousCup = null;
            foreach (var c in cupOrder.Select(c => c - 0x30))
            {
                maxCup = (c > maxCup) ? c : maxCup;
                minCup = (c < minCup) ? c : minCup;

                Cup newCup = new Cup() { Label = c };

                if (firstCup == null)
                {
                    firstCup = newCup;
                    previousCup = newCup;
                }
                else
                {
                    previousCup.Next = newCup;
                    previousCup = newCup;
                }
                cupMap.Add(c, newCup);
            }

            /* connect the last one to the first one */
            previousCup.Next = firstCup;
            currentCup = firstCup;
        }

        public CupGame(string cupOrder, int totalCups)
        {
            Cup firstCup = null;
            Cup previousCup = null;
            foreach (var c in cupOrder.Select(c => c - 0x30))
            {
                maxCup = (c > maxCup) ? c : maxCup;
                minCup = (c < minCup) ? c : minCup;

                Cup newCup = new Cup() { Label = c };

                if (firstCup == null)
                {
                    firstCup = newCup;
                    previousCup = newCup;
                }
                else
                {
                    previousCup.Next = newCup;
                    previousCup = newCup;
                }
                cupMap.Add(c, newCup);
            }

            for (var x = maxCup + 1; x <= 1000000; x++)
            {
                Cup newCup = new Cup() { Label = x };

                if (firstCup == null)
                {
                    firstCup = newCup;
                    previousCup = newCup;
                }
                else
                {
                    previousCup.Next = newCup;
                    previousCup = newCup;
                }
                cupMap.Add(x, newCup);
            }
            maxCup = totalCups;
            /* connect the last one to the first one */
            previousCup.Next = firstCup;
            currentCup = firstCup;
        }

        public void Move()
        {
            var chainStart = currentCup.Next;
            var chainEnd = chainStart.Next.Next;
            currentCup.Next = chainEnd.Next;

            var dest = currentCup.Label - 1;
            if (dest < minCup)
            {
                dest = maxCup;
            }
            while(!IsValidTarget(chainStart,dest))
            {
                dest--;
                if (dest < minCup)
                {
                    dest = maxCup;
                }
            }
            var target = cupMap[dest];
            var targetNext = target.Next;
            target.Next = chainStart;
            chainEnd.Next = targetNext;

            currentCup = currentCup.Next;
        }

        bool IsValidTarget(Cup chainStart, int target)
        {
            if (chainStart.Label == target)
            {
                return false;
            }
            if (chainStart.Next.Label == target)
            {
                return false;
            }
            if (chainStart.Next.Next.Label == target)
            {
                return false;
            }

            return true;
        }

        public void PrintCups()
        {
            Console.Write("(" + currentCup.Label + ") ");
            var nextCup = currentCup.Next;
            while (nextCup != currentCup)
            {
                Console.Write(nextCup.Label + " ");
                nextCup = nextCup.Next;
            }
            Console.WriteLine();
        }

        public string CupsStartingAt(int cupNumber)
        {
            StringBuilder sb = new();
            

            var stopCup = cupMap[cupNumber];
            var startingCup = stopCup.Next;

            sb.Append(startingCup.Label);
            var nextCup = startingCup.Next;
            while (nextCup != stopCup)
            {
                sb.Append(nextCup.Label);
                nextCup = nextCup.Next;
            }

            return sb.ToString();
        }
        public Cup GetCup(int cupNumber)
        {
            return cupMap[cupNumber];
        }
    }

    public class Cup
    {
        public int Label;
        public Cup Next;
    }


    public class Day_23 : BaseDay
    {
        public Day_23()
        {
            
        }

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
            CupGame g = new CupGame(InputParser.AsLine(InputFilePath),1000000);
            for (var x = 0; x < 10000000; x++)
            {
                g.Move();
            }
            var cup1 = g.GetCup(1);
            var cup2 = cup1.Next;
            var cup3 = cup2.Next;

            long ans = cup2.Label * (long)cup3.Label;

            return ans.ToString();
        }

    }
}
