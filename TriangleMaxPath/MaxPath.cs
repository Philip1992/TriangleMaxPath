using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


namespace TriangleMaxPath
{
    /* Rules:
     * You will be given a triangle input (which is a directed graph) and you need to find the path that 
     * provides the maximum possible sum of the numbers per the given rules below: 
     * 1.You will start from the top and move downwards to the last possible node. 
     * 2.You must proceed by changing between even and odd numbers subsequently. Suppose that you are on 
     * an even number, the next number you choose must be odd, or if you are on an odd number the next 
     * number must be even. In other words, the final path would be Odd -> even -> odd -> even ... 
     * 3.You must reach to the bottom of the pyramid. 
     * 4.Assume that there is at least one valid path to the bottom. 
     * 5.If there are multiple paths, which result in the same maximum amount, you can choose any of them.
     */

        /* The rules state that I will start at the top and move downwards to the last possible node,
         * though I imagine that is just show the idea behind the problem, and to ensure the right output.
         * Since we do have access to the entire triangle right off the bat and I do not believe it changes
         * the complexity of the problem. And here I am not refering to the run time complexity. However, as 
         * far as I can tell the run time complexity should be considerably lower with this solution
         * compared to a top down approach.
         */

    class MaxPath
    {
        static void Main()
        {
            // Initialise file.
            // Assuming the file is of the same format as it appeared in the email.
            /*string filePath = Path.Combine(Environment.CurrentDirectory, @"Tests\Big.txt");

            StreamReader sr = new StreamReader(filePath);
            int lineCount = File.ReadLines(filePath).Count();

            int[][] triangle = new int[lineCount][];

            for (int i = 0; i < lineCount; i++)
            {
                String line = sr.ReadLine();
                line = line.Trim();
                triangle[i] = line.Split(" ").Select(int.Parse).ToArray();
            }
            sr.Close();
            */
            // End of initialisation 


            // I have mostly used the file, I just added the hardcoded version in case that is what you prefer,
            // and because I am not uploading the directory where I keep the file, to github.
            int[][] triangle = new int[][]
            {
                new int[] { 215 },
                new int[] { 192, 124 },
                new int[] { 117, 269, 442 },
                new int[] { 218, 836, 347, 235 },
                new int[] { 320, 805, 522, 417, 345 },
                new int[] { 229, 601, 728, 835, 133, 124 },
                new int[] { 248, 202, 277, 433, 207, 263, 257 },
                new int[] { 359, 464, 504, 528, 516, 716, 871, 182 },
                new int[] { 461, 441, 426, 656, 863, 560, 380, 171, 923 },
                new int[] { 381, 348, 573, 533, 448, 632, 387, 176, 975, 449 },
                new int[] { 223, 711, 445, 645, 245, 543, 931, 532, 937, 541, 444 },
                new int[] { 330, 131, 333, 928, 376, 733, 017, 778, 839, 168, 197, 197 },
                new int[] { 131, 171, 522, 137, 217, 224, 291, 413, 528, 520, 227, 229, 928 },
                new int[] { 223, 626, 034, 683, 839, 052, 627, 310, 713, 999, 629, 817, 410, 121 },
                new int[] { 924, 622, 911, 233, 325, 139, 721, 218, 253, 223, 107, 233, 230, 124, 233 },
            };

            // Determine the best path up through the tree.
            int[] bestPath = BestPath(triangle);

            Output(bestPath);

        }

