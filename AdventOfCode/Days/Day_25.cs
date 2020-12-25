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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace AdventOfCode
{
    public class Day_25 : BaseDay
    {
        long[] PublicKeys;
        public Day_25()
        {
            PublicKeys = InputParser.AsLinesLong(InputFilePath).ToArray();
        }

        public override string Solve_1()
        {
            long loopSize1 = FindLoopSize(7,PublicKeys[0]);

            long ans = CalcEncryptionKey(PublicKeys[1], loopSize1);

            return ans.ToString();
        }

        long CalcEncryptionKey(long publicKey, long loopSize)
        {
            long curVal = 1;
            for(var x = 0; x < loopSize;x++)
            {
                curVal = (curVal * publicKey) % 20201227;
            }

            return curVal;
        }
        long FindLoopSize(long subjectNumber, long target)
        {
            int loopCount = 0;
            long curVal = 1;
            while (curVal != target)
            {
                curVal = (curVal * subjectNumber) % 20201227;
                loopCount++;
            }

            return loopCount;
        }

        public override string Solve_2()
        {
            return "Merry Christmas!";
        }
    }
}
