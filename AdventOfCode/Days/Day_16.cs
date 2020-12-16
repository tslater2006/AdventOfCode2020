using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using AdventOfCode.Utilities;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode
{
    class TicketField
    {
        internal string FieldName;
        internal List<(int lower, int upper)> ValidRanges = new();
        internal int Column;
    }

    class Ticket
    {
        public Ticket(string values)
        {
            rawFieldValues = values.Split(",").Select(s => int.Parse(s)).ToArray();
        }

        public IEnumerable<(int column, int value)> GetInvalidFieldValues(List<TicketField> fields)
        {
            for (var x = 0; x < rawFieldValues.Length; x++)
            {
                int val = rawFieldValues[x];
                bool valueInvalid = true;
                foreach (var field in fields)
                {
                    foreach (var range in field.ValidRanges)
                    {
                        if (val >= range.lower && val <= range.upper)
                        {
                            valueInvalid = false;
                        }
                    }
                }

                if (valueInvalid)
                {
                    yield return (x, val);
                }
            }
        }

        public bool ColumnValidForField(int column, TicketField field)
        {
            int val = rawFieldValues[column];
            foreach (var range in field.ValidRanges)
            {
                if (val >= range.lower && val <= range.upper)
                {
                    return true;
                }
            }
            return false;
        }
    

        internal int[] rawFieldValues;
        internal List<TicketField> Fields = new List<TicketField>();
    }
    public class Day_16 : BaseDay
    {
        string[] _lines;
        Ticket myTicket;
        List<Ticket> nearbyTickets = new List<Ticket>();
        List<TicketField> fields = new List<TicketField>();
        Regex numberRange = new("(\\d+)-(\\d+)");
        public Day_16()
        {
            _lines = InputParser.AsLines(InputFilePath);
            
            foreach(var line in _lines)
            {
                if (line.Contains(" or "))
                {
                    /* field rule */
                    TicketField tf = new();
                    tf.FieldName = line[0..line.IndexOf(':')];
                    var rangeMatches = numberRange.Matches(line);
                    foreach(Match match in rangeMatches)
                    {
                        var lower = int.Parse(match.Groups[1].Value);
                        var upper= int.Parse(match.Groups[2].Value);
                        tf.ValidRanges.Add((lower, upper));
                    }
                    fields.Add(tf);
                    continue;
                }

                if (line.Contains(","))
                {
                    /* ticket rule */
                    if (myTicket == null)
                    {
                        myTicket = new Ticket(line);
                    } else
                    {
                        nearbyTickets.Add(new Ticket(line));
                    }
                }
            }
        }

        public override string Solve_1()
        {
            return nearbyTickets.Sum(t => t.GetInvalidFieldValues(fields).Sum(i => i.value)).ToString();
        }

        public override string Solve_2()
        {
            ResolveFields(nearbyTickets.Where(t => t.GetInvalidFieldValues(fields).Count() == 0).ToList());

            var departureFields = fields.Where(f => f.FieldName.StartsWith("departure")).ToList();

            long ans = 1;
            foreach (var f in departureFields)
            {
                ans *= myTicket.rawFieldValues[f.Column];
            }

            return ans.ToString();
        }

        private void ResolveFields(List<Ticket> validTickets)
        {
            /* init the dictionary/lists */
            Dictionary<int, List<TicketField>> candidateMap = new();
            for (var col = 0; col < myTicket.rawFieldValues.Length; col++)
            {
                candidateMap.Add(col, new List<TicketField>());
            }

            /* for every column */
            for (var col = 0; col < myTicket.rawFieldValues.Length; col++)
            {
                /* check each field rule we have */
                foreach (var f in fields)
                {
                    /* against every nearby ticket */
                    bool fieldPossible = true;
                    foreach (var t in validTickets)
                    {
                        if (!t.ColumnValidForField(col,f))
                        {
                            fieldPossible = false;
                            break;
                        }
                        
                    }

                    if (fieldPossible)
                    {
                        candidateMap[col].Add(f);
                    }
                }
            }
            HashSet<TicketField> resolvedFields = new();
            var remainingFields = candidateMap.Values.Where(v => v.Where(t => resolvedFields.Contains(t) == false).Count() > 1).ToList() ;
            while (true)
            {
                var resolvedCols = candidateMap.Where(kv => kv.Value.Count(t => !resolvedFields.Contains(t)) == 1).Select(kv => kv.Key).ToList();
                foreach(var col in resolvedCols)
                {
                    var resolvedField = candidateMap[col].Where(t => resolvedFields.Contains(t) == false).First();
                    resolvedField.Column = col;
                    resolvedFields.Add(resolvedField);
                }

                if (remainingFields.Count == 0)
                {
                    break;
                }

                remainingFields = candidateMap.Values.Where(v => v.Where(t => resolvedFields.Contains(t) == false).Count() > 1).ToList();
            }
        }

    }
}
