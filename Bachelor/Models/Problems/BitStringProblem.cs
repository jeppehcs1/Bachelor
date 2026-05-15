using System;
using System.Collections;
using System.Transactions;

namespace Bachelor.Models.Problems;

public abstract class BitStringProblem(int dimension) : IProblemType<BitArray>
{
    public int Dimension { get; set; } = dimension;
    public abstract int Fitness(BitArray c);

}