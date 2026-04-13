using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public class TSPProblem(int dimension) : PermutationProblem(dimension)
{

    internal int EuclidianDistance((int, int) p1, (int, int) p2)
    {
        var (x1, y1) = p1;
        var (x2, y2) = p2;

        int xDist = x2 - x1;
        int yDist = y2 - y1;
        
        // Rounds to nearest integer
        return (int) Math.Round(Math.Sqrt(xDist * xDist + yDist * yDist));
    }
    
    public override int Fitness(TSPInstance c)
    {
        int fitness = 0;
        for (int i = 1; i < Dimension; i++)
        {
            (int, int) p1 = c.Graph[c.Permutation[i-1]];
            (int, int) p2 = c.Graph[c.Permutation[i]];

            fitness = fitness + EuclidianDistance(p1, p2);
        }
        fitness += EuclidianDistance(c.Graph[c.Permutation[Dimension - 1]], c.Graph[c.Permutation[0]]);
        return fitness;
    }


    
}