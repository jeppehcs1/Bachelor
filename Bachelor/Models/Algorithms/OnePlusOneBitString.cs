using Bachelor.Models.Problems;

namespace Bachelor.Models.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;  

// author Jeppe and Clement
public class OnePlusOneBitString(ProblemType<BitArray> problem) : OnePlusOne<BitArray>(problem)
{

    public override BitArray CloneSearchPoint()
    {
        return SearchPoint.Clone() as BitArray;
    }
    public override void UpdateSearchPoint(BitArray old)
    {
        int newFitness = Problem.Fitness(SearchPoint);
        if (BSFF > newFitness) 
        {
            SearchPoint = old; // old is better
            return;
        }
        BSFF = newFitness;
    }

    
    public override void MutateSearchPoint()
    {
        
        for (var i = 0; i < Problem.Dimension; i++)
        {
            if (_random.Next(Problem.Dimension) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                SearchPoint[i] = !SearchPoint[i];
            }
        }
    }

    
    

    public override void InitializeCore()
    {
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            bits[i] = random.Next(2) == 1;  // Random true or false
        }
        SearchPoint = new BitArray(bits);
        BSFF = GetFitness();
    }
}