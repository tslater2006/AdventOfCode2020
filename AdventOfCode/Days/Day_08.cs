using AdventOfCode.Common;
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
    public class Day_08 : BaseDay
    {
        GameConsole console;
        public Day_08()
        {
            console = new GameConsole(InputParser.AsLines(InputFilePath));
        }
        public override string Solve_1()
        {
            /* false if it looped, true if it halted */
            var runHalted = RunUntilLoopOrHalt(console);

            return console.State.Acc.ToString();
        }

        public override string Solve_2()
        {
            var answer = 0;
            for(var x = 0; x < console.Instructions.Length; x++)
            {
                console.ResetState();

                InstructionType oldOpCode = console.Instructions[x].OpCode;
                /* is current instruction a JMP or NOP? if so, flip it */
                switch(oldOpCode)
                {
                    case InstructionType.JMP:
                        console.Instructions[x].OpCode = InstructionType.NOP;
                        break;
                    case InstructionType.NOP:
                        console.Instructions[x].OpCode = InstructionType.JMP;
                        break;
                    default:
                        continue;
                }

                /* false if it looped, true if it halted */
                var runHalted = RunUntilLoopOrHalt(console);

                if (runHalted)
                {
                    answer = console.State.Acc;
                    break;
                }

                /* still loops, revert and try the next */
                console.Instructions[x].OpCode = oldOpCode;
                
            }


            return answer.ToString();
        }
     
        private bool RunUntilLoopOrHalt(GameConsole c)
        {
            HashSet<int> seenInstructions = new HashSet<int>();
            bool stepResult = true;
            while (stepResult && !seenInstructions.Contains(c.State.IP))
            {
                seenInstructions.Add(c.State.IP);
                stepResult = c.Step();
            }

            return !stepResult;
        }

    }
}
