using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode
{
    public class LuggageRule
    {
        public string Type;
        public string ContentString;
        public Dictionary<LuggageRule, int> Contents = new Dictionary<LuggageRule, int>();
        public List<LuggageRule> ContainedBy = new List<LuggageRule>();
    }


    public class Day_07 : BaseDay
    {
        string[] luggageRules;
        Dictionary<string, LuggageRule> ruleMap = new Dictionary<string, LuggageRule>();
        public Day_07()
        {
            luggageRules = InputParser.AsLines(InputFilePath);

            /* parse all luggage rules */

            Regex ruleStart = new Regex("(.*?) bags contain (.*)");
            Regex ruleContents = new Regex("(\\d+|no) (.*?) bag");

            /* first we make each rule and add to the dictionary. then we will process each rules contents */
            foreach (var r in luggageRules)
            {
                var match = ruleStart.Match(r);
                ruleMap.Add(match.Groups[1].Value, new LuggageRule() { Type = match.Groups[1].Value, ContentString = match.Groups[2].Value });
            }

            foreach (var rule in ruleMap.Values)
            {
                var matches = ruleContents.Matches(rule.ContentString);
                foreach (Match match in matches)
                {
                    var amount = match.Groups[1].Value;
                    var type = match.Groups[2].Value;
                    if (amount != "no")
                    {
                        var containedType = ruleMap[type];
                        rule.Contents.Add(ruleMap[type], int.Parse(amount));
                        containedType.ContainedBy.Add(rule);
                    }
                }
            }
        }
        public override string Solve_1()
        {
            var shinyGoldRule = ruleMap["shiny gold"];
            HashSet<string> possible = new HashSet<string>();

            Stack<LuggageRule> stack = new Stack<LuggageRule>();
            stack.Push(shinyGoldRule);

            while (stack.Count > 0)
            {
                foreach (var c in stack.Pop().ContainedBy)
                {
                    possible.Add(c.Type);
                    if (c.ContainedBy.Count > 0)
                    {
                        stack.Push(c);
                    }
                }
            }

            return possible.Count.ToString();
        }

        public override string Solve_2()
        {
            var shinyGoldRule = ruleMap["shiny gold"];

            var sum = GetCountForBag(shinyGoldRule);


            return sum.ToString();
        }
        static Dictionary<LuggageRule, int> memoize = new Dictionary<LuggageRule, int>();
        static long memoizeHit;
        static long memoizeMiss;
        public int GetCountForBag(LuggageRule r)
        {
            /* if we've already resolved this bag type, pull from cache */
            if (memoize.ContainsKey(r))
            {
                memoizeHit++;
                return memoize[r];
            }

            memoizeMiss++;
            
            /* otherwise, for each bag this bag contains, do "bags inside that bag + 1 
                * (to count the outer bag itself) times how many of this bag are included */
            var cost = 0;
            foreach (var c in r.Contents)
            {
                cost += c.Value * (GetCountForBag(c.Key) + 1);
            }

            /* cache the result for later lookups */
            memoize.Add(r, cost);
            return cost;

        }
    }
}
