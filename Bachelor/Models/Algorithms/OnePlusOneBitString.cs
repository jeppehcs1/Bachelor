using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;  


public class OnePlusOneBitString(ProblemType<BitArray> problem) : OnePlusOne<BitArray>(problem)
{

    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    public override void UpdateSearchPoint(BitArray old)
    {
        if (Problem.Fitness(SearchPoint) < Problem.Fitness(old))
        {
            SearchPoint = old;
        }
    }

    public override void MutateSearchPoint(Random random)
    {
        var dim = Problem.dimension;
        for (var i = 0; i < dim; i++)
        {
            if (random.Next(dim) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                SearchPoint[i] = !SearchPoint[i];
            }
        }
    }

    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    public override void Initialize() 
    {
        var dim = Problem.dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = random.Next(2) == 1;  // Random true or false
        }
        SearchPoint = new BitArray(bits);
    }
    
}