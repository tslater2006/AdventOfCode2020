using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.IO;

namespace AdventOfCode
{

    public class Day_04 : BaseDay
    {
        string[] passports;
        List<Regex> fieldRegexes = new List<Regex>();
        public Day_04()
        {
            passports = InputParser.AsParagraphs(InputFilePath);
            passports = passports.Select(p => p.Replace("\r\n", " ")).ToArray();

            string[] required = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

            foreach(var r in required)
            {
                fieldRegexes.Add(new Regex($"({r}):([^ ]+)",RegexOptions.Singleline));
            }

        }

        public override string Solve_1()
        {
            var validCount = passports.Where(p => ValidatePassport(p,false)).Count();

            return validCount.ToString();
        }

        public override string Solve_2()
        {
            var validCount = passports.Where(p => ValidatePassport(p, true)).Count();

            return validCount.ToString();
        }

        bool ValidatePassport(string passport, bool validateData)
        {
            foreach(Regex r in fieldRegexes)
            {
                var match = r.Match(passport);
                if (match.Success)
                {
                    if (validateData)
                    {
                        var key = match.Groups[1].Value;
                        var value = match.Groups[2].Value;
                        if (!ValidateField(key, value))
                        {
                            return false;
                        }
                    }
                } else
                {
                    /* missing a required field */
                    return false;
                }

            }
            return true;
        }

        public bool ValidateField(string key, string value)
        {
            int parseResult;
            switch (key)
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

}
