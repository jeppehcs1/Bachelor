using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int dimension) : ProblemType<TSPInstance>(dimension)
{

    public TSPInstance MutateTSP(TSPInstance instance)
    {
        var random = new Random();
        int rand1 = random.Next(dimension);
        int rand2 = random.Next(dimension);
        while (rand1 == rand2)
        {
            rand2 = random.Next(dimension);
        }

        var c = instance.Permutation;
        (c[rand1], c[rand2]) = (c[rand2], c[rand1]);

        return instance;
    }
}