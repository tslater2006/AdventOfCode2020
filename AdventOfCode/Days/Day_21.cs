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
    class Food
    {
        public HashSet<string> Ingredients = new();
        public HashSet<string> Allergens = new();
    }


    public class Day_21 : BaseDay
    {
        List<Food> foodList = new();
        Dictionary<string, string> allergenToIngredientMap = new();
        public Day_21()
        {
            foreach(var line in InputParser.AsLines(InputFilePath))
            {
                
                var cleanLine = Regex.Replace(line, "[(,)]", "");
                var words = cleanLine.Split(" ");
                bool foundContains = false;

                var foodItem = new Food();

                foreach(var w in words)
                {
                    if (w == "contains")
                    {
                        foundContains = true;
                        continue;
                    }
                    if (foundContains)
                    {
                        foodItem.Allergens.Add(w);
                    } else
                    {
                        foodItem.Ingredients.Add(w);
                    }
                }

                foodList.Add(foodItem);
            }

        }

        public override string Solve_1()
        {
            HashSet<string> resolvedIngredients = new();

            Dictionary<string, List<string>> candidateMap = new();

            foreach (var f in foodList)
            {
                /* try to resolve each allergen */
                foreach (var allergen in f.Allergens)
                {
                    if (candidateMap.ContainsKey(allergen)) continue;

                    List<string> candidates = new List<string>(f.Ingredients);

                    foreach (var f2 in foodList.Where(z => z != f))
                    {
                        if (f2.Allergens.Contains(allergen))
                        {
                            candidates = candidates.Intersect(f2.Ingredients).ToList();
                        }
                    }

                    candidateMap.Add(allergen, candidates);

                }
            }

            /* reduce candidates */
            while (true)
            {
                foreach (var kv in candidateMap.Where(kv => allergenToIngredientMap.ContainsKey(kv.Key) == false && kv.Value.Where(v => resolvedIngredients.Contains(v) == false).Count() == 1))
                {
                    var key = kv.Key;
                    var val = kv.Value;

                    allergenToIngredientMap.Add(kv.Key, kv.Value.Where(v => resolvedIngredients.Contains(v) == false).First());
                    resolvedIngredients.Add(kv.Value.Where(v => resolvedIngredients.Contains(v) == false).First());
                }

                if (resolvedIngredients.Count == candidateMap.Count)
                {
                    break;
                }

            }

            var answer = 0;
            foreach (var f in foodList)
            {
                foreach(var i in f.Ingredients)
                {
                    answer += resolvedIngredients.Contains(i) ? 0 : 1;
                }
            }

            return "";
        }

        public override string Solve_2()
        {
            var badIngredients = allergenToIngredientMap.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray();
            return String.Join(',', badIngredients);
        }

    }
}
