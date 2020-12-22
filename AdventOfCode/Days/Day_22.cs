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
    class CombatGame
    {
        HashSet<string> SeenHandsPlayer1 = new();
        HashSet<string> SeenHandsPlayer2 = new();
        public Queue<int> Player1 = null;
        public Queue<int> Player2 = null;
        bool Recursive;
        public CombatGame(Queue<int> p1, Queue<int> p2, bool recurse = false)
        {
            Player1 = new Queue<int>(p1);
            Player2 = new Queue<int>(p2);
            Recursive = recurse;
        }

        public int PlayUntilWin()
        {
            while (Player1.Count > 0 && Player2.Count > 0) {
                if (SeenHandsPlayer1.Contains(String.Join(',', Player1)) || SeenHandsPlayer2.Contains(String.Join(',', Player2)))
                {
                    /* player 1 wins */
                    return 1;
                }
                SeenHandsPlayer1.Add(String.Join(',', Player1));
                SeenHandsPlayer2.Add(String.Join(',', Player2));
                int p1Card = Player1.Dequeue();
                int p2Card = Player2.Dequeue();

                if (Recursive)
                {
                    /* ensure both players have enough cards */
                    if (Player1.Count >= p1Card && Player2.Count >= p2Card)
                    {
                        /* Recursive Combat! */
                        var player1NewHand = new Queue<int>(Player1.Take(p1Card));
                        var player2NewHand = new Queue<int>(Player2.Take(p2Card));

                        int winner = new CombatGame(player1NewHand, player2NewHand, true).PlayUntilWin();

                        if (winner == 1)
                        {
                            /* player 1 won the recursive round */
                            Player1.Enqueue(p1Card);
                            Player1.Enqueue(p2Card);
                        } else
                        {
                            Player2.Enqueue(p2Card);
                            Player2.Enqueue(p1Card);
                        }
                        continue;
                    }
                }

                /* either not playing recursive, or there wasn't enough cards to play recursive */
                if (p1Card > p2Card)
                {
                    Player1.Enqueue(p1Card);
                    Player1.Enqueue(p2Card);
                } else
                {
                    Player2.Enqueue(p2Card);
                    Player2.Enqueue(p1Card);
                }
            }

            return (Player1.Count > 0) ? 1 : 2;
        }
    }


    public class Day_22 : BaseDay
    {
        Queue<int> Player1 = new();
        Queue<int> Player2 = new();
        public Day_22()
        {
            var _lines = InputParser.AsLines(InputFilePath);
            Queue<int> activeQueue = Player1;

            for(var x = 1; x< _lines.Length;x++)
            {
                if (_lines[x] == "")
                {
                    x += 1;
                    activeQueue = Player2;
                }else
                {
                    activeQueue.Enqueue(int.Parse(_lines[x]));
                }
            }

        }

        public override string Solve_1()
        {
            var game = new CombatGame(Player1, Player2, false);
            int winner = game.PlayUntilWin();

            var answer = 0;
            var winningPlayer = winner == 1 ? game.Player1 : game.Player2;

            var reverseCards = winningPlayer.Reverse().ToArray();
            for(var x = 0; x < reverseCards.Length; x++)
            {
                answer += (x + 1) * reverseCards[x];
            }

            return answer.ToString();
        }

        public override string Solve_2()
        {
            var game = new CombatGame(Player1, Player2, true);
            int winner = game.PlayUntilWin();


            var answer = 0;
            var winningPlayer = winner == 1 ? game.Player1 : game.Player2;

            var reverseCards = winningPlayer.Reverse().ToArray();
            for (var x = 0; x < reverseCards.Length; x++)
            {
                answer += (x + 1) * reverseCards[x];
            }

            return answer.ToString();
        }

    }
}
