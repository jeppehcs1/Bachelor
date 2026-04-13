using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int dimension) : ProblemType<TSPInstance>(dimension)
{

    public TSPInstance MutateTSP(TSPInstance instance,Random random)
    {
        
        int rand1 = random.Next(Dimension);
        int rand2 = random.Next(Dimension);
        while (rand1 == rand2)
        {
            rand2 = random.Next(Dimension);
        }

        var c = instance.Permutation;
        (c[rand1], c[rand2]) = (c[rand2], c[rand1]);

        return instance;
    }
    
}