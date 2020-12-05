using AdventOfCode.Inputs;
using AoCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal record TreeNode(TreeNode Left, TreeNode Right, int Number);
    public class Day_05 : BaseDay
    {
        string[] boardingPasses;
        TreeNode rootRowNode;
        TreeNode rootColumnNode;
        public Day_05()
        {
            boardingPasses = InputParser.AsLines(InputFilePath);

            /* build out binary tree for row */
            rootRowNode = BuildTree(128);

            rootColumnNode = BuildTree(8);
        }

        private TreeNode BuildTree(int leafCount)
        {
            Queue<TreeNode> nodesLeft = new Queue<TreeNode>();
            for(var x = 0; x < leafCount; x++)
            {
                nodesLeft.Enqueue(new TreeNode(null,null,x));
            }

            while (nodesLeft.Count > 1)
            {
                /* pop 2, make a parent, push to the end */
                nodesLeft.Enqueue(new TreeNode(nodesLeft.Dequeue(), nodesLeft.Dequeue(),0));
            }

            return nodesLeft.Dequeue();
        }

        private (int Row,int Column) DecodeBoardingPass(string boardingPass)
        {
            var curNode = rootRowNode;
            /* decode the row */
            for(var x = 0; x < 7; x++)
            {
                switch (boardingPass[x])
                {
                    case 'F':
                        curNode = curNode.Left;
                        break;
                    case 'B':
                        curNode = curNode.Right;
                        break;
                }
            }

            var row = curNode.Number;

            curNode = rootColumnNode;
            for (var x = 7; x < 10; x++)
            {
                switch (boardingPass[x])
                {
                    case 'L':
                        curNode = curNode.Left;
                        break;
                    case 'R':
                        curNode = curNode.Right;
                        break;
                }
            }

            var column = curNode.Number;

            return (row,column);
        }

        public override string Solve_1()
        {
            var maxID = 0;

            foreach(var pass in boardingPasses)
            {
                var location = DecodeBoardingPass(pass);
                var seatID = location.Row * 8 + location.Column;

                if (seatID > maxID)
                {
                    maxID = seatID;
                }
            }

            return maxID.ToString();
        }

        public override string Solve_2()
        {
            SortedSet<int> seatIDs = new SortedSet<int>();

            foreach (var pass in boardingPasses)
            {
                var location = DecodeBoardingPass(pass);
                seatIDs.Add(location.Row * 8 + location.Column);
            }

            var sortedArray = seatIDs.ToArray();

            var mySeat = -1;

            for (var x = 0; x < sortedArray.Length - 1; x++)
            {
                if (sortedArray[x+1] != sortedArray[x] + 1)
                {
                    mySeat = sortedArray[x] + 1;
                    break;
                }
            }


            return mySeat.ToString();
        }
    }
}
