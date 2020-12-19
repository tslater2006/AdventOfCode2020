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
    public class Rule
    {
        public int ID;
        public string Terminal = null;
        public List<int> Alternate1 = null;
        public List<int> Alternate2 = null;
    }

    public class Day_19 : BaseDay
    {
        string[] _lines;
        Dictionary<int, Rule> rules = new();
        List<string> inputStrings = new List<string>();
        int part1MaxDepth = 0;
        public Day_19()
        {
            _lines = InputParser.AsLines(InputFilePath);

            var ruleParsing = true;
            foreach(var l in _lines)
            {
                if (l == "")
                {
                    ruleParsing = false;
                    continue;
                }

                if (ruleParsing)
                {
                    var rule = new Rule();
                    var idAndRule = l.Split(":").Select(s => s.Trim()).ToArray();
                    rule.ID = int.Parse(idAndRule[0]);

                    /* is this a terminal? */
                    if (idAndRule[1].StartsWith("\""))
                    {
                        rule.Terminal = idAndRule[1][1..^1];
                    }
                    else
                    {
                        var alternates = idAndRule[1].Trim().Split("|");
                        if (alternates.Length == 2)
                        {
                            rule.Alternate2 = alternates[1].Trim().Split(" ").Select(s => int.Parse(s)).ToList();
                        }
                        rule.Alternate1 = alternates[0].Trim().Split(" ").Select(s => int.Parse(s)).ToList();
                    }
                    rules.Add(rule.ID, rule);
                } else
                {
                    inputStrings.Add(l);
                }

            }

        }

        public string BuildRuleRegex(Rule r, int maxDepth = -1)
        {
            if (maxDepth == 0) return "";

            /* for part 1, maxDepth is -1 and descends, and we want to track how deep it goes */
            if (maxDepth < 0 && maxDepth < part1MaxDepth)
            {
                part1MaxDepth = maxDepth;
            }

            if (r.Terminal != null)
            {
                return r.Terminal;
            }
            StringBuilder sb = new StringBuilder();
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            
            foreach (var rule in r.Alternate1)
            {
                sb1.Append(BuildRuleRegex(rules[rule], maxDepth - 1));
            }
            if (r.Alternate2 != null)
            {
                /* has 2 alternates */
                foreach (var rule in r.Alternate2)
                {
                    sb2.Append(BuildRuleRegex(rules[rule], maxDepth - 1));
                }
            }

            if (sb1.Length > 0)
            {
                sb.Append("(");
                sb.Append(sb1);
                if (sb2.Length > 0)
                {
                    sb.Append('|');
                    sb.Append(sb2);
                }
                sb.Append(")");
            }
            return sb.ToString();
        }

        public override string Solve_1()
        {
            var regex = "^" + BuildRuleRegex(rules[0]) + "$";
            var reg = new Regex(regex);

            var count = inputStrings.Count(s => reg.IsMatch(s));

            return count.ToString();
        }

        public override string Solve_2()
        {
            /* change rule 8 */
            rules[8].Alternate2 = new List<int>() { 42, 8 };
            rules[11].Alternate2 = new List<int>() { 42, 11, 31};

            int lastCount = 0;
            /* you can put this to 15 and get the answer much faster, but for generic solution, best to keep at 0 */
            int curMax = Math.Abs(part1MaxDepth);
            while (true)
            {
                var regex = "^" + BuildRuleRegex(rules[0], curMax) + "$";

                var reg = new Regex(regex);

                var count = inputStrings.Count(s => reg.IsMatch(s));
                if (count > 0 && count == lastCount)
                {
                    break;
                }
                lastCount = count;

                curMax++;
            }
            return lastCount.ToString();
        }

    }
}