        static int[] BestPath(int[][] triangle)
        {

            int[][] simplifiedTriangle = Simplify(triangle);

            // Depth of the triangle
            int len = triangle.Length;

            Dictionary<(int, int), List<int>> bestPathToPoint = new Dictionary<(int, int), List<int>>();
            // Using a new list for every node, to keep track of the best possible path to that node.
            // This does not include nodes that can never be part of a valid path.
            // It could be done in a more space effecient manor, if necessary, but as far as I can tell, 
            // it would need to be done from the top down, which would mean a lot more potential paths than 
            // what we are dealing with here.

            // Filling out the paths for the bottom nodes. They only consist of the themselves.
            for (int j = 0; j < triangle[len - 1].Length; j++)
            {
                List<int> list = new List<int>
                { simplifiedTriangle[len - 1][j] };
                bestPathToPoint.Add((len - 1, j), list);
            }

            // Going through the triangle bottom to top.
            for (int i = len - 1; i > 0; i--)
            {
                for (int j = 0; j < triangle[i].Length - 1; j++)
                {
                    // If the parent is not -1, continue
                    if (simplifiedTriangle[i - 1][j] != -1) 
                    {
                        // If the left child is greater than or equal to the right child, and it is not -1, choose that child.
                        if (simplifiedTriangle[i][j] >= simplifiedTriangle[i][j + 1] && simplifiedTriangle[i][j] != -1)
                        {
                            // Make list with the parent value.
                            List<int> list = new List<int>(bestPathToPoint[(i, j)])
                            { simplifiedTriangle[i - 1][j] };

                            // Add the optimal path up to the child to the list mentioned above.
                            bestPathToPoint.Add((i - 1, j), list);
                            // Add the value of the child to the parent
                            simplifiedTriangle[i - 1][j] += simplifiedTriangle[i][j];

                        } else 
                        // If the left child is greater than the right child, and not -1, choose this child instead. 
                        if (simplifiedTriangle[i][j] < simplifiedTriangle[i][j + 1] && simplifiedTriangle[i][j + 1] != -1)
                        {

                            List<int> list = new List<int>(
                            bestPathToPoint[(i, j + 1)])
                            { simplifiedTriangle[i - 1][j] };

                            // Add the optimal path up to the child to the list mentioned above.
                            bestPathToPoint.Add((i - 1, j), list);
                            // Add the value of the child to the parent
                            simplifiedTriangle[i - 1][j] += simplifiedTriangle[i][j + 1];
                        }
                        // If both children are -1, then we will just continue, however that should never happen, 
                        // as the parent would be -1 as well, in that case.
                    }
                }
            }

            return bestPathToPoint[(0, 0)].ToArray();
        }


        // Certain values in the triangle are sure to never be part of a valid path.
        // These are values that do not follow the odd/even rule. If two neighboring
        // children do not follow this rule, it is certain that their common parent cannot
        // be part of the final path either, since one of the children then would have
        // to be part of the path as well.
        // Other simplifications could be made as well if it was deemed necessary.
        // For instance, if a left most or right most child in a level of the triangle is 
        // invalid, it would make all left most/right most children below invalid as well.
        static int[][] Simplify(int[][] triangle)
        {
            int evenOrOdd = triangle[0][0] % 2; // Even = 0, odd = 1.
            int len = triangle.Length;

            for (int i = len-1; i >= 0; i--)
            {
                for (int j = 0; j <= triangle[i].Length -1; j++)
                {
                    // The code below can be combined to just make one if-statement,
                    // but I feel like this is somewhat easier to understand.
                    
                    // If the line number is even and the value of the node matches the
                    // value of the start value (as in if the start value is even then we 
                    // check if this value is even too, and the same goes for odd).
                    // Then everything is good and we just continue
                    if (i%2 == 0 && triangle[i][j]%2 == evenOrOdd)
                    {
                        // Valid number
                    }
                    else
                    // If the line number is odd and the value of the node does not match 
                    // the value of the start value.
                    // Then everything is good and we just continue
                    if (i%2 == 1 && triangle[i][j]%2 != evenOrOdd)
                    {
                        // Valid number
                    }
                    else
                    {
                        // Invalid number. 
                        // Replacing numbers that cannot be part of a valid path with -1.
                        // Could also be replace with null, or if it was an actual graph
                        // the node could be removed.
                        triangle[i][j] = -1;
                    }
                }
            }

            // If two neighboring children both are invalid, then so is their common parent.
            // That is what is done here.
            for (int i = len - 1; i >= 0; i--)
            {
                for (int j = 0; j < triangle[i].Length - 1; j++)
                {
                    if (triangle[i][j] == -1 && triangle[i][j+1] == -1)
                    {
                        triangle[i - 1][j] = -1;
                    }
                }
            }
            
            return triangle;
        }
        // Prints the Max sum and the max value path based on the found path.
        static void Output(int[] bestPath)
        {
            Console.WriteLine("Max sum: " + bestPath.Sum());

            string outputPath = "Path: " + bestPath[bestPath.Length - 1];
            for (int i = bestPath.Length - 2; i >= 0; i--)
            {
                outputPath += ", " + bestPath[i];
            }
            Console.WriteLine(outputPath);
        }
    }
}
