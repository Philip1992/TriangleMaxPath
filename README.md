# TriangleMaxPath
Calculates the max possible sum going through the triangle from the top to any of the bottom nodes, and prints the sum and the found path.
The actual approach used is going from the bottom up.

Method: Ignore values that do not satisfy the odd/even rule. In this case it is done by setting these values to -1.
That means that the program will not be able to solve triangles with negatives as part of the max path. If this is necessary one could use null instead of -1. Or make an actual graph, and remove the nodes all together.
On top of this, ignore all values with 2 children that are already ignored.
This could also be done as we go through the triangle, but for the sake of simplicity it is done beforehand. 

We then traverse the tree from bottom to top, while we for each parent add the value of the largets child to that parent, and keep the path used up to said parent. Continue to do this while ignoring the nodes that have been deemed "invalid".

When at the top, the path saved for the top node will be the path with the highest possible value. Then print the sum of the values in the path along with the path itself.

The code is heavily commented and I hope it helps more than it hurts. 
