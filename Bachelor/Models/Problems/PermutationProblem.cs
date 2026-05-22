using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int dimension) : ProblemType<TSPInstance>
{
    public int Dimension { get; set; } = dimension;

    public int Fitness(TSPInstance c)
    {
        return base.Fitness(c);
    }
}