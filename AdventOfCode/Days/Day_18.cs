using AoCHelper;
using System;
using AdventOfCode.Inputs;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using AdventOfCode.Utilities;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode
{
    enum TokenType
    {
        NUMBER, ADD, MULT, GROUP
    }

    record Token (TokenType Type, long Value = 0, string GroupString = null);
    public class Day_18 : BaseDay
    {
        string[] _lines;
        public Day_18()
        {
            _lines = InputParser.AsLines(InputFilePath);
        }

        List<Token> ParseExpression(string expr)
        {
            List<Token> result = new();
            var tokenStrings = expr.Split(" ");

            for (var x = 0; x < tokenStrings.Length; x++)
            {
                var str = tokenStrings[x];
                if (str is "*" or "+")
                {
                    result.Add(new Token(str == "*" ? TokenType.MULT : TokenType.ADD));
                    continue;
                }
                if (str.StartsWith("("))
                {
                    /* build out parenthetical group */
                    var openParenCount = str.Count(c => c == '(');
                    StringBuilder sb = new();
                    sb.Append(str).Append(' ');
                    while (openParenCount > 0)
                    {
                        var temp = tokenStrings[++x];
                        foreach (var c in temp)
                        {
                            if (c is '(') openParenCount++;
                            if (c is ')') openParenCount--;
                        }
                        sb.Append(temp).Append(' ');
                    }
                    result.Add(new Token(TokenType.GROUP, 0, sb.ToString().Trim()[1..^1]));
                } else
                {
                    result.Add(new Token(TokenType.NUMBER, long.Parse(str)));
                }
            }
            return result;
        }

        long EvaluateExpression(List<Token> tokens)
        {
            for (var x = 0; x < tokens.Count(); x++)
            {
                if (tokens[x].Type is TokenType.ADD or TokenType.MULT)
                {
                    var leftToken = tokens[x - 1];
                    var rightToken = tokens[x + 1];
                    var opToken = tokens[x];
                    if (leftToken.Type == TokenType.GROUP)
                    {
                        leftToken = new Token(TokenType.NUMBER, EvaluateExpression(ParseExpression(leftToken.GroupString)));
                    }

                    if (rightToken.Type == TokenType.GROUP)
                    {
                        rightToken = new Token(TokenType.NUMBER, EvaluateExpression(ParseExpression(rightToken.GroupString)));
                    }
                    Token newToken = null;
                    if (opToken.Type == TokenType.ADD)
                    {
                        newToken = new Token(TokenType.NUMBER, leftToken.Value + rightToken.Value);
                    }
                    else if (opToken.Type == TokenType.MULT)
                    {
                        newToken = new Token(TokenType.NUMBER, leftToken.Value * rightToken.Value);
                    }

                    tokens.RemoveAt(x + 1);
                    tokens.RemoveAt(x);
                    tokens.RemoveAt(x - 1);
                    tokens.Insert(x - 1, newToken);
                    x--;
                }
            }
            return tokens[0].Value;
        }

        long EvaluateExpressionWithPrecedence(List<Token> tokens)
        {
            TokenType[] precedenceOrder = new TokenType[] { TokenType.ADD, TokenType.MULT };
            foreach (var opType in precedenceOrder)
            {
                for (var x = 0; x < tokens.Count(); x++)
                {
                    if (tokens[x].Type == opType)
                    {
                        var leftToken = tokens[x - 1];
                        var rightToken = tokens[x + 1];
                        if (leftToken.Type == TokenType.GROUP)
                        {
                            leftToken = new Token(TokenType.NUMBER, EvaluateExpressionWithPrecedence(ParseExpression(leftToken.GroupString)));
                        }

                        if (rightToken.Type == TokenType.GROUP)
                        {
                            rightToken = new Token(TokenType.NUMBER, EvaluateExpressionWithPrecedence(ParseExpression(rightToken.GroupString)));
                        }
                        Token newToken = null;
                        if (opType == TokenType.ADD)
                        {
                            newToken = new Token(TokenType.NUMBER, leftToken.Value + rightToken.Value);
                        } else if (opType == TokenType.MULT)
                        {
                            newToken = new Token(TokenType.NUMBER, leftToken.Value * rightToken.Value);
                        }

                        tokens.RemoveAt(x + 1);
                        tokens.RemoveAt(x);
                        tokens.RemoveAt(x - 1);
                        tokens.Insert(x - 1, newToken);

                        x--;
                    }
                }
            }

            return tokens[0].Value;
        }

        public override string Solve_1()
        {
            long ans = 0;

            
            foreach(var l in _lines)
            {
                ans += EvaluateExpression(ParseExpression(l));
            }
            return ans.ToString();
        }

        public override string Solve_2()
        {
            long ans = 0;


            foreach (var l in _lines)
            {
                ans += EvaluateExpressionWithPrecedence(ParseExpression(l));
            }
            return ans.ToString();
        }

    }
}
