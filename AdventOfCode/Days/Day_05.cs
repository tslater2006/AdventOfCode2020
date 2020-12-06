using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                /* shift everything to make room for next bit */
                seatID <<= 1;

                /* happy accident that 'B' and 'R' when modded by 7 and then that modded by 2 are 1
                 * and 'F' and 'L' when done the same are 0, this happens to map to the 1's and 0's we need */
                seatID += ((c % 7) % 2);

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
            
            var sum = 0;
            var min = 100000;
            var max = 0;

            /* in a single pass, we get the sum, the smallest number, and the largest number */
            foreach (var pass in boardingPasses)
            {
                var seat = GetSeatID(pass);
                if (seat > max)
                {
                    max = seat;
                }
                if (seat < min)
                {
                    min = seat;
                }
                sum += seat;
            }

            /*    sum of integers 1 to max
             *  - sum of integers 1 to min = sum of range (min to max)
             *  - sum of numbers present = index of missing number in the range
             *  + min = actual missing number
             */
            var mySeat = (max * (max + 1) / 2) - (min * (min + 1) / 2) - sum + min;


            return mySeat.ToString();
        }
    }
}
