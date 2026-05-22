using System;
using System.Collections;
using System.Transactions;

namespace Bachelor.Models.Problems;

public abstract class BitStringProblem(int dimension) : ProblemType<BitArray>
{
    public int Dimension { get; set; } = dimension;

    public int Fitness(BitArray c)
    {
        return base.Fitness(c);
    }

}