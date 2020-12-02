using AoCHelper;
using AdventOfCode.Inputs;
using System.Text.RegularExpressions;
using System.Linq;

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
            Regex format = new Regex("(\\d+)-(\\d+) (.): (.+)");

            var result = format.Match(pass);

            /* convert values to proper types */
            var minimum = int.Parse(result.Groups[1].Value);
            var maximum = int.Parse(result.Groups[2].Value);
            var reqChar = result.Groups[3].Value[0];
            var passText = result.Groups[4].Value.Trim();

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
                var passLength = passText.Length;
                var matchCount = 0;
                if (minimum <= passLength && passText[minimum - 1] == reqChar)
                {
                    matchCount++;
                }

                if (maximum <= passLength && passText[maximum - 1] == reqChar)
                {
                    matchCount++;
                }

                return matchCount == 1;
            }

        }

    }

}
