using System;

namespace Bachelor.Models.Problems;

using System.Collections.Generic;

public abstract class PermutationProblem(int dimension) : ProblemType<TSPInstance>(dimension)
{

    public TSPInstance MutateTSP_2opt(TSPInstance instance,Random random)
    {
        
        int rand1 = random.Next(dimension);
        int rand2 = random.Next(dimension);
        while (rand1 == rand2)
        {
            rand2 = random.Next(dimension);
        }
        if(rand1>rand2) (rand1, rand2) = (rand2, rand1);
        
        instance.Permutation.Reverse(rand1+1, rand2-rand1);

        return instance;
    }
    
    public TSPInstance MutateTSP_3opt(TSPInstance instance,Random random)
    {
        
        int rand1 = random.Next(dimension);
        int rand2 = random.Next(dimension);
        int rand3 = random.Next(dimension);
        while (rand1 == rand2 || rand3 == rand1 || rand2 == rand3)
        {
            rand2 = random.Next(dimension);
            rand3 = random.Next(dimension);
        }

        var c = instance.Permutation;
        (c[rand1], c[rand2]) = (c[rand2], c[rand1]);

        return instance;
    }
}