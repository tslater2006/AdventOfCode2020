using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class Day_06 : BaseDay
    {
        string[] Groups;
        public Day_06()
        {
            Groups = InputParser.AsParagraphs(InputFilePath);
        }
        public override string Solve_1()
        {
            return Groups.Sum(g => g.Where(c => c >= 'a' && c <= 'z').Distinct().Count()).ToString();
        }

        public override string Solve_2()
        {
            var sum = 0;
            foreach (var g in Groups)
            {
                var people = g.Split("\r\n");
                List<char[]> groupAnswers = people.Select(p => p.ToCharArray()).ToList();

                IEnumerable<char> final = groupAnswers[0];

                for (var x = 1; x < groupAnswers.Count; x++)
                {
                    final = final.Intersect(groupAnswers[x]);
                }
                sum += final.Count();
            }
            return sum.ToString();
        }
    }
}
