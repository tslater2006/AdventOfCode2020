using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day_05 : BaseDay
    {
        string[] boardingPasses;
        public Day_05()
        {
            boardingPasses = InputParser.AsLines(InputFilePath);
        }

        /* Seat strings are basically binary strings where FBLR matches to 0101 respectively
         * Just decode the string into a number, treating it as binary
         * No need to split into row and column even since row * 8 + column matches the full number */
        private int GetSeatID(string boardingPass)
        {
            int seatID = 0;
            foreach (var c in boardingPass)
            {
                seatID <<= 1;
                switch (c)
                {
                    case 'B':
                        seatID++;
                        break;
                    case 'R':
                        seatID++;
                        break;
                }

            }
            return seatID;
        }

        public override string Solve_1()
        {
            var maxID = 0;

            maxID = boardingPasses.Select(b => GetSeatID(b)).Max();

            return maxID.ToString();
        }

        public override string Solve_2()
        {
            var seatIDs = boardingPasses.Select(b => GetSeatID(b)).OrderBy(b => b).ToArray();

            var mySeat = -1;

            for (var x = 0; x < seatIDs.Length - 1; x++)
            {
                if (seatIDs[x+1] != seatIDs[x] + 1)
                {
                    mySeat = seatIDs[x] + 1;
                    break;
                }
            }

            return mySeat.ToString();
        }
    }
}
