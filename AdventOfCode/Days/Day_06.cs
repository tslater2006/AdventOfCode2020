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
            Groups = File.ReadAllText(InputFilePath).Split("\r\n\r\n");
        }
        public override string Solve_1()
        {
            var sum = 0;
            foreach(var g in Groups)
            {
                HashSet<char> groupSet = new HashSet<char>();

                foreach(var c in g)
                {
                    if (c >= 'a' && c <= 'z' && groupSet.Contains(c) == false)
                    {
                        groupSet.Add(c);
                    }
                }

                sum += groupSet.Count;
            }


            return sum.ToString();
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
