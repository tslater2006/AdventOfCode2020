using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Common
{
    public struct CPUState
    {
        internal int IP; /* instruction pointer */
        internal int Acc; /* Accumulator */
    }

    public struct CPUInstruction
    {
        internal InstructionType OpCode;
        internal int Operand1;
    }

    public enum InstructionType
    {
        NOP, ACC, JMP
    }

    public class GameConsole
    {
        public CPUState State = new CPUState();
        public CPUInstruction[] Instructions;

        public GameConsole(string[] programText)
        {
            Instructions = ParseProgram(programText);
        }

        public bool Step()
        {
            if (State.IP < Instructions.Length)
            {
                ExecuteInstruction(Instructions[State.IP]);
                return true;
            }

            return false;
        }

        public void ResetState()
        {
            State.IP = 0;
            State.Acc = 0;
        }

        private void ExecuteInstruction(CPUInstruction instr) 
        {
            switch(instr.OpCode)
            {
                case InstructionType.NOP:
                    State.IP += 1;
                    break;
                case InstructionType.ACC:
                    State.Acc += instr.Operand1;
                    State.IP += 1;
                    break;
                case InstructionType.JMP:
                    State.IP += instr.Operand1;
                    break;
            }
        }


        private CPUInstruction[] ParseProgram(string[] programText)
        {
            CPUInstruction[] instrs = new CPUInstruction[programText.Length];

            for (var x = 0; x < programText.Length; x++)
            {
                var curInstr = new CPUInstruction();
                var instrParts = programText[x].Split(" ");
                switch (instrParts[0])
                {
                    case "nop":
                        curInstr.OpCode = InstructionType.NOP;
                        break;
                    case "acc":
                        curInstr.OpCode = InstructionType.ACC;
                        curInstr.Operand1 = int.Parse(instrParts[1]);
                        break;
                    case "jmp":
                        curInstr.OpCode = InstructionType.JMP;
                        curInstr.Operand1 = int.Parse(instrParts[1]);
                        break;
                }
                instrs[x] = curInstr;
            }
            return instrs;
        }

    }
}
