using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class GraphProblem(int dimension) : IProblemType<TSPInstance>
{
    public int Dimension { get; set; } = dimension;
    public abstract int Fitness(TSPInstance c);
}