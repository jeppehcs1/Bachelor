using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
using System.Collections.Generic;
using System;
using System.Collections;  


public class OnePlusOneBitString(ProblemType<BitArray> problem) : OnePlusOne<BitArray>(problem)
{
    public override void Run()
    {                                                                  
        var dim = problem.dimension;
        var random = new Random();
        var old = searchPointString.Clone() as BitArray;
        
        for (var i = 0; i < dim; i++)
        {
            if (random.Next(dim) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                searchPointString[i] = !searchPointString[i];
            }
        }
        if (problem.Fitness(searchPointString) < problem.Fitness(old))
        {
            searchPointString = old;
        }
        Console.WriteLine(BitArrayToString(searchPointString));
    }

    public override int GetFitness()
    {
        return problem.Fitness(searchPointString);
    }
    public override void Initialize() 
    {
        var dim = problem.dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = random.Next(2) == 1;  // Random true or false
        }
        searchPointString = new BitArray(bits);
    }

}