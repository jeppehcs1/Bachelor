using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int Dimension) : IProblemType<TSPInstance>
{
    public int Dimension { get; set; }
    public abstract int Fitness(TSPInstance c);
}