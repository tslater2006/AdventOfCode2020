using AoCHelper;
using AdventOfCode.Inputs;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace AdventOfCode
{
    public class Day_02 : BaseDay
    {
        private readonly string[] _input;

        public Day_02()
        {
            _input = InputParser.AsLines(InputFilePath);
        }

        public override string Solve_1()
        {
            return _input.Where(p => PasswordValid(p, true)).Count().ToString();
        }

        public override string Solve_2()
        {
            return _input.Where(p => PasswordValid(p, false)).Count().ToString();
        }

        private bool PasswordValid(string pass, bool part1)
        {
            /* Extract the password rule and password */

            /* Regex is the proper way to do it, but for this input, gross string
             * splitting is actually a lot faster, inputs are well formed too
            /*
            Regex format = new Regex("(\\d+)-(\\d+) (.): (.+)");
            var result = format.Match(pass);
            var minimum = int.Parse(result.Groups[1].Value);
            var maximum = int.Parse(result.Groups[2].Value);
            var reqChar = result.Groups[3].Value[0];
            var passText = result.Groups[4].Value.Trim();
            */

            var leftRight = pass.Split(":");
            var range = leftRight[0].Split("-");
            var minimum = int.Parse(range[0]);
            var maxReq = range[1].Split(" ");
            var maximum = int.Parse(maxReq[0]);
            var reqChar = maxReq[1][0];
            var passText = leftRight[1].Trim();
            

            if (part1)
            {
                /* part 1 says that a password needs to have >= min and <= max of reqchar */
                var occursCount = passText.Where(c => c == reqChar).Count();
                return occursCount >= minimum && occursCount <= maximum;
            }
            else
            {
                /* part 2 says the rule is actual that the reqChar needs to be at exactly
                 * one of the positions [minimum] or [maximum], 1-indexed 
                 */
                return (passText[minimum - 1] == reqChar) ^ (passText[maximum - 1] == reqChar);

            }

        }

    }

}
