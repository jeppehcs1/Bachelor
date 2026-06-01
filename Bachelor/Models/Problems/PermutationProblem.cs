using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int dimension) : ProblemType<TSPInstance>(dimension)
{
    

    public int Fitness(TSPInstance c)
    {
        return base.Fitness(c);
    }
}