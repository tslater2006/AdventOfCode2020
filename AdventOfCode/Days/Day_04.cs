using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace AdventOfCode
{
    public class Passport
    {
        public Dictionary<string, string> Fields = new Dictionary<string, string>();

        public bool IsValid(bool validateValues)
        {
            string[] required = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            string[] currentKeys = Fields.Keys.ToArray();
            foreach(var r in required)
            {
                if (currentKeys.Contains(r) == false)
                {
                    return false;
                } else
                {
                    if (validateValues)
                    {
                        if (ValidateField(r,Fields[r]) == false)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool ValidateField(string key, string value)
        {
            int parseResult;
            switch(key)
            {
                case "byr":
                    return (value.Length == 4 && int.TryParse(value, out parseResult) && (parseResult >= 1920 && parseResult <= 2002));
                case "iyr":
                    return (value.Length == 4 && int.TryParse(value, out parseResult) && (parseResult >= 2010 && parseResult <= 2020));
                case "eyr":
                    return (value.Length == 4 && int.TryParse(value, out parseResult) && (parseResult >= 2020 && parseResult <= 2030));
                case "hgt":
                    return (ValidateHeight(value));
                case "hcl":
                    Regex hexCode = new Regex("^#[0-9a-f]{6}$");
                    return hexCode.IsMatch(value);
                case "ecl":
                    string[] allowedColors = new string[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
                    return allowedColors.Contains(value);
                case "pid":
                    Regex passportID = new Regex("^\\d{9}$");
                    return passportID.IsMatch(value);
                case "cid":
                    return true;
                default:
                    int i = 3;
                    return false;

            }
        }

        private bool ValidateHeight(string hgt)
        {
            Regex heightReg = new Regex("^(\\d+)(cm|in)$");
            var match = heightReg.Match(hgt);
            if (match.Success == false) return false;

            var value = int.Parse(match.Groups[1].Value);
            var unit = match.Groups[2].Value;
            
            if (unit == "cm")
            {
                return (value >= 150 && value <= 193);                
            }
            else if (unit == "in")
            {
                return (value >= 59 && value <= 76);
            }
            else
            {
                /* not cm or in */
                return false;
            }
        }

    }
    public class Day_04 : BaseDay
    {
        private readonly string[] _input;
        List<Passport> passports;
        public Day_04()
        {
            _input = InputParser.AsLines(InputFilePath);
            passports = ParsePassports();
        }

        public override string Solve_1()
        {
            var validCount = passports.Where(p => p.IsValid(false)).Count();

            return validCount.ToString();
        }

        public override string Solve_2()
        {
            var validCount = passports.Where(p => p.IsValid(true)).Count();

            return validCount.ToString();
        }

        List<Passport> ParsePassports()
        {
            List<Passport> parsedList = new List<Passport>();
            Passport current = new Passport();

            for (var x = 0; x < _input.Length; x++)
            {
                var line = _input[x].Trim();
                if (line.Length > 0)
                {
                    var fields = line.Split(" ");
                    foreach (var f in fields)
                    {
                        var parts = f.Split(":");
                        current.Fields.Add(parts[0], parts[1]);
                    }
                }
                else
                {
                    parsedList.Add(current);
                    current = new Passport();
                }
            }
            parsedList.Add(current);
            return parsedList;
        }
    }

}
