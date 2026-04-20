using System;
using System.Collections;
using System.Transactions;

namespace Bachelor.Models.Problems;

public abstract class BitStringProblem(int dimension) : ProblemType<BitArray>(dimension)

{
    public BitArray MutateBitArray(BitArray searchPoint, Random random)
    {

        for (var i = 0; i < Dimension; i++)
        {
            if (random.Next(Dimension) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                searchPoint[i] = !searchPoint[i];
            }
        }
        return searchPoint;
    }
}