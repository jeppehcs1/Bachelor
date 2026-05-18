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
    public override int UpdateSearchPoint(BitArray old)
    {
        if (Problem.Fitness(SearchPoint) < Problem.Fitness(old))
        {
            SearchPoint = old;
            return 1;
        }
        return 0;
    }

    
    public override void MutateSearchPoint()
    {
        //SearchPoint = ((BitStringProblem)Problem).MutateBitArray(SearchPoint, random);
        
        for (var i = 0; i < problem.Dimension; i++)
        {
            if (_random.Next(problem.Dimension) == 0) // 1/dim chance of being 0, i.e. flipping a bit
            {
                SearchPoint[i] = !SearchPoint[i];
            }
        }
        //return SearchPoint;
    }

    public override int GetFitness()
    {
        return Problem.Fitness(SearchPoint);
    }
    public override void Initialize() 
    {
        var dim = Problem.Dimension;
        var bits = new bool[dim];
        var random = new Random();
        
        for (int i = 0; i < dim; i++)
        {
            //bits[i] = random.Next(2) == 1;  // Random true or false
            bits[i] = false;
        }
        SearchPoint = new BitArray(bits);
    }
    
}